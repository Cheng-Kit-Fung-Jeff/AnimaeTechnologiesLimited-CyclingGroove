#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(GetSelfField))]
public class GetSelfPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.PropertyField(position, property, label);
        EditorGUI.EndDisabledGroup();

        if (property.propertyType == SerializedPropertyType.ObjectReference)
        {
            string propertyType = property.type[6..^1];            
            Component check = ((MonoBehaviour)property.serializedObject.targetObject).GetComponent(propertyType);
            if ((Component)property.boxedValue != check)
                property.boxedValue = check;
        }
        
    }
}
#endif