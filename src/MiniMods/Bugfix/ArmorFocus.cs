using Grooki.MiniMods.Settings;
using HarmonyLib;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Items;

namespace Grooki.MiniMods.Bugfix
{
    [HarmonyPatch(typeof(ItemEntityArmor), nameof(ItemEntityArmor.ArmorType))]
    [SettingCategory(SettingCategory.Bugfixes)]
    internal static class ArmorFocus
    {
        #region Properties

        [Setting("Mithril Armor Feats")]
        [SettingDescription("If enabled, mithril armor will count as its base armor type for the purposes of feats like armor focus.")]
        public static bool Enabled { get; set; }

        #endregion Properties

        #region Methods

        public static void Postfix(ref ArmorProficiencyGroup __result, ItemEntityArmor __instance)
        {
            if (Enabled)
            {
                __result = __instance.Blueprint.ProficiencyGroup;
            }
        }

        #endregion Methods
    }
}