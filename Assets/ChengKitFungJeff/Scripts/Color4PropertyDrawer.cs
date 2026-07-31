#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Color4Field))]
public class Color4PropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        if (property.propertyType == SerializedPropertyType.Color)
        {
            property.colorValue = EditorGUILayout.ColorField(label, property.colorValue);
            property.colorValue = EditorGUILayout.Vector4Field("Vector4", property.colorValue);
        }
        else if (property.propertyType == SerializedPropertyType.Vector4)
        {
            property.vector4Value = EditorGUILayout.Vector4Field(label, property.vector4Value);
            property.vector4Value = EditorGUILayout.ColorField("Color", property.vector4Value);
        }
    }
}
#endif