namespace Amss.Boilerplate.Common.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.Unity;

    public class UnitOfWorkLifetimeManager : LifetimeManager
    {
        #region Constants and Fields

        [ThreadStatic]
        private static bool enabled;

        [ThreadStatic]
        private static Dictionary<Guid, object> values;

        private readonly Guid key = Guid.NewGuid();

        #endregion

        #region Public Methods and Operators

        public override object GetValue()
        {
            object result;
            EnsureValues();
            values.TryGetValue(this.key, out result);
            return result;
        }

        public override void RemoveValue()
        {
            EnsureValues();
            values.Remove(this.key);
        }

        public override void SetValue(object newValue)
        {
            EnsureValues();
            if (!enabled)
            {
                throw new InvalidOperationException("UnitOfWork not started.");
            }

            var prevValue = this.GetValue();
            if (prevValue != null)
            {
                var disposable = prevValue as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }

            values[this.key] = newValue;
        }

        #endregion

        #region Methods

        internal static void Clear()
        {
            if (values != null)
            {
                foreach (var value in values.Values.OfType<IDisposable>())
                {
                    value.Dispose();
                }

                values.Clear();
            }
        }

        internal static void Disable()
        {
            if (!enabled)
            {
                throw new InvalidOperationException("Already disabled!");
            }

            enabled = false;
        }

        internal static void Enable()
        {
            if (enabled)
            {
                throw new InvalidOperationException("Already enabled!");
            }

            enabled = true;
        }

        private static void EnsureValues()
        {
            if (values == null)
            {
                values = new Dictionary<Guid, object>();
            }
        }

        #endregion
    }
}