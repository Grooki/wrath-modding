using Grooki.MiniMods.Settings;
using HarmonyLib;
using Kingmaker.Armies;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Globalmap.State;
using Kingmaker.UI;

namespace Grooki.MiniMods.Rebalance
{
    [HarmonyPatch]
    public static class AutoWinTacticalCombat
    {
        #region Properties

        [Setting("Always Win Crusade Combat")]
        [SettingDescription("You will always win the crusade mode tactical combats, regardless of army strength.")]
        public static bool Enabled { get; set; }

        #endregion Properties

        #region Methods

        [HarmonyPatch(typeof(PlayerUISettings), nameof(PlayerUISettings.AutoTacticalCombat), MethodType.Getter)]
        [HarmonyPostfix]
        private static void OverrideAutoCombat(ref bool __result)
        {
            if (Enabled)
            {
                __result = true;
            }
        }

        [HarmonyPatch(typeof(TacticalCombatResultsPrediction), nameof(TacticalCombatResultsPrediction.GetAttackerLossesPercent))]
        [HarmonyPostfix]
        private static void OverrideCalculation(ref float __result, GlobalMapArmyState attacker)
        {
            if (Enabled)
            {
                __result = attacker.Data.Faction == ArmyFaction.Crusaders ? 0 : 100;
            }
        }

        #endregion Methods
    }
}