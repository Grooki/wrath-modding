using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using System.Collections.Generic;
using System.Linq;

namespace Grooki.MiniMods.Shared
{
    internal static class ComponentExtension
    {
        #region Methods

        private static IEnumerable<T> FindActions<T>(GameAction action)
        {
            if (action is Conditional conditional)
            {
                foreach (var i in conditional.IfTrue.Actions.Concat(conditional.IfFalse.Actions).SelectMany(FindActions<T>))
                {
                    yield return i;
                }
            }
            else if (action is T typedAction)
            {
                yield return typedAction;
            }
        }

        public static IEnumerable<T> GetActions<T>(this AbilityEffectRunAction action)
        {
            return action.Actions.Actions.SelectMany(FindActions<T>);
        }

        #endregion Methods
    }
}