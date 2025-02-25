using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;

namespace Grooki.MiniMods.Shared
{
    internal static class ScriptableObjectExtension
    {
        #region Methods

        public static void AddComponent(this BlueprintScriptableObject obj, BlueprintComponent component)
        {
            var newArray = new BlueprintComponent[obj.ComponentsArray.Length + 1];
            Array.Copy(obj.ComponentsArray, 0, newArray, 0, obj.ComponentsArray.Length);
            newArray[obj.ComponentsArray.Length] = component;

            obj.SetComponents(newArray);
        }

        public static void SetComponents(this BlueprintScriptableObject obj, params BlueprintComponent[] components)
        {
            // Fix names of components. Generally this doesn't matter, but if they have serialization state,
            // then their name needs to be unique.
            var names = new HashSet<string>();
            foreach (var c in components)
            {
                if (string.IsNullOrEmpty(c.name))
                {
                    c.name = $"${c.GetType().Name}";
                }
                if (!names.Add(c.name))
                {
                    String name;
                    for (int i = 0; !names.Add(name = $"{c.name}${i}"); i++) ;
                    c.name = name;
                }
            }

            obj.ComponentsArray = components;
        }

        #endregion Methods
    }
}