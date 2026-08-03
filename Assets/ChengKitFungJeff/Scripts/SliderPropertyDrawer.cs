#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SliderField))]
public class SliderPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SliderField param = attribute as SliderField;
        if (param.min is float min && param.max is float max)
            EditorGUI.Slider(position, property, min, max, label);
        else if (param.min is Vector2 min2 && param.max is Vector2 max2) {
            EditorGUI.LabelField(position, label);
            property.vector2Value = new(
                EditorGUILayout.Slider("X", property.vector2Value.x, min2.x, max2.x),
                EditorGUILayout.Slider("Y", property.vector2Value.y, min2.y, max2.y)
            );
        }
        else if (param.min is Vector3 min3 && param.max is Vector3 max3)
        {
            EditorGUI.LabelField(position, label);
            property.vector3Value = new(
                EditorGUILayout.Slider("X", property.vector3Value.x, min3.x, max3.x),
                EditorGUILayout.Slider("Y", property.vector3Value.y, min3.y, max3.y),
                EditorGUILayout.Slider("Z", property.vector3Value.z, min3.z, max3.z)
            );
        }
        else if (param.min is Vector4 min4 && param.max is Vector4 max4)
        {
            EditorGUI.LabelField(position, label);
            property.vector4Value = new(
                EditorGUILayout.Slider("X", property.vector3Value.x, min4.x, max4.x),
                EditorGUILayout.Slider("Y", property.vector3Value.y, min4.y, max4.y),
                EditorGUILayout.Slider("Z", property.vector3Value.z, min4.z, max4.z),
                EditorGUILayout.Slider("W", property.vector4Value.w, min4.w, max4.w)
            );
        }
    }
}
#endif