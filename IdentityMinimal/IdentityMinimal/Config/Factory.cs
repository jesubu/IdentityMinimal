using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Services.InMemory;
using IdentityServer3.EntityFramework;

namespace IdentityMinimal.Config
{
    class Factory
    {
        public static IdentityServerServiceFactory Configure()
        {
            var factory = new IdentityServerServiceFactory();

            var serviceOptions = new EntityFrameworkServiceOptions { ConnectionString = "Identity" };
            factory.RegisterOperationalServices(serviceOptions);
            factory.RegisterConfigurationServices(serviceOptions);

            var scopeStore = new InMemoryScopeStore(Scopes.Get());
            factory.ScopeStore = new Registration<IScopeStore>(scopeStore);

            var clientStore = new InMemoryClientStore(Clients.Get());
            factory.ClientStore = new Registration<IClientStore>(clientStore);

            factory.UserService = new Registration<IUserService>(UserServiceFactory.Create());
            //factory.UserService = new Registration<IUserService, CustomLoginPageUserService>(UserServiceFactory.Create());

            factory.ClaimsProvider = new Registration<IClaimsProvider>(typeof(IdentityClaimsProvider));

            factory.CorsPolicyService = new Registration<ICorsPolicyService>(new DefaultCorsPolicyService { AllowAll = true });

            return factory;
        }

    }
}
