using ModMenu.Settings;

namespace Grooki.MiniMods.Settings
{
    internal interface ISetting
    {
        #region Properties

        SettingCategory Category { get; }

        string Name { get; }

        int Order { get; }

        #endregion Properties

        #region Methods

        void AddToBuilder(SettingsBuilder settingBuilder);

        void LoadValue();

        #endregion Methods
    }
}