using System;

namespace Grooki.MiniMods.Settings
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class SettingAttribute : Attribute
    {
        #region Constructors

        public SettingAttribute(string name)
        {
            Name = name;
        }

        #endregion Constructors

        #region Properties

        public string Name { get; }

        #endregion Properties
    }
}