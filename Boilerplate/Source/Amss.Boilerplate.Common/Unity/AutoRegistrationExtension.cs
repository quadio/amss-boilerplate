namespace Amss.Boilerplate.Common.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Microsoft.Practices.Unity;

    using global::Unity.AutoRegistration;

    public static class AutoRegistrationExtension
    {
        public static void WellKnownITypeNameAutoRegistration(this UnityContainerExtension parent, string value)
        {
            parent.WellKnownITypeNameAutoRegistration<TransientLifetimeManager>(value);
        }

        public static void WellKnownITypeNameAutoRegistration<T>(this UnityContainerExtension parent, string value) 
            where T : LifetimeManager, new()
        {
            Contract.Assert(parent != null);
            parent
                .ConfigureSelfAutoRegistration()
                .IncludeWellKnownITypeName<T>(value)
                .ApplyAutoRegistration();
        }

        public static IAutoRegistration ConfigureSelfAutoRegistration(this UnityContainerExtension parent)
        {
            Contract.Assert(parent != null);
            Contract.Assert(parent.Container != null);

            return parent.Container.ConfigureSelfAutoRegistration(parent.GetType());
        }

        public static IAutoRegistration ConfigureSelfAutoRegistration(this UnityContainerExtension parent, params Type[] types)
        {
            Contract.Assert(parent != null);
            Contract.Assert(parent.Container != null);

            var list = new List<Type>(types) { parent.GetType() };
            return parent.Container.ConfigureSelfAutoRegistration(list.ToArray());
        }

        public static IAutoRegistration ConfigureSelfAutoRegistration<T>(this IUnityContainer container)
        {
            Contract.Assert(container != null);
            return container.ConfigureSelfAutoRegistration(typeof(T));
        }

        public static IAutoRegistration IncludeWellKnownITypeName(this IAutoRegistration registration, string value)
        {
            return registration.IncludeWellKnownITypeName<TransientLifetimeManager>(value);
        }

        public static IAutoRegistration IncludeWellKnownITypeName<T>(this IAutoRegistration registration, string value) 
            where T : LifetimeManager, new()
        {
            Contract.Assert(registration != null);
            registration.Include(
                type => type.ImplementsWellKnownITypeName(value),
                Then.Register().UsingLifetime<T>().AsITypeName());
            return registration;
        }

        public static bool ImplementsWellKnownITypeName(this Type type, string value)
        {
            Contract.Assert(type != null);
            return type.ImplementsITypeName() && type.Name.EndsWith(value);
        }

        public static IFluentRegistration AsITypeName(this IFluentRegistration registration)
        {
            Contract.Assert(registration != null);
            registration.As(type => type.GetInterfaces().Single(i => i.Name.StartsWith("I") && i.Name.Remove(0, 1) == type.Name));
            return registration;
        }

        private static IAutoRegistration ConfigureSelfAutoRegistration(this IUnityContainer container, params Type[] types)
        {
            Contract.Assert(container != null);
            Contract.Assert(types != null);
            var assemblies = (from t in types select t.Assembly).ToArray();
            var result = container.ConfigureAutoRegistration()
                .ExcludeAssemblies(asm => !assemblies.Contains(asm));
            return result;
        }
    }
}