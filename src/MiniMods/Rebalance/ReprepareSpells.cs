using Grooki.MiniMods.Settings;
using HarmonyLib;
using Kingmaker;
using Kingmaker.UnitLogic;

namespace Grooki.MiniMods.Rebalance
{
    [HarmonyPatch(typeof(SpellSlot), nameof(SpellSlot.SpellShell), MethodType.Setter)]
    internal class ReprepareSpells
    {
        #region Properties

        [Setting("Reprepare Spells")]
        [SettingDescription("Allows prepared spellcasters to change which spells they have prepared while out of combat.")]
        public static bool Enabled { get; set; } = false;

        #endregion Properties

        #region Methods

        public static void Postfix(SpellSlot __instance, bool __state)
        {
            if (!Enabled) return;

            if (__state && !__instance.Available && !Game.Instance.Player.IsInCombat)
            {
                __instance.Available = true;
            }
        }

        public static bool Prefix(SpellSlot __instance, out bool __state)
        {
            __state = __instance.Available;
            return true;
        }

        #endregion Methods
    }
}