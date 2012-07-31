namespace Amss.Boilerplate.Api.Common
{
    using System;
    using System.Diagnostics.Contracts;

    using Amss.Boilerplate.Common;
    using Amss.Boilerplate.Common.Security;
    using Amss.Boilerplate.Common.Transactions;

    using ServiceStack.Common;
    using ServiceStack.Common.Web;
    using ServiceStack.ServiceHost;
    using ServiceStack.ServiceInterface;

    using ApplicationIdentity = Amss.Boilerplate.Common.Security.ApplicationIdentity;

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    internal class UnitOfWorkAttribute : Attribute, IHasRequestFilter, IHasResponseFilter
    {
        #region Constants and Fields

        private const ApplyTo TransactionalMethods = ApplyTo.Delete | ApplyTo.Post | ApplyTo.Put;

        #endregion

        #region Explicit Interface Properties

        int IHasRequestFilter.Priority
        {
            get
            {
                return 1 + (int)RequestFilterPriority.RequiredPermission;
            }
        }

        int IHasResponseFilter.Priority
        {
            get
            {
                return 1;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void RequestFilter(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            Contract.Assert(req != null);
            Contract.Assert(req.Items != null);
            new RequestState(req).Start();
        }

        public void ResponseFilter(IHttpRequest req, IHttpResponse res, object response)
        {
            new RequestState(req).End(response);
        }

        #endregion

        #region Nested types

        private class RequestState
        {
            #region Constants and Fields

            private const string TransactionKey = "Transaction";

            private const string UnitOfWorkKey = "UnitOfWork";

            private readonly IHttpRequest req;

            #endregion

            #region Constructors and Destructors

            public RequestState(IHttpRequest req)
            {
                Contract.Assert(req != null);
                Contract.Assert(req.Items != null);
                this.req = req;
            }

            #endregion

            #region Properties

            private bool IsTransactional
            {
                get
                {
                    var httpMethod = this.req.HttpMethodAsApplyTo();
                    var result = TransactionalMethods.Has(httpMethod);
                    return result;
                }
            }

            private Transaction Transaction
            {
                get
                {
                    var transaction = (Transaction)this.req.Items[TransactionKey];
                    return transaction;
                }

                set
                {
                    if (value != null)
                    {
                        Contract.Assert(!this.req.Items.ContainsKey(TransactionKey));
                        this.req.Items.Add(TransactionKey, value);
                    }
                    else
                    {
                        this.req.Items.Remove(TransactionKey);
                    }
                }
            }

            private UnitOfWork UnitOfWork
            {
                get
                {
                    var unitOfWork = (UnitOfWork)this.req.Items[UnitOfWorkKey];
                    return unitOfWork;
                }

                set
                {
                    if (value != null)
                    {
                        Contract.Assert(!this.req.Items.ContainsKey(UnitOfWorkKey));
                        this.req.Items.Add(UnitOfWorkKey, value);
                    }
                    else
                    {
                        this.req.Items.Remove(UnitOfWorkKey);
                    }
                }
            }

            #endregion

            #region Public Methods and Operators

            public void Start()
            {
                this.UnitOfWork = new UnitOfWork();
                if (this.IsTransactional)
                {
                    this.Transaction = new Transaction();
                }

                IApplicationPrincipal principal;
                var session = this.req.GetSession();
                if (session != null && session.IsAuthenticated)
                {
                    var identity = new ApplicationIdentity(
                        long.Parse(session.UserAuthId),
                        session.UserName, 
                        session.DisplayName,
                        session.Email);
                    principal = new ApplicationPrincipal(
                        identity, 
                        session.Roles.ToArray(), 
                        session.Permissions.ToArray());
                }
                else
                {
                    principal = ApplicationPrincipal.Anonymous;
                }

                System.Threading.Thread.CurrentPrincipal = principal;
            }

            public void End(object response)
            {
                try
                {
                    if (this.IsTransactional)
                    {
                        this.CompleteTransaction(response);
                    }
                }
                finally
                {
                    this.UnitOfWork.Dispose();
                    this.UnitOfWork = null;
                }
            }
             
            #endregion

            #region Methods

            private void CompleteTransaction(object response)
            {
                try
                {
                    if (!response.IsErrorResponse())
                    {
                        this.Transaction.Complete();
                    }
                }
                finally
                {
                    this.Transaction.Dispose();
                    this.Transaction = null;
                }
            }

            #endregion
        }

        #endregion
    }
}