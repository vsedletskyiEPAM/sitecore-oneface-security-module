namespace DreamTeam.Foundation.Security.Model
{
    using DreamTeam.Foundation.Security.Entities;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SecurityEntitlementModel
    {
        public List<EntityModel> Entities { get; set; }
    }
}