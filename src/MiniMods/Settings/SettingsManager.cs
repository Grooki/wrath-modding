using Grooki.MiniMods.Shared;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Localization;
using ModMenu.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
                { SettingCategory.Rebalance, Localization.CreateString("minimods.rebalance.name", "Rebalance") },
                { SettingCategory.Bugfixes, Localization.CreateString("minimods.bugfixes.name", "Bugfixes") },
            };

            var builder = SettingsBuilder.New(RootKey, Localization.CreateString("minimods.name", "Mini Mods"));
            builder.SetMod(Main.ModEntry, false, true);
            builder.SetModAuthor("Grooki");

            var settings = typeof(SettingsManager).Assembly.GetTypes()
                .SelectMany(t => t.GetProperties(BindingFlags.Static | BindingFlags.Public))
                .Select(ReflectionSetting.Create)
                .Where(i => i != null)
                .ToList();

            foreach (var settingCategory in settings.GroupBy(tuple => tuple.Category).OrderBy(group => group.Key))
            {
                if (settingCategory.Key != SettingCategory.General)
                {
                    builder.AddAnotherSettingsGroup(RootKey + settingCategory.Key.ToString().ToLower(), categoryNames[settingCategory.Key]);
                }
                //Settings with no group
                foreach (var setting in settingCategory.Where(i => i.Group is null).OrderBy(setting => setting.Order).ThenBy(setting => setting.Name))
                {
                    setting.AddToBuilder(builder);
                }
                //Grouped settings
                foreach(var settingGroup in settingCategory.Where(i => i.Group != null).GroupBy(i => i.Group).OrderBy(group => group.Key))
                {
                    //Add header
                    var settingKey = Regex.Replace(settingGroup.Key.ToLowerInvariant(), @"\s+", "");
                    var name = Localization.CreateString($"minimods.{settingCategory.Key.ToString().ToLowerInvariant()}.{settingKey}.name", settingGroup.Key);
                    builder.AddSubHeader(name);
                    foreach(var setting in settingGroup.OrderBy(setting => setting.Order).ThenBy(setting => setting.Name))
                    {
                        setting.AddToBuilder(builder);
                    }
                }
            }

            ModMenu.ModMenu.AddSettings(builder);

            settings.ForEach(setting => setting.LoadValue());
        }

        #endregion Methods
    }
}