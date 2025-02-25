using Grooki.MiniMods.Settings;
using Grooki.MiniMods.Shared;
using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Class.LevelUp;
using System;
using System.Linq;

namespace Grooki.MiniMods.QualityOfLife
{
    /// <summary>
    /// Automatically allocates avalaible skill points to the skills with the highest points already allocated.
    /// </summary>
    [HarmonyPatch(typeof(LevelUpController), nameof(LevelUpController.ApplySkillPoints))]
    public static class AutoAllocateSkillPoints
    {
        #region Properties

        [Setting("Auto Allocate Skill Points")]
        [SettingDescription("On level up, automatically allocates avalaible skill points to the " +
            "skills with the highest points already allocated. You're still free to change them.")]
        public static bool Enabled { get; set; } = false;

        #endregion Properties

        #region Methods

        public static void Postfix(LevelUpController __instance)
        {
            if (!Enabled) return;

            try
            {
                //Order skills by points allocated, skip those with 0 points.
                var skillPoints = StatTypeHelper.Skills.Select(i => Tuple.Create(i, __instance.Unit.Stats.GetStat(i).BaseValue));
                var skillsToAllocate = skillPoints.Where(i => i.Item2 != 0).OrderByDescending(i => i.Item2).Select(i => i.Item1).ToList();
                __instance.SpendLevelPlanSkillPoints(skillsToAllocate);
            }
            catch (Exception ex)
            {
                Log.Instance.Log("Failed to allocate skill points");
                Log.Instance.LogException(ex);
            }
        }

        #endregion Methods
    }
}