using System;

namespace Grooki.MiniMods.Settings
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class RangeAttribute : Attribute
    {
        #region Fields

        public double Max;
        public double Min;

        #endregion Fields
    }
}