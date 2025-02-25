using Kingmaker.Localization;
using Kingmaker.UI.SettingsUI;
using ModMenu.Settings;

namespace Grooki.MiniMods.Settings
{
    internal static class SettingBuilderExtension
    {
        #region Methods

        public static TBuilder WithOptionalLongDescription<TUIEntity, TBuilder>(this BaseSetting<TUIEntity, TBuilder> setting, LocalizedString description)
                                    where TUIEntity : UISettingsEntityBase where TBuilder : BaseSetting<TUIEntity, TBuilder>
        {
            if (description != null)
            {
                setting.WithLongDescription(description);
            }

            return setting as TBuilder;
        }

        #endregion Methods
    }
}