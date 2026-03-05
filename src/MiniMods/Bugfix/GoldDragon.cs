using Grooki.MiniMods.Settings;
using Grooki.MiniMods.Shared;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace Grooki.MiniMods.Bugfix
{
    [SettingCategory(SettingCategory.Bugfixes)]
    internal static class GoldDragon
    {
        #region Fields

        private static bool _enabled;

        #endregion Fields

        #region Properties

        [Setting("Gold Dragon Blindsight")]
        [SettingDescription("If enabled, Gold Dragon Form will correctly give blindsense.")]
        public static bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                Apply();
            }
        }

        #endregion Properties

        #region Methods

        private static void Apply()
        {
            if (!Enabled) return;

            var dragonForm = BlueprintHelper.GetBlueprint<BlueprintBuff>("dbe1d6ac18ad4eafb4f6d24e48eb12dc");
            var blindsense = new Blindsense() { Blindsight = false, Range = new Kingmaker.Utility.Feet(60) };
            dragonForm.AddComponent(blindsense);
        }

        #endregion Methods
    }
}