namespace DreamTeam.Foundation.Security
{
    using DreamTeam.Foundation.Security.CustomAuthSystemCore;
    using DreamTeam.Foundation.Security.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;

    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            ServiceCollectionServiceExtensions.AddTransient<IExternalAuthorizationService, ExternalAuthorizationService>(serviceCollection);
            ServiceCollectionServiceExtensions.AddSingleton<ISecurityModelFromExternalServer, SecurityModelFromFakeExternalServer>(serviceCollection);

            ServiceCollectionServiceExtensions.AddSingleton<IEASConfigurationService, EASConfigurationService>(serviceCollection);
        }
    }
}