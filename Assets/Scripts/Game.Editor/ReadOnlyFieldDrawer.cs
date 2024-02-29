using UnityEditor;
using UnityEngine;

namespace Game.Editor {
    [CustomPropertyDrawer(typeof(ReadOnlyFieldAttribute))]
    public class ReadOnlyFieldDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            using (new EditorGUI.DisabledScope(true)) {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }
}
