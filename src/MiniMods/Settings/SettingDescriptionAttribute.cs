using System;

namespace Grooki.MiniMods.Settings
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class SettingDescriptionAttribute : Attribute
    {
        #region Constructors

        public SettingDescriptionAttribute(string description)
        {
            Description = description;
        }

        #endregion Constructors

        #region Properties

        public string Description { get; }

        #endregion Properties
    }
}