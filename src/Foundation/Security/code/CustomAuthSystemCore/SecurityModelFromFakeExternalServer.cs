namespace DreamTeam.Foundation.Security.CustomAuthSystemCore
{
    using DreamTeam.Foundation.Security.Entities;
    using DreamTeam.Foundation.Security.Model;
    using Sitecore.Diagnostics;
    using Sitecore.Security.Accounts;
    using System.Collections.Generic;

    public class SecurityModelFromFakeExternalServer : ISecurityModelFromExternalServer
    {
        private static Dictionary<string, SecurityEntitlementModel> _fakeData => new Dictionary<string, SecurityEntitlementModel>()
        {
            {"regular_user", new SecurityEntitlementModel()
                                {
                                    Entities = new List<EntityModel>
                                    {
                                        new EntityModel() { Urn = "Product 1", IsAllowed = false },
                                        new EntityModel() { Urn = "Product 2", IsAllowed = true },
                                        new EntityModel() { Urn = "Product 3", IsAllowed = true },
                                    }
                                }
            },
            {"admin_user", new SecurityEntitlementModel()
                                {
                                    Entities = new List<EntityModel>
                                    {
                                        new EntityModel() { Urn = "Product 1", IsAllowed = true },
                                        new EntityModel() { Urn = "Product 2", IsAllowed = true },
                                        new EntityModel() { Urn = "Product 3", IsAllowed = true },
                                    }
                                }
            }
        };
        /// <summary>
        /// This method implement ISecurityModelFromExternalServer iterface.
        /// This method call external auth system, get response and convert security model to 'SecurityEntitlementModel' class, magically :)
        /// </summary>
        /// <param name="userIdentity"></param>
        /// <returns></returns>
        public SecurityEntitlementModel GetSecuirtyEntitiesByUserId(User user)
        {
            //TODO: Request to external system
            //TODO: Getting response from external system

            try
            {
                //TODO: Convert response object to 'SecurityEntitlementModel' class

                //Return fake data based on User name
                return _fakeData[user.LocalName];
            }
            catch
            {
                Log.Error($"Exception over fetching data from External System", this);
            }

            return null;
        }
    }
}