using System.Collections.Generic;
using UnityEngine;

namespace Valkirie.Core.Objectives
{
    public enum CompletionRule { All, Any }

    [CreateAssetMenu(menuName = "Valkirie/Objectives/Objective Definition")]
    public class ObjectiveDefinition : ScriptableObject
    {
        public string id;
        [TextArea] public string description;
        public List<ObjectiveCondition> conditions = new();
        public CompletionRule completionRule = CompletionRule.All;
        public List<ObjectiveEffect> onCompleteEffects = new();
        public List<ObjectiveEffect> onFailEffects = new();
        public bool isOptional;
        public float timeLimit; // 0 = none
    }
}
