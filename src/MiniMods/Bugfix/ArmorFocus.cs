using Grooki.MiniMods.Settings;
using Grooki.MiniMods.Shared;
using HarmonyLib;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.UnitLogic.ActivatableAbilities.Restrictions;
using System.Collections.Generic;
using System.Linq;

namespace Grooki.MiniMods.Bugfix
{
    [HarmonyPatch]
    [SettingCategory(SettingCategory.Bugfixes)]
    internal static class ArmorFocus
    {
        #region Properties

        [Setting("Armor Focus for Mithril Armor")]
        [SettingDescription("If enabled, mithril armor will count as its base armor type for the purposes of armor focus and mythic armor focus.")]
        public static bool Enabled { get; set; }

        #endregion Properties

        #region Methods

        private static bool CheckArmor(UnitEntityData unit, ArmorProficiencyGroup armorProficiency)
        {
            UnitBody body = unit.Body;
            if (!RestrictionsHelper.CheckHasArmor(unit)) return false;

            return body.Armor.Armor.Blueprint.ProficiencyGroup == armorProficiency;
        }

        private static bool CheckArmor(UnitEntityData unit, ArmorProficiencyGroupFlag armorProficiency)
        {
            UnitBody body = unit.Body;
            if (!RestrictionsHelper.CheckHasArmor(unit)) return false;

            ArmorProficiencyGroup armorProficiencyGroup = body.Armor.Armor.Blueprint.ProficiencyGroup;

            return ((uint)(1 << (int)armorProficiencyGroup) & (uint)armorProficiency) != 0;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Kingmaker.Designers.Mechanics.Buffs.ArmorFocus), nameof(Kingmaker.Designers.Mechanics.Buffs.ArmorFocus.CheckArmor))]
        private static bool FixArmorFocus(Kingmaker.Designers.Mechanics.Buffs.ArmorFocus __instance)
        {
            if (!Enabled) return true;

            if (CheckArmor(__instance.Owner, __instance.ArmorCategory))
            {
                __instance.ActivateModifier();
            }
            else
            {
                __instance.DeactivateModifier();
            }
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(HasArmorFeatureUnlock), nameof(HasArmorFeatureUnlock.CheckEligibility))]
        private static bool FixArmorFeatureUnlock(HasArmorFeatureUnlock __instance)
        {
            if (!Enabled) return true;

            bool hasArmorType = !__instance.FilterByBlueprintArmorTypes || RestrictionsHelper.CheckArmor(__instance.Owner, __instance.m_BlueprintArmorTypes);
            bool hasArmorGroup = !__instance.FilterByArmorProficiencyGroup || CheckArmor(__instance.Owner, __instance.m_ArmorProficiencyGroupEntries);
            bool hasShield = !__instance.m_DisableWhenHasShield || !__instance.Owner.Body.SecondaryHand.HasShield;
            if (hasArmorType && hasArmorGroup && hasShield)
            {
                __instance.AddFact();
            }
            else
            {
                __instance.RemoveFact();
            }

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UnitArmor), nameof(UnitArmor.CheckCondition))]
        private static bool FixArmorCondition(UnitArmor __instance, ref bool __result)
        {
            if (!Enabled) return true;

            if (__instance.Unit == null || !__instance.Unit.TryGetValue(out var value))
            {
                __result = false;
                return true;
            }

            var activeCategories = new HashSet<ArmorProficiencyGroup>();

            if (RestrictionsHelper.CheckHasArmor(value))
            {
                activeCategories.Add(value.Body.Armor.Armor.Blueprint.ProficiencyGroup);
            }
            if (value.Body.PrimaryHand != null && value.Body.PrimaryHand.HasShield)
            {
                activeCategories.Add(value.Body.PrimaryHand.Shield.ArmorComponent.Blueprint.ProficiencyGroup);
            }
            if (value.Body.SecondaryHand != null && value.Body.SecondaryHand.HasShield)
            {
                activeCategories.Add(value.Body.SecondaryHand.Shield.ArmorComponent.Blueprint.ProficiencyGroup);
            }

            __result = true;

            if (__instance.IncludeArmorCategories?.Length > 0 && __instance.IncludeArmorCategories.All(i => !activeCategories.Contains(i)))
            {
                __result = false;
            }

            if (__instance.ExcludeArmorCategories?.Length > 0 && __instance.ExcludeArmorCategories.Any(i => activeCategories.Contains(i)))
            {
                __result = false;
            }

            return false;
        }

        #endregion Methods
    }
}