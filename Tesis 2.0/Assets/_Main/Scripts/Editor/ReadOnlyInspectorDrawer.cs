using _Main.Scripts.Attributes;
using UnityEditor;
using UnityEngine;

namespace _Main.Scripts.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyInspectorAttribute))]
    public class ReadOnlyInspectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect p_position, SerializedProperty p_property, GUIContent p_label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(p_position, p_property, p_label);
            GUI.enabled = true;
        }
    }
}
