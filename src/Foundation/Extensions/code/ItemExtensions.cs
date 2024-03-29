using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;
using System;

namespace DreamTeam.Foundation.Extensions
{
    public static class ItemExtensions
    {
        public static bool InheritsFrom(this Item item, ID templateId)
        {
            return item.Template.DoesTemplateInheritFrom(templateId);
        }

        public static bool IsTemplateItem(this Item item)
        {
            return item.TemplateID == TemplateIDs.Template || (item.Name.Equals("__Standard Values", StringComparison.InvariantCultureIgnoreCase) && item.Paths.Path.StartsWith("/sitecore/templates"));
        }

        public static bool DoesTemplateInheritFrom(this TemplateItem template, ID templateId)
        {
            return template != null && !templateId.IsNull && (template.ID == templateId || template.DoesTemplateInheritFrom(TemplateManager.GetTemplate(templateId, template.Database)));
        }

        private static bool DoesTemplateInheritFrom(this TemplateItem template, Template baseTemplate)
        {
            if (baseTemplate == null)
                return false;

            return TemplateManager.GetTemplate(template.ID, template.Database).DescendsFromOrEquals(baseTemplate.ID);
        }
    }
}