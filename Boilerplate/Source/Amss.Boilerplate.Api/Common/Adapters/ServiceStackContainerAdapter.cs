namespace Amss.Boilerplate.Api.Common.Adapters
{
    using Microsoft.Practices.Unity;

    using ServiceStack.Configuration;

    internal class ServiceStackContainerAdapter : IContainerAdapter
    {
        #region Constants and Fields

        private readonly IUnityContainer container;

        #endregion

        #region Constructors and Destructors

        public ServiceStackContainerAdapter(IUnityContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public Methods and Operators

        public T Resolve<T>()
        {
            var result = this.container.Resolve<T>();
            return result;
        }

        public T TryResolve<T>()
        {
            var result = default(T);
            if (this.container.IsRegistered<T>())
            {
                result = this.container.Resolve<T>();
            }

            return result;
        }

        #endregion
    }
}