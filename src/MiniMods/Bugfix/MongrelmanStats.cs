using Grooki.MiniMods.Settings;
using Grooki.MiniMods.Shared;
using Kingmaker.Blueprints.Classes;
using System;

namespace LR.WrathMod.Changes.Bugfix
{
    /// <summary>
    /// Removes the incorrect floating race stat for the mongrelman race.
    /// </summary>
    internal class MongrelmanStats
    {
        #region Fields

        private static bool? _backupValue = null;
        private static bool _enabled;

        #endregion Fields

        #region Properties

        [Setting("Mongrel Racial Stats")]
        [SettingDescription("Removes the incorrect")]
        [SettingCategory(SettingCategory.Bugfixes, 0)]
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
            try
            {
                var mongerlmanRace = BlueprintHelper.GetBlueprint<BlueprintRace>("daca06a3f3355664bba1e87e67f3b5b3");
                if (Enabled)
                {
                    if (!_backupValue.HasValue)
                    {
                        _backupValue = mongerlmanRace.SelectableRaceStat = false;
                    }
                    mongerlmanRace.SelectableRaceStat = false;
                }
                else if (_backupValue.HasValue)
                {
                    mongerlmanRace.SelectableRaceStat = _backupValue.Value;
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