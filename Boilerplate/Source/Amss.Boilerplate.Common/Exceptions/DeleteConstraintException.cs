namespace Amss.Boilerplate.Common.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DeleteConstraintException : RootException
    {
        #region Constructors and Destructors

        public DeleteConstraintException()
        {
        }

        public DeleteConstraintException(string message)
            : base(message)
        {
        }

        public DeleteConstraintException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected DeleteConstraintException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}