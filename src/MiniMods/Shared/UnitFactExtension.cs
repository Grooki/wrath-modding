using Kingmaker.Blueprints.Facts;

namespace Grooki.MiniMods.Shared
{
    internal static class UnitFactExtension
    {
        #region Methods

        public static void SetDescription(this BlueprintUnitFact feature, string description)
        {
            feature.m_Description = Localization.CreateString($"{feature.Name}.Description", description);
        }

        public static void SetName(this BlueprintUnitFact feature, string name)
        {
            feature.m_DisplayName = Localization.CreateString($"{feature.Name}.Name", name);
        }

        #endregion Methods
    }
}