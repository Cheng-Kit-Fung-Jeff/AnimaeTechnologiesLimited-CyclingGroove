#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(CKF_ExecuteUnityEvent))]
public class Editor_CKF_ExecuteUnityEvent : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        if(GUILayout.Button("Execute"))((CKF_ExecuteUnityEvent)target).execution.Invoke();
        EditorGUI.EndDisabledGroup();
    }
}
#endif