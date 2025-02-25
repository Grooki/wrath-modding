using ModMenu.Settings;
using System;
using System.Reflection;

namespace Grooki.MiniMods.Settings
{
    internal class ReflectionSliderSetting : ReflectionSetting<int>
    {
        #region Fields

        private readonly RangeAttribute _rangeAttribute;

        #endregion Fields

        #region Constructors

        public ReflectionSliderSetting(PropertyInfo property, SettingAttribute attribute, RangeAttribute rangeAttribute) : base(property, attribute)
        {
            _rangeAttribute = rangeAttribute;
        }

        #endregion Constructors

        #region Methods

        public override void AddToBuilder(SettingsBuilder settingBuilder)
        {
            settingBuilder.AddSliderInt(SliderInt.New(Key, GetValue(), GetLocalizedName(), Convert.ToInt32(_rangeAttribute.Min), Convert.ToInt32(_rangeAttribute.Max))
                .WithOptionalLongDescription(GetLocalizedDescription())
                .OnValueChanged(SetValue)
                .ShowVisualConnection());
        }

        #endregion Methods
    }
}