namespace Amss.Boilerplate.Business.Exceptions
{
    using System;

    [Serializable]
    public class ValidationFailureInfo
    {
        #region Constructors and Destructors

        public ValidationFailureInfo(
            string propertyName, 
            string error, 
            string errorCode = null, 
            object attemtedValue = null, 
            object customState = null)
        {
            this.PropertyName = propertyName;
            this.ErrorMessage = error;
            this.ErrorCode = errorCode;
            this.AttemptedValue = attemtedValue;
            this.CustomState = customState;
        }

        #endregion

        #region Public Properties

        public object AttemptedValue { get; private set; }

        public object CustomState { get; private set; }

        public string ErrorCode { get; private set; }

        public string ErrorMessage { get; private set; }

        public string PropertyName { get; private set; }

        #endregion

        #region Public Methods and Operators

        public override string ToString()
        {
            return this.ErrorMessage;
        }

        #endregion
    }
}