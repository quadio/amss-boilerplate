namespace Amss.Boilerplate.Common.Exceptions
{
    using System;
    using System.Diagnostics.Contracts;

    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

    public static class ExceptionHelper
    {
        public static Exception TransformException(this Exception ex, string policyName)
        {
            Contract.Assert(policyName != null);

            var exceptionToProcess = ex;
            if (ex != null)
            {
                Exception exceptionToThrow;
                var rethrow = ExceptionPolicy.HandleException(ex, policyName, out exceptionToThrow);
                if (rethrow && exceptionToThrow != null)
                {
                    exceptionToProcess = exceptionToThrow;
                }
            }

            return exceptionToProcess;
        }
    }
}
