namespace Amss.Boilerplate.Business.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [Serializable]
    public class BusinessValidationException : BusinessException
    {
        #region Constructors and Destructors

        public BusinessValidationException()
            : this(string.Empty, "Unspecified validation failure.")
        {
        }

        public BusinessValidationException(string message)
            : this(string.Empty, message)
        {
        }

        public BusinessValidationException(string message, Exception inner)
            : this(string.Empty, message, null, inner)
        {
        }

        public BusinessValidationException(string propertyName, string error, string errorCode = null, Exception innerException = null)
            : this(new[] { new ValidationFailureInfo(propertyName, error, errorCode) }, innerException)
        {
        }

        public BusinessValidationException(params ValidationFailureInfo[] errors)
            : this((IList<ValidationFailureInfo>)errors)
        {
        }

        public BusinessValidationException(IList<ValidationFailureInfo> errors, Exception innerException = null)
            // ReSharper disable PossibleMultipleEnumeration
            : base(BuildErrorMesage(errors), innerException)
        {
            this.Errors = errors;
            // ReSharper restore PossibleMultipleEnumeration
        }

        protected BusinessValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Public Properties

        public IEnumerable<ValidationFailureInfo> Errors { get; private set; }

        #endregion

        #region Methods

        private static string BuildErrorMesage(IEnumerable<ValidationFailureInfo> errors)
        {
            var arr = (from x in errors where x != null && x.ErrorMessage != null select "\r\n -- " + x.ErrorMessage).ToArray<string>();
            return "Validation failed: " + string.Join(string.Empty, arr);
        }

        #endregion
    }
}