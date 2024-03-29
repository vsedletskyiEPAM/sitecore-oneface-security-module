using DreamTeam.Foundation.Security.CustomAuthSystemCore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using System;
using System.Linq;

namespace DreamTeam.Foundation.Security.Providers
{
    public class ExternalAuthorizationSystemProvider : IExternalAuthorizationSystemProvider
    {
        public bool IsUserAuthorizedToGetSpecificItemAccess(Item entity, User user)
        {
            Assert.ArgumentNotNull(entity, "entity");
            Assert.ArgumentNotNull(user, "user");

            //Only Admin use has full granted access
            if (user.IsAdministrator)
            {
                Log.Audit($"[ExternalAuthorizationSystemProvider]:: user is admin", this);
                return true;
            }

            var externalSecurityModel = SecurityEntitlement.GetSecurityModelByUserId(user);

            //return decidion based on external security model and mapped with 'Urn' field value
            return externalSecurityModel?.Entities?
                        .FirstOrDefault(securityModel => entity[Templates._ExternalId.Fields.Urn]
                        .StartsWith(securityModel.Urn, StringComparison.InvariantCultureIgnoreCase))?.IsAllowed ?? false;
        }
    }
}