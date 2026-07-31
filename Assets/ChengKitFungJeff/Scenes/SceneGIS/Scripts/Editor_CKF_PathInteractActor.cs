#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(CKF_PathInteractActor))]
public class Editor_CKF_PathInteractActor : Editor
{
    SerializedProperty
        distance,
        interactPaths,
        targetController;

    CKF_PathInteractActor self;

    private void OnEnable()
    {
        self = (CKF_PathInteractActor)target;
        distance = serializedObject.FindProperty("distance");
        interactPaths = serializedObject.FindProperty("interactPaths");
        targetController = serializedObject.FindProperty("targetController");
    }
    override public void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (targetController.boxedValue is CKF_PathController pc)
        {
            if (GUILayout.Button("Set interactPaths"))
            {
                interactPaths.ClearArray();

                foreach (var p in FindObjectsByType<CKF_Path>(FindObjectsSortMode.None))
                {
                    if(p.controller != pc) continue;
                    Fn.SphereIntersectLine(p.nodeA.position,p.nodeB.position, self.transform.position, distance.floatValue, out float lerpA, out float lerpB);
                    if ((0 <= lerpA && lerpA <= 1) || (0 <= lerpB && lerpB <= 1))
                    {
                        interactPaths.InsertArrayElementAtIndex(interactPaths.arraySize);
                        var newEle = interactPaths.GetArrayElementAtIndex(interactPaths.arraySize - 1);
                        newEle.boxedValue = p;
                    }
                }
            }
        }
        else
        {
            EditorGUI.HelpBox(GUILayoutUtility.GetRect(18, 18, "TextField"), "targetController(CKF_PathController) not assigned ", MessageType.Info);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif