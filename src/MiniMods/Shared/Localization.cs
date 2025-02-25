using Kingmaker.Localization;
using System.Collections.Generic;

namespace Grooki.MiniMods.Shared
{
    internal static class Localization
    {
        #region Fields

        private static Dictionary<string, LocalizedString> _localizedStrings = new Dictionary<string, LocalizedString>();

        #endregion Fields

        #region Methods

        internal static LocalizedString CreateString(string key, string value)
        {
            if (value is null) return null;

            //Get existing value if any
            if (_localizedStrings.TryGetValue(value, out LocalizedString localized)) return localized;

            //Add new value
            LocalizationManager.CurrentPack.PutString(key, value);
            return _localizedStrings[value] = new LocalizedString { Key = key };
        }

        #endregion Methods
    }
}