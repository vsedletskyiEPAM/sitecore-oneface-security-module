namespace DreamTeam.Foundation.Security.CustomAuthSystemCore
{
    using DreamTeam.Foundation.Security.Model;
    using Sitecore.Security.Accounts;

    public interface ISecurityModelFromExternalServer
    {
        SecurityEntitlementModel GetSecuirtyEntitiesByUserId(User user);
    }
}