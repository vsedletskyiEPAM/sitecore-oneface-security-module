using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.AccessControl;
using Sitecore.Security.Accounts;
using DreamTeam.Foundation.Extensions;
using DreamTeam.Foundation.Security.Providers;
using Sitecore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DreamTeam.Foundation.Security.Services
{
    public class ExternalAuthorizationService : IExternalAuthorizationService
    {
        /// <summary>
        /// Take place for different security auth providers under this variable
        /// </summary>
        private static IExternalAuthorizationSystemProvider _externalAuthorizationProvider => ServiceLocator.ServiceProvider.GetService<IExternalAuthorizationSystemProvider>();

        public AccessResult GetAccess(ISecurable entity, Account account)
        {
            Assert.ArgumentNotNull(account, "account");

            if (entity is Item entityItem && entityItem.InheritsFrom(Templates._EncourageByEntitlements.ID) && !entityItem.IsTemplateItem())
            {
                //TODO: Possible to place caching mehanism

                var user = (account as User) ?? User.FromName(account.Name, false);

                Log.Info($"[ExternalAuthorizationService]:: username: {user.Name} or user.LocalName: {user.LocalName}", this);

                var isPermit = _externalAuthorizationProvider.IsUserAuthorizedToGetSpecificItemAccess(entityItem, user);

                Log.Info($"[ExternalAuthorizationService]:: permit: {isPermit}", this);
                var accessResult = isPermit ? new AccessResult(AccessPermission.Allow, new AccessExplanation(Constants.GrantAccessExplanationByEAS))
                                            : new AccessResult(AccessPermission.Deny, new AccessExplanation(Constants.DenyAccessExplanationByEAS));

                //TODO: place accessResult to the cache for faster exit from code

                return accessResult;
            }

            return new AccessResult(AccessPermission.NotSet, new AccessExplanation("Skiped by External Authorization system due to lack of entitlement restrictions or no data for particular item."));
        }
    }
}