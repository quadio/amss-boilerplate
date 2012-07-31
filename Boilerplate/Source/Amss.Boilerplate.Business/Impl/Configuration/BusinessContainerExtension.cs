namespace Amss.Boilerplate.Business.Impl.Configuration
{
    using Amss.Boilerplate.Common.Security;
    using Amss.Boilerplate.Common.Unity;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
    using Microsoft.Practices.Unity;

    using Unity.AutoRegistration;

    public class BusinessContainerExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.WellKnownITypeNameAutoRegistration<ContainerControlledLifetimeManager>(WellKnownAppParts.Manager);
            this.Container.RegisterType<IApplicationPrincipal, ThreadApplicationPrincipal>(new ContainerControlledLifetimeManager());

            var configurationSource = new DictionaryConfigurationSource();
            var builder = new ConfigurationSourceBuilder();
            builder.ConfigureCryptography()
                .EncryptUsingHashAlgorithmProviderNamed(UserManager.HashInstance)
                .WithOptions
                    .UsingHashAlgorithm<Zetetic.Security.Pbkdf2Hash>()
                /*.SetAsDefault()*/; // do not want Pbkdf2Hash (low speed algorithm) to be default
            builder.UpdateConfigurationWithReplace(configurationSource);

            var configurator = new UnityContainerConfigurator(this.Container);
            EnterpriseLibraryContainer.ConfigureContainer(configurator, configurationSource);
        }
    }
}