namespace DreamTeam.Foundation.Security.Services
{
    using Sitecore.Configuration;

    public class EASConfigurationService : IEASConfigurationService
    {
        private readonly bool _EASFeatureEnabled = Settings.GetBoolSetting("EASFeatureEnabled", false);

        public bool IsEASFeatureEnabled()
        {
            return _EASFeatureEnabled;
        }
    }
}