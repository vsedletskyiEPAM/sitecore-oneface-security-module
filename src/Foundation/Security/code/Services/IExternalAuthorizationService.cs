using Sitecore.Security.AccessControl;
using Sitecore.Security.Accounts;

namespace DreamTeam.Foundation.Security.Services
{
    public interface IExternalAuthorizationService
    {
        AccessResult GetAccess(ISecurable entity, Account account);
    }
}