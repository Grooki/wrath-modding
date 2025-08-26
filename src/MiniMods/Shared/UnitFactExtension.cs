using Kingmaker.Blueprints.Facts;

namespace Grooki.MiniMods.Shared
{
    internal static class UnitFactExtension
    {
        #region Methods

        public static void SetDescription(this BlueprintUnitFact feature, string description)
        {
            feature.m_Description = Localization.CreateString($"minimods.{feature.Name.ToLowerInvariant()}.description", description);
        }

        public static void SetName(this BlueprintUnitFact feature, string name)
        {
            feature.m_DisplayName = Localization.CreateString($"minimods.{feature.Name.ToLowerInvariant()}.name", name);
        }

        #endregion Methods
    }
}