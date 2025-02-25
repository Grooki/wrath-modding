using ModMenu.Settings;
using System.Reflection;

namespace Grooki.MiniMods.Settings
{
    internal class ReflectionToggleSetting : ReflectionSetting<bool>
    {
        #region Constructors

        public ReflectionToggleSetting(PropertyInfo property, SettingAttribute attribute) : base(property, attribute)
        {
        }

        #endregion Constructors

        #region Methods

        public override void AddToBuilder(SettingsBuilder settingBuilder)
        {
            settingBuilder.AddToggle(Toggle.New(Key, GetValue(), GetLocalizedName())
                .WithOptionalLongDescription(GetLocalizedDescription())
                .OnValueChanged(SetValue)
                .ShowVisualConnection());
        }

        #endregion Methods
    }
}