namespace Amss.Boilerplate.Common.Exceptions
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class ObjectNotFoundException : RootException
    {
        #region Constructors

        public ObjectNotFoundException(long id, Type type, Exception ex)
            : this(ToMessage(id, type), ex)
        {
            Contract.Assert(type != null);
            this.ObjectId = id;
            this.ObjectType = type.Name;
        }

        public ObjectNotFoundException()
        {
        }

        public ObjectNotFoundException(string message)
            : base(message)
        {
        }

        public ObjectNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ObjectNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.ObjectId = info.GetInt32("ObjectId");
            this.ObjectType = info.GetString("ObjectType");
        }

        #endregion

        #region Properties

        public long ObjectId { get; set; }

        public string ObjectType { get; set; }

        #endregion

        #region Methods

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ObjectId", this.ObjectId);
            info.AddValue("ObjectType", this.ObjectType);
        }

        private static string ToMessage(long id, Type type)
        {
            Contract.Assert(type != null);
            var message = string.Format(
                    CultureInfo.InvariantCulture,
                    "Can not load object {0} by id {1}",
                    type.Name,
                    id);
            return message;
        }

        #endregion
    }
}