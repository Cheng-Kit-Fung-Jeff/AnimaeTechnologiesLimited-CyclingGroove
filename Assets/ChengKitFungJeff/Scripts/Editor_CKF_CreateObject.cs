#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(CKF_CreateObject))]
public class Editor_CKF_CreateObject : Editor
{
    SerializedProperty
        data,
        savePath;
    private void OnEnable()
    {
        data = serializedObject.FindProperty("data");
        savePath = serializedObject.FindProperty("path");
    }
    override public void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (data.boxedValue == null)
        {
            EditorGUI.HelpBox(GUILayoutUtility.GetRect(18, 18, "TextField"), "No data", MessageType.Error);
        }
        else if (savePath.stringValue == null || savePath.stringValue == "")
        {
            EditorGUI.HelpBox(GUILayoutUtility.GetRect(18, 18, "TextField"), "No save path", MessageType.Error);
        }
        else
        {
            if (GUILayout.Button("Create"))
            {
                AssetDatabase.CreateAsset((UnityEngine.Object)data.boxedValue, savePath.stringValue);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif