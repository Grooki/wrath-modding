using System;

namespace Grooki.MiniMods.Settings
{
    internal class SettingDropDownTypeAttribute : Attribute
    {
        #region Constructors

        public SettingDropDownTypeAttribute(Type type)
        {
            Type = type;
        }

        #endregion Constructors

        #region Properties

        public Type Type { get; }

        #endregion Properties
    }
}