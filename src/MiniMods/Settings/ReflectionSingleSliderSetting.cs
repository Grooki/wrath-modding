using ModMenu.Settings;
using System;
using System.Reflection;

namespace Grooki.MiniMods.Settings
{
    internal class ReflectionSingleSliderSetting : ReflectionSetting<float>
    {
        #region Fields

        private readonly RangeAttribute _rangeAttribute;

        #endregion Fields

        #region Constructors

        public ReflectionSingleSliderSetting(PropertyInfo property, SettingAttribute attribute, RangeAttribute rangeAttribute) : base(property, attribute)
        {
            _rangeAttribute = rangeAttribute;
        }

        #endregion Constructors

        #region Methods

        public override void AddToBuilder(SettingsBuilder settingBuilder)
        {
            settingBuilder.AddSliderFloat(SliderFloat.New(Key, GetValue(), GetLocalizedName(), Convert.ToSingle(_rangeAttribute.Min), Convert.ToSingle(_rangeAttribute.Max))
                .WithOptionalLongDescription(GetLocalizedDescription())
                .OnValueChanged(SetValue)
                .ShowVisualConnection());
        }

        #endregion Methods
    }
}