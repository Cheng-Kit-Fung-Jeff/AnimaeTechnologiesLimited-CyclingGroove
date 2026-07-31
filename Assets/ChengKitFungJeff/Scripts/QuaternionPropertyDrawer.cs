#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(QuaternionField))]
public class QuaternionPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.PropertyField(position, property, label);
        property.quaternionValue = new(
            EditorGUILayout.Slider("X", property.quaternionValue.x, -1, 1),
            EditorGUILayout.Slider("Y", property.quaternionValue.y, -1, 1),
            EditorGUILayout.Slider("Z", property.quaternionValue.z, -1, 1),
            EditorGUILayout.Slider("W", property.quaternionValue.w, -1, 1)
        );
        if (GUILayout.Button("Negate"))
            property.quaternionValue = new(-property.quaternionValue.x, -property.quaternionValue.y, -property.quaternionValue.z, -property.quaternionValue.w);
    }
}
#endif