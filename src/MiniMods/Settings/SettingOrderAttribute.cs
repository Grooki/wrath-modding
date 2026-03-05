using System;

namespace Grooki.MiniMods.Settings
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class SettingOrderAttribute : Attribute
    {
        #region Constructors

        public SettingOrderAttribute(int order)
        {
            Order = order;
        }

        #endregion Constructors

        #region Properties

        public int Order { get; }

        #endregion Properties
    }
}