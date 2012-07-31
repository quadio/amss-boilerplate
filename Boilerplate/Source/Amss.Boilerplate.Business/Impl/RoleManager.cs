namespace Amss.Boilerplate.Business.Impl
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using Amss.Boilerplate.Business.Impl.Validation;
    using Amss.Boilerplate.Common.Exceptions;
    using Amss.Boilerplate.Common.Utils;
    using Amss.Boilerplate.Data;
    using Amss.Boilerplate.Data.Specifications;

    internal class RoleManager : ManagerBase<RoleEntity>, IRoleManager
    {
        public override RoleEntity Create(RoleEntity instance)
        {
            Contract.Assert(instance != null);
            this.DemandValid<RoleValidator>(instance);
            return base.Create(instance);
        }

        public RoleEntity Load(string name)
        {
            Contract.Assert(name != null);
            var instance = this.Repository.FindOne(new RoleByName(name));
            if (instance == null)
            {
                throw new ObjectNotFoundException("Unable to load role by name {0}".Fmt(name));
            }

            return instance;
        }

        public override void Update(RoleEntity instance)
        {
            Contract.Assert(instance != null);
            this.DemandValid<RoleValidator>(instance);
            base.Update(instance);
        }
    }
}