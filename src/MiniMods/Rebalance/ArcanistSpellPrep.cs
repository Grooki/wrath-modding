using Grooki.MiniMods.Settings;
using Grooki.MiniMods.Shared;
using HarmonyLib;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Linq;

namespace Grooki.MiniMods.Rebalance
{
    [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.GetSpellSlotsCount))]
    [SettingCategory(SettingCategory.Rebalance)]
    [SettingGroup("Arcanist")]
    public static class ArcanistSpellPrep
    {
        #region Fields

        private static float _bonusSpellRatio = 0;
        private static int _mythicSpellBonus = 0;

        #endregion Fields

        #region Properties

        [Setting("Additional Spells Prepared for Bonus Spells Per Day")]
        [SettingDescription("The number of additional spells prepared for arcanists for each bonus spell slot they get from items and features. " +
            "\nFor example, if set to 0.5, Abundant Casting will allow an arcanist to also prepare two extra spells at each level. If set to 1, " +
            "they could prepare would get four addtional spells. The final value is rounded.")]
        [Range(Min = 0, Max = 2)]
        public static float BonusSpellRatio
        {
            get => _bonusSpellRatio;
            set
            {
                _bonusSpellRatio = value;
            }
        }

        [Setting("Increase Spells Prepared When Merged")]
        [SettingDescription("The number of additional spells prepared for merged arcanists at each level where they have mythic spells.")]
        [Range(Min = 0, Max = 5)]
        public static int IncreaseInterval
        {
            get => _mythicSpellBonus;
            set
            {
                _mythicSpellBonus = value;
            }
        }

        #endregion Properties

        #region Methods

        public static void Postfix(Spellbook __instance, ref int __result, int spellLevel)
        {
            try
            {
                if (__instance is null) return;

                if (__instance.Blueprint.IsArcanist)
                {
                    if (__instance.GetKnownSpells(spellLevel).Any(i => i.IsFromMythicSpellList))
                    {
                        __result += _mythicSpellBonus;
                    }

                    var extraSpells = __instance.Owner.Get<UnitPartExtraSpellsPerDay>();
                    if(extraSpells != null)
                    {
                        __result += Convert.ToInt32(Math.Round(extraSpells.BonusSpells[spellLevel] * _bonusSpellRatio, 0));
                    }
                }
            }
            catch (Exception e)
            {
                Log.Instance.LogException(e);
            }
        }

        #endregion Methods
    }
}