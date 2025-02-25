using Grooki.MiniMods.Shared;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using ModMenu.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniRx;

namespace Grooki.MiniMods.Settings
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    internal static class SettingsManager
    {
        #region Fields

        private const string RootKey = "grooki.minimods";

        #endregion Fields

        #region Methods

        public static void Postfix()
        {
            var categoryNames = new Dictionary<SettingCategory, LocalizedString>()
            {
                { SettingCategory.General, Localization.CreateString("minimods.general.name", "General") },
                { SettingCategory.Bugfixes, Localization.CreateString("minimods.bugfixes.name", "Bugfixes") },
                { SettingCategory.ImprovedCantrips, Localization.CreateString("minimods.improvedcantrips.name", "Improved Cantrips") },
                { SettingCategory.Tailwinds, Localization.CreateString("minimods.tailwinds.name", "Tailwinds") },
            };

            var builder = SettingsBuilder.New(RootKey, Localization.CreateString("minimods.name", "Mini Mods"));
            builder.SetMod(Main.ModEntry, false, true);

            var settings = typeof(SettingsManager).Assembly.GetTypes()
                .SelectMany(t => t.GetProperties(BindingFlags.Static | BindingFlags.Public))
                .Select(ReflectionSetting.Create)
                .Where(i => i != null)
                .ToList();

            foreach (var propertyGroup in settings.GroupBy(tuple => tuple.Category).OrderBy(group => group.Key))
            {
                if (propertyGroup.Key != SettingCategory.General)
                {
                    builder.AddAnotherSettingsGroup(RootKey + propertyGroup.Key.ToString().ToLower(), categoryNames[propertyGroup.Key]);
                }
                foreach (var setting in propertyGroup.OrderBy(setting => setting.Order).ThenBy(setting => setting.Name))
                {
                    setting.AddToBuilder(builder);
                }
            }

            ModMenu.ModMenu.AddSettings(builder);

            settings.ForEach(setting => setting.LoadValue());
        }

        #endregion Methods
    }
}