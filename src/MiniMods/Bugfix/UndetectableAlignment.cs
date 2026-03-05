using Grooki.MiniMods.Settings;
using HarmonyLib;
using Kingmaker.UnitLogic.FactLogic;
using System.Linq;

namespace Grooki.MiniMods.Bugfix
{
    [HarmonyPatch(typeof(AddUndetectableAlignment.Runtime), nameof(AddUndetectableAlignment.Runtime.OnDeactivate))]
    [SettingCategory(SettingCategory.Bugfixes)]
    internal static class UndetectableAlignment
    {
        #region Properties

        [Setting("Undetectable Alignment")]
        [SettingDescription("Fixes the issue causing characters to get stuck with undetectable alignment from items.")]
        public static bool Enabled { get; set; } = false;

        #endregion Properties

        #region Methods

        private static void Postfix(AddUndetectableAlignment.Runtime __instance)
        {
            if (!Enabled) return;

            var expected = __instance.Owner.Facts.List.Count(fact => fact.Active && fact.BlueprintComponents.OfType<AddUndetectableAlignment>().Any());
            while (__instance.Owner.Alignment.Undetectable.Count > expected)
            {
                __instance.Owner.Alignment.Undetectable.Release();
            }
        }

        #endregion Methods
    }
}