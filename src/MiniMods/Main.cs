using Grooki.MiniMods.Shared;
using HarmonyLib;
using UnityModManagerNet;

namespace Grooki.MiniMods
{
    public static class Main
    {
        #region Properties

        public static UnityModManager.ModEntry ModEntry { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Mod entry point
        /// </summary>
        public static void Load(UnityModManager.ModEntry modEntry)
        {
            ModEntry = modEntry;
            Log.Instance = modEntry.Logger;

            //Harmony patches
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll();
        }

        #endregion Methods
    }
}