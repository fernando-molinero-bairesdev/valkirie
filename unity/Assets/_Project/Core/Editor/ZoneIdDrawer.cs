#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Valkirie.Core.Incidents;

namespace Valkirie.Core.Editor
{
    [CustomPropertyDrawer(typeof(ZoneIdAttribute))]
    public class ZoneIdDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var registry = FindRegistry();
            if (registry == null || registry.knownZoneIds.Count == 0)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            var options = registry.knownZoneIds.ToArray();
            var index = System.Array.IndexOf(options, property.stringValue);
            if (index < 0) index = 0;

            var newIndex = EditorGUI.Popup(position, label.text, index, options);
            property.stringValue = options[newIndex];
        }

        private static ZoneIdRegistry FindRegistry()
        {
            var guids = AssetDatabase.FindAssets("t:ZoneIdRegistry");
            return guids.Length == 0
                ? null
                : AssetDatabase.LoadAssetAtPath<ZoneIdRegistry>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }
    }
}
#endif
