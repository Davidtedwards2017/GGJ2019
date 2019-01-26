using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(SoundEffectData))]
public class SoundEffectDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var clipRect = new Rect(position.x, position.y, position.width - 30, position.height);
        var volumnRect = new Rect(position.x + position.width - 30, position.y, 25, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(clipRect, property.FindPropertyRelative("Clip"), GUIContent.none);
        EditorGUI.PropertyField(volumnRect, property.FindPropertyRelative("Volume"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
