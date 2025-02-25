using Grooki.MiniMods.Settings;
using HarmonyLib;
using Kingmaker.Armies;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Globalmap.State;

namespace Grooki.MiniMods.Rebalance
{
    [HarmonyPatch(typeof(TacticalCombatResultsPrediction), nameof(TacticalCombatResultsPrediction.GetAttackerLossesPercent))]
    public static class AutoWinTacticalCombat
    {
        #region Properties

        [Setting("Always Win Crusade Combat")]
        [SettingDescription("You will always win the crusade mode tactical combats, regardless of army strength.")]
        public static bool Enabled { get; set; } = false;

        #endregion Properties

        #region Methods

        private static void Postfix(ref float __result, GlobalMapArmyState attacker)
        {
            if (Enabled)
            {
                __result = attacker.Data.Faction == ArmyFaction.Crusaders ? 0 : 100;
            }
        }

        #endregion Methods
    }
}