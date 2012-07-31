namespace Amss.Boilerplate.Api.Common
{
    using System;

    using Amss.Boilerplate.Api.Configuration;
    using Amss.Boilerplate.Common.Exceptions;

    using ServiceStack.ServiceInterface;

    internal abstract class AppRestServiceBase<T> : RestServiceBase<T>
    {
        #region Methods

        protected override object HandleException(T request, Exception ex)
        {
            var transformed = ex.TransformException(ApiContainerExtension.DefaultPolicy);
            return base.HandleException(request, transformed);
        }

        #endregion
    }
}