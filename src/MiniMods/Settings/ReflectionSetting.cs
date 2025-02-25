using Grooki.MiniMods.Shared;
using Kingmaker.Localization;
using ModMenu.Settings;
using System;
using System.Reflection;

namespace Grooki.MiniMods.Settings
{
    internal static class ReflectionSetting
    {
        #region Methods

        public static ISetting Create(PropertyInfo propertyInfo)
        {
            try
            {
                var settingAttribute = propertyInfo.GetCustomAttribute<SettingAttribute>();
                if (settingAttribute == null) return null;

                if (typeof(Enum).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    var dropDownAttribute = propertyInfo.GetCustomAttribute<SettingDropDownTypeAttribute>();
                    if (dropDownAttribute is null) return null;
                    var settingType = typeof(ReflectionEnumSetting<>).MakeGenericType(propertyInfo.PropertyType);
                    return Activator.CreateInstance(settingType, propertyInfo, settingAttribute, dropDownAttribute) as ISetting;
                }
                else if (propertyInfo.PropertyType == typeof(bool))
                {
                    return new ReflectionToggleSetting(propertyInfo, settingAttribute);
                }
                else if (propertyInfo.PropertyType == typeof(int))
                {
                    var rangeAttribute = propertyInfo.GetCustomAttribute<RangeAttribute>();
                    if (rangeAttribute is null) return null;
                    return new ReflectionSliderSetting(propertyInfo, settingAttribute, rangeAttribute);
                }
            }
            catch
            {
            }

            return null;
        }

        #endregion Methods
    }

    internal abstract class ReflectionSetting<T> : ISetting
    {
        #region Fields

        private readonly SettingAttribute _attribute;
        private readonly PropertyInfo _property;

        #endregion Fields

        #region Constructors

        public ReflectionSetting(PropertyInfo property, SettingAttribute attribute)
        {
            if (!typeof(T).IsAssignableFrom(property.PropertyType))
            {
                throw new ArgumentException();
            }

            _property = property;
            _attribute = attribute;

            var categoryAttribute = _property.GetCustomAttribute<SettingCategoryAttribute>();
            Category = categoryAttribute?.Category ?? SettingCategory.General;
            Order = categoryAttribute?.Order ?? 0;
        }

        #endregion Constructors

        #region Properties

        public SettingCategory Category { get; }
        public string Key => $"{_property.DeclaringType.FullName}.{_property.Name}".ToLower();
        public string Name => _attribute.Name;
        public int Order { get; }

        #endregion Properties

        #region Methods

        protected LocalizedString GetLocalizedDescription()
        {
            var attribute = _property.GetCustomAttribute<SettingDescriptionAttribute>();
            if (attribute is null) return null;
            return Localization.CreateString($"{Key}.description", attribute.Description);
        }

        protected LocalizedString GetLocalizedName()
        {
            return Localization.CreateString($"{Key}.name", _attribute.Name);
        }

        protected T GetValue()
        {
            return (T)_property.GetValue(null);
        }

        protected void SetValue(T newValue)
        {
            try
            {
                _property.SetValue(null, newValue); //We require all setting properties to be static
            }
            catch (Exception ex)
            {
                Log.Instance.LogException(ex);
            }
        }

        public abstract void AddToBuilder(SettingsBuilder settingBuilder);

        public void LoadValue()
        {
            SetValue(ModMenu.ModMenu.GetSettingValue<T>(Key));
        }

        #endregion Methods
    }
}