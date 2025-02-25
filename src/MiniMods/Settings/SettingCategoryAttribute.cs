using System;

namespace Grooki.MiniMods.Settings
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class SettingCategoryAttribute : Attribute
    {
        #region Constructors

        public SettingCategoryAttribute(SettingCategory category, int order)
        {
            Category = category;
            Order = order;
        }

        #endregion Constructors

        #region Properties

        public SettingCategory Category { get; }
        public int Order { get; }

        #endregion Properties
    }
}