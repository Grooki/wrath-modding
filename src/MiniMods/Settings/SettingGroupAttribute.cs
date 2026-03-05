using System;

namespace Grooki.MiniMods.Settings
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    internal class SettingGroupAttribute : Attribute
    {
        #region Constructors

        public SettingGroupAttribute(string name)
        {
            Name = name;
        }

        #endregion Constructors

        #region Properties

        public string Name { get; }

        #endregion Properties
    }
}