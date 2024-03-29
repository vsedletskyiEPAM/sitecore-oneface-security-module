using System;

namespace DreamTeam.Foundation.Security.Entities
{
    [Serializable]
    public class EntityModel
    {
        public string Urn { get; set; }

        public bool IsAllowed { get; set; }
    }
}