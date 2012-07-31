namespace Amss.Boilerplate.Persistence.Impl.Utilities.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;

    internal static class ExpressionExtender
    {
        #region Public Methods and Operators

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", 
            Justification = "As designed")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", 
            Justification = "As designed")]
        public static LambdaExpression AddAccessor<T, TResult>(this Expression<Func<T, TResult>> prop, string property)
        {
            var expr = prop.Body is MemberExpression ? prop.Body : ((UnaryExpression)prop.Body).Operand;
            var accessed = Expression.PropertyOrField(expr, property);
            var call = Expression.Lambda(accessed, prop.Parameters);
            return call;
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", 
            Justification = "As designed")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", 
            Justification = "As designed")]
        public static Expression<Func<T, bool>> AddEquals<T, TResult>(
            this Expression<Func<T, TResult>> prop, object value, Type type = null)
        {
            var expression = prop as LambdaExpression;
            return AddEquals<T>(expression, value, type);
        }

        public static Expression<Func<T, bool>> AddEquals<T>(this LambdaExpression prop, object value, Type type = null)
        {
            var expr = prop.Body is MemberExpression ? prop.Body : ((UnaryExpression)prop.Body).Operand;
            var constant = type == null || value == null ? Expression.Constant(value) : Expression.Constant(value, type);
            var binaryExpression = Expression.Equal(expr, constant);
            return Expression.Lambda<Func<T, bool>>(binaryExpression, prop.Parameters);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", 
            Justification = "As designed")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", 
            Justification = "As designed")]
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var newY =
                new ExpressionRewriter().Substitute(expression2.Parameters[0], expression1.Parameters[0]).Inline()
                .Apply(expression2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expression1.Body, newY), expression1.Parameters);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", 
            Justification = "As designed")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", 
            Justification = "As designed")]
        public static string GetFullPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> exp)
        {
            var result = string.Empty;
            MemberExpression memberExp;
            if (TryFindMemberExpression(exp.Body, out memberExp))
            {
                var memberNames = new Stack<string>();
                do
                {
                    memberNames.Push(memberExp.Member.Name);
                }
                while (TryFindMemberExpression(memberExp.Expression, out memberExp));

                result = string.Join(".", memberNames.ToArray());
            }

            return result;
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", 
            Justification = "At designed")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", 
            Justification = "At designed")]
        public static string GetPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> exp)
        {
            var result = string.Empty;
            MemberExpression memberExp;
            return TryFindMemberExpression(exp.Body, out memberExp) ? memberExp.Member.Name : result;
        }

        public static string GetPropertyName<T>(this Expression<Func<T, object>> exp)
        {
            return GetPropertyName<T, object>(exp);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", 
            Justification = "As designed")]
        public static Type GetPropertyType(this Type type, string fullPropertyName)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var parts = fullPropertyName.Split('.');
            var path = fullPropertyName;
            var root = type;

            if (parts.Length > 1)
            {
                path = parts[parts.Length - 1];
                parts = parts.TakeWhile((p, i) => i < parts.Length - 1).ToArray();
                var path2 = string.Join(".", parts);
                root = type.GetPropertyType(path2);
            }

            return root.GetProperty(path).PropertyType;
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", 
            Justification = "As designed")]
        public static T GetPropertyValue<T>(this object value, string fullPropertyName)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var parts = fullPropertyName.Split('.');
            var path = fullPropertyName;
            var root = value;

            if (parts.Length > 1)
            {
                path = parts[parts.Length - 1];
                parts = parts.TakeWhile((p, i) => i < parts.Length - 1).ToArray();
                var path2 = string.Join(".", parts);
                root = value.GetPropertyValue<object>(path2);
                if (root == null)
                {
                    return default(T);
                }
            }

            var sourceType = root.GetType();
            return (T)sourceType.GetProperty(path).GetValue(root, null);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", 
            Justification = "As designed")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", 
            Justification = "As designed")]
        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var newY =
                new ExpressionRewriter().Substitute(expression2.Parameters[0], expression1.Parameters[0]).Inline()
                .Apply(expression2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expression1.Body, newY), expression1.Parameters);
        }

        #endregion

        #region Methods

        private static bool IsConversion(Expression exp)
        {
            return exp.NodeType == ExpressionType.Convert || exp.NodeType == ExpressionType.ConvertChecked;
        }

        private static bool TryFindMemberExpression(Expression exp, out MemberExpression memberExp)
        {
            var result = false;
            memberExp = exp as MemberExpression;
            if (memberExp != null)
            {
                // heyo! that was easy enough
                result = true;
            }
            else
            {
                // if the compiler created an automatic conversion,
                // it'll look something like...
                // obj => Convert(obj.Property) [e.g., int -> object]
                // OR:
                // obj => ConvertChecked(obj.Property) [e.g., int -> long]
                // ...which are the cases checked in IsConversion
                if (IsConversion(exp))
                {
                    var unaryExpression = exp as UnaryExpression;
                    if (unaryExpression != null)
                    {
                        memberExp = unaryExpression.Operand as MemberExpression;
                        if (memberExp != null)
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        #endregion
    }
}