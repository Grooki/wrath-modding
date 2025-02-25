using Kingmaker.Blueprints;

namespace Grooki.MiniMods.Shared
{
    internal static class BlueprintHelper
    {
        #region Methods

        public static T GetBlueprint<T>(string id) where T : SimpleBlueprint
        {
            var assetId = new BlueprintGuid(System.Guid.Parse(id));
            return GetBlueprint<T>(assetId);
        }

        public static T GetBlueprint<T>(BlueprintGuid id) where T : SimpleBlueprint
        {
            SimpleBlueprint asset = ResourcesLibrary.TryGetBlueprint(id);
            T value = asset as T;
            return value;
        }

        #endregion Methods
    }
}