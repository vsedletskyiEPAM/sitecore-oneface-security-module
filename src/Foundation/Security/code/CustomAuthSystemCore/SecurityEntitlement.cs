namespace DreamTeam.Foundation.Security.CustomAuthSystemCore
{
    using Sitecore.Diagnostics;
    using Sitecore.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;
    using DreamTeam.Foundation.Security.Model;
    using Sitecore.Security.Accounts;

    public static class SecurityEntitlement
    {
        private static readonly ISecurityModelFromExternalServer _securityModelServerRequester;

        private static readonly object _lockObj;

        static SecurityEntitlement()
        {
            _securityModelServerRequester = ServiceLocator.ServiceProvider.GetService<ISecurityModelFromExternalServer>();

            Assert.ArgumentNotNull(_securityModelServerRequester, nameof(_securityModelServerRequester));

            _lockObj = new object();
        }

        public static SecurityEntitlementModel GetSecurityModelByUserId(User user)
        {
            Assert.ArgumentNotNull(user, nameof(user));

            lock (_lockObj)
            {
                return TransferRequestToFakeSecurityEntitlementModel(user) ?? new SecurityEntitlementModel();
            }
        }

        private static SecurityEntitlementModel TransferRequestToFakeSecurityEntitlementModel(User user)
        {
            Assert.ArgumentNotNull(user, nameof(user));

            return _securityModelServerRequester.GetSecuirtyEntitiesByUserId(user);
        }
    }
}