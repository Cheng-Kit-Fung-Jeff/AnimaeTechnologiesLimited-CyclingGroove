#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(CKF_RectPosToAnchor))]
public class Editor_CKF_RectPosToAnchor : Editor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (((CKF_RectPosToAnchor)target).transform is RectTransform selfRT)
        {
            if (selfRT.parent is RectTransform parentRT)
            {
                Debug.Log(parentRT.rect.ToString()+ parentRT.anchoredPosition.ToString()+ selfRT.rect.ToString()+selfRT.anchoredPosition.ToString());
                /*if (GUILayout.Button("Set Anchor"))
                {

                }*/
            }
            else
            {
                EditorGUI.HelpBox(GUILayoutUtility.GetRect(24, 24, "TextField"), "This object has no RectTransform Parent", MessageType.Error);
            }
        }
        else
        {
            EditorGUI.HelpBox(GUILayoutUtility.GetRect(24, 24, "TextField"), "This object does not have a RectTransform", MessageType.Error);
        }
    }
}
#endif