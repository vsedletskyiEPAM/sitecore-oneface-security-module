using Sitecore.Data.Items;
using Sitecore.Security.Accounts;

namespace DreamTeam.Foundation.Security.Providers
{
    public interface IExternalAuthorizationSystemProvider
    {
        bool IsUserAuthorizedToGetSpecificItemAccess(Item entity, User user);
    }
}
