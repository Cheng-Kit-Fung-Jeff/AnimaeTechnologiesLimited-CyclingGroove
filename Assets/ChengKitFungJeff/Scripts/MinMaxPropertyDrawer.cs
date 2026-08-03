#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinMaxField))]
public class MinMaxPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        MinMaxField param = (MinMaxField)attribute;
        float curMin = property.vector2Value.x;
        float curMax = property.vector2Value.y;
        EditorGUI.MinMaxSlider(position, label, ref curMin, ref curMax, param.min, param.max);
        property.vector2Value = new(curMin, curMax);
        Vector2 nextVector2 = EditorGUILayout.Vector2Field(" ", property.vector2Value);
        nextVector2 = new(Mathf.Max(Mathf.Min(nextVector2.x, property.vector2Value.y),param.min),Mathf.Min(Mathf.Max(property.vector2Value.x, nextVector2.y),param.max));
        property.vector2Value = nextVector2;
    }
}
#endif
