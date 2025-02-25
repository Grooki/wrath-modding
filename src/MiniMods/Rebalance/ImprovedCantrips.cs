using Grooki.MiniMods.Settings;
using Grooki.MiniMods.Shared;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using Kingmaker.UI.SettingsUI;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Grooki.MiniMods.Rebalance
{
    public static class ImprovedCantrips
    {
        #region Classes

        private class DiceTypeDropdown : UISettingsEntityDropdownEnum<DiceType>
        { }

        #endregion Classes

        #region Fields

        private static readonly string[] CantripIds = new string[]
        {
            "0c852a2405dd9f14a8bbcfaf245ff823", //Acid Splash
            "8a1992f59e06dd64ab9ba52337bf8cb5", //Divine Zap
            "564c2ac83c7844beb1921e69ab159ac6", //Ignition
            "16e23c7a8ae53cc42a93066d19766404", //Jolt
            "9af2ab69df6538f4793b2f9c3cc85603", //Ray of Frost
        };

        private static readonly Dictionary<BlueprintGuid, string> OldDescriptions = new Dictionary<BlueprintGuid, string>();
        private static DiceType _dieSize = DiceType.D6;
        private static int _increaseInterval = 4;
        private static int _maxDice = 20;
        private static bool _rescaleCantrips = false;

        #endregion Fields

        #region Properties

        [Setting("Die Size")]
        [SettingCategory(SettingCategory.ImprovedCantrips, 1)]
        [SettingDescription("Base die size for cantrips.")]
        [SettingDropDownType(typeof(DiceTypeDropdown))]
        public static DiceType DieSize
        {
            get => _dieSize;
            set
            {
                _dieSize = value;
                Apply();
            }
        }

        [Setting("Level Interval")]
        [SettingCategory(SettingCategory.ImprovedCantrips, 2)]
        [SettingDescription("Number of caster levels required to add an addtional damage die.")]
        [Range(Min = 1, Max = 20)]
        public static int IncreaseInterval
        {
            get => _increaseInterval;
            set
            {
                _increaseInterval = value;
                Apply();
            }
        }

        [Setting("Maximum Dice")]
        [SettingCategory(SettingCategory.ImprovedCantrips, 3)]
        [SettingDescription("The maximum number of damage dice.")]
        [Range(Min = 1, Max = 20)]
        public static int MaxDice
        {
            get => _maxDice;
        }

        [Setting("Enabled")]
        [SettingCategory(SettingCategory.ImprovedCantrips, 0)]
        [SettingDescription("Enables cantrip changes. Requires restart if disabled.")]
        public static bool RescaleCantrips
        {
            get => _rescaleCantrips;
            set
            {
                _rescaleCantrips = value;
                Apply();
            }
        }

        #endregion Properties

        #region Methods

        private static void Apply()
        {
            Log.Instance.Log($"Applying Cantrip Scaling {RescaleCantrips}");

            if (!RescaleCantrips) return;

            foreach (var id in CantripIds)
            {
                var blueprint = BlueprintHelper.GetBlueprint<BlueprintAbility>(id);
                if (blueprint is null) continue;
                if (!OldDescriptions.ContainsKey(blueprint.AssetGuid))
                {
                    OldDescriptions.Add(blueprint.AssetGuid, blueprint.Description);
                }

                ScaleCantrip(blueprint);
            }
        }

        private static void ScaleCantrip(BlueprintAbility cantrip)
        {
            //Update the damage
            DiceType oldDie = DiceType.Zero;
            foreach (var damageAction in cantrip.GetComponent<AbilityEffectRunAction>().GetActions<ContextActionDealDamage>())
            {
                oldDie = damageAction.Value.DiceType;
                damageAction.Value.DiceType = DieSize;
                damageAction.Value.DiceCountValue = new Kingmaker.UnitLogic.Mechanics.ContextValue()
                {
                    ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Rank,
                    ValueRank = Kingmaker.Enums.AbilityRankType.Default
                };
            }

            //Update the scaling values
            var rankComponent = cantrip.GetComponent<ContextRankConfig>();
            if (rankComponent is null)
            {
                rankComponent = new ContextRankConfig();
                cantrip.AddComponent(rankComponent);
            }

            rankComponent.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
            rankComponent.m_Progression = ContextRankProgression.StartPlusDivStep;
            rankComponent.m_StartLevel = 1;
            rankComponent.m_StepLevel = IncreaseInterval;
            rankComponent.m_Max = MaxDice;
            rankComponent.m_UseMax = true;

            //Fix description
            var oldVal = (int)oldDie;
            var newVal = (int)DieSize;

            var description = Regex.Replace(OldDescriptions[cantrip.AssetGuid], "1–3 \\((.*)\\)", "$1");
            description = description.Replace($"1d3", $"1d{newVal}");
            if (IncreaseInterval == 1)
            {
                description = Regex.Replace(description, $"(points of .*?)\\.", $"$1, plus an additional 1d{newVal} points of damage for every level beyond 1st.");
            }
            else
            {
                description = Regex.Replace(description, $"(points of .*?)\\.", $"$1, plus an additional 1d{newVal} points of damage for every {IncreaseInterval} levels beyond 1st.");
            }
            cantrip.SetDescription(description);
        }

        #endregion Methods
    }
}