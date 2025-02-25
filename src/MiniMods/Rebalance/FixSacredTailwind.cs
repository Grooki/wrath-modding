using Grooki.MiniMods.Settings;
using Grooki.MiniMods.Shared;
using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Utility;
using System;
using System.Linq;

namespace Grooki.MiniMods.Rebalance
{
    /// <summary>
    /// Auto metamagic class restrictions now apply to spells that are actually
    /// in the character's spellbook for that class, not just on the class's
    /// spell list in general.
    /// </summary>
    [HarmonyPatch(typeof(AutoMetamagic), nameof(AutoMetamagic.ShouldApplyTo))]
    internal class FixSacredTailwind
    {
        #region Properties

        [Setting("Sacred Tailwind Applies To Added Spells")]
        [SettingDescription("By default, Sacred Tailwind only applies to spells that are on the base divine spell " +
            "lists. If this is enabled, it applies to any addtional spells added to the list as well (domain spells, " +
            "oracle mystery spells, mythic spells, etc.).")]
        [SettingCategory(SettingCategory.Tailwinds, 0)]
        public static bool Enabled { get; set; }

        #endregion Properties

        #region Methods

        public static void Postfix(AutoMetamagic c, [CanBeNull] AbilityData data, ref bool __result, BlueprintCharacterClassReference[] __state)
        {
            try
            {
                if (Enabled)
                {
                    //Restore the cleared array
                    c.m_IncludeClasses = __state;

                    if (!__result) return; //Failed even without the class check, keep failed state.

                    //Otherwise, we need to addtionally ensure that the class is valid
                    var abilityClass = data?.SpellbookBlueprint?.CharacterClass;
                    if (c.m_IncludeClasses.Any() && abilityClass != null)
                    {
                        __result = c.m_IncludeClasses.Any(classRef => classRef.Is(abilityClass));
                    }
                }
            }
            catch (Exception e)
            {
                Log.Instance.LogException(e);
            }
        }

        public static bool Prefix(ref AutoMetamagic c, out BlueprintCharacterClassReference[] __state)
        {
            if (Enabled)
            {
                //Save include classes to check in the postfix
                __state = c?.m_IncludeClasses ?? Array.Empty<BlueprintCharacterClassReference>();

                //Clear the array so it's not checked by the base method
                c.m_IncludeClasses = Array.Empty<BlueprintCharacterClassReference>();
            }
            else
            {
                __state = Array.Empty<BlueprintCharacterClassReference>();
            }

            return true; //Don't skip method
        }

        #endregion Methods
    }
}