namespace Amss.Boilerplate.Api.Common.Adapters
{
    using System;

    using ServiceStack.Logging;

    using LogManager = global::Common.Logging.LogManager;

    internal class ServiceStackLogFactoryAdapter : ILogFactory
    {
        #region Public Methods and Operators

        public ILog GetLogger(Type type)
        {
            return new LogAdapter(LogManager.GetLogger(type));
        }

        public ILog GetLogger(string typeName)
        {
            return new LogAdapter(LogManager.GetLogger(typeName));
        }

        #endregion

        private class LogAdapter : ILog
        {
            #region Constants and Fields

            private readonly global::Common.Logging.ILog logger;

            #endregion

            #region Constructors and Destructors

            public LogAdapter(global::Common.Logging.ILog logger)
            {
                this.logger = logger;
            }

            #endregion

            #region Public Properties

            public bool IsDebugEnabled
            {
                get
                {
                    return this.logger.IsDebugEnabled;
                }
            }

            #endregion

            #region Public Methods and Operators

            public void Debug(object message)
            {
                this.logger.Debug(message);
            }

            public void Debug(object message, Exception exception)
            {
                this.logger.Debug(message, exception);
            }

            public void DebugFormat(string format, params object[] args)
            {
                this.logger.DebugFormat(format, args);
            }

            public void Error(object message)
            {
                this.logger.Error(message);
            }

            public void Error(object message, Exception exception)
            {
                this.logger.Error(message, exception);
            }

            public void ErrorFormat(string format, params object[] args)
            {
                this.logger.ErrorFormat(format, args);
            }

            public void Fatal(object message)
            {
                this.logger.Fatal(message);
            }

            public void Fatal(object message, Exception exception)
            {
                this.logger.Fatal(message, exception);
            }

            public void FatalFormat(string format, params object[] args)
            {
                this.logger.FatalFormat(format, args);
            }

            public void Info(object message)
            {
                this.logger.Info(message);
            }

            public void Info(object message, Exception exception)
            {
                this.logger.Info(message, exception);
            }

            public void InfoFormat(string format, params object[] args)
            {
                this.logger.InfoFormat(format, args);
            }

            public void Warn(object message)
            {
                this.logger.Warn(message);
            }

            public void Warn(object message, Exception exception)
            {
                this.logger.Warn(message, exception);
            }

            public void WarnFormat(string format, params object[] args)
            {
                this.logger.WarnFormat(format, args);
            }

            #endregion
        }
    }
}