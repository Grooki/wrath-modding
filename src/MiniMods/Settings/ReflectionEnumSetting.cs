using Kingmaker.UI.SettingsUI;
using ModMenu.Settings;
using System;
using System.Reflection;
using UnityEngine;

namespace Grooki.MiniMods.Settings
{
    internal class ReflectionEnumSetting<T> : ReflectionSetting<T> where T : Enum
    {
        #region Fields

        private readonly SettingDropDownTypeAttribute _dropDownAttribute;

        #endregion Fields

        #region Constructors

        public ReflectionEnumSetting(PropertyInfo property, SettingAttribute attribute, SettingDropDownTypeAttribute dropDownAttribute) : base(property, attribute)
        {
            _dropDownAttribute = dropDownAttribute;
        }

        #endregion Constructors

        #region Methods

        public override void AddToBuilder(SettingsBuilder settingBuilder)
        {
            var dropdownUi = ScriptableObject.CreateInstance(_dropDownAttribute.Type) as UISettingsEntityDropdownEnum<T>;

            settingBuilder.AddDropdown(Dropdown<T>.New(Key, GetValue(), GetLocalizedName(), dropdownUi)
                .WithOptionalLongDescription(GetLocalizedDescription())
                .OnValueChanged(SetValue)
                .ShowVisualConnection());
        }

        #endregion Methods
    }
}