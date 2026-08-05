#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow;
[CustomEditor(typeof(CKF_PolyPath))]
public class Editor_CKF_PolyPath : Editor
{
    SerializedProperty
        controller,
        nodeA,nodes,nodeB,enterA,enterB,enteredA,enteredB,pathMultA,pathMultB,
        width,pathInnerColor,pathOuterColor,
        mode, prefabAnchor,midArc, minAngle;

    private void OnEnable()
    {
        controller = serializedObject.FindProperty("controller");
        nodeA = serializedObject.FindProperty("nodeA");
        nodes = serializedObject.FindProperty("nodes");
        nodeB = serializedObject.FindProperty("nodeB");
        enterA = serializedObject.FindProperty("enterA");
        enterB = serializedObject.FindProperty("enterB");
        enteredA = serializedObject.FindProperty("enteredA");
        enteredB = serializedObject.FindProperty("enteredB");
        width = serializedObject.FindProperty("width");
        pathInnerColor = serializedObject.FindProperty("pathInnerColor");
        pathOuterColor = serializedObject.FindProperty("pathOuterColor");
        mode = serializedObject.FindProperty("mode");
        prefabAnchor = serializedObject.FindProperty("prefabAnchor");
        midArc = serializedObject.FindProperty("midArc");
        minAngle = serializedObject.FindProperty("minAngle");
    }
    override public void OnInspectorGUI()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script",MonoScript.FromMonoBehaviour((MonoBehaviour)target), typeof(MonoScript), false);
        EditorGUI.EndDisabledGroup();
        controller.boxedValue = EditorGUILayout.ObjectField("Controller", (Object)controller.boxedValue, typeof(CKF_PathController), true);
        nodeA.boxedValue = EditorGUILayout.ObjectField("Node A", (Object)nodeA.boxedValue, typeof(Transform), true);
        EditorGUILayout.PropertyField(nodes, true);
        nodeB.boxedValue = EditorGUILayout.ObjectField("Node B", (Object)nodeB.boxedValue, typeof(Transform), true);
        enterA.boolValue = EditorGUILayout.Toggle("Enter A", enterA.boolValue);
        enterB.boolValue = EditorGUILayout.Toggle("Enter B", enterB.boolValue);
        enteredA.boolValue = EditorGUILayout.Toggle("Entered A", enteredA.boolValue);
        enteredB.boolValue = EditorGUILayout.Toggle("Entered B", enteredB.boolValue);
        width.floatValue = EditorGUILayout.FloatField("Width", width.floatValue);
        EditorGUILayout.PropertyField(pathInnerColor, true);
        EditorGUILayout.PropertyField(pathOuterColor, true);
        EditorGUILayout.LabelField(" ");
        EditorGUILayout.LabelField("Editor Interface", new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter});
        EditorGUILayout.LabelField(" ");
        EditorGUILayout.PropertyField(mode, true);
        if (mode.enumNames[mode.enumValueIndex] == "Arc")
        {
            EditorGUILayout.PropertyField(prefabAnchor, true);
            EditorGUILayout.PropertyField(midArc, true);
            minAngle.floatValue = EditorGUILayout.FloatField("Min Angle", minAngle.floatValue);
            EditorGUILayout.LabelField(" ");

            Transform trA = (Transform)nodeA.boxedValue,
                trB = (Transform)nodeB.boxedValue,
                trM = (Transform)midArc.boxedValue;
            GameObject gbPrefabAnchor = (GameObject)prefabAnchor.boxedValue;
            bool flagPass = true;
            if (trA == null)
            {
                EditorGUI.HelpBox(GUILayoutUtility.GetRect(20, 20, "TextField"),"nodeA not assigned.", MessageType.Error);
                flagPass = false;
            }
            if (trB == null)
            {
                EditorGUI.HelpBox(GUILayoutUtility.GetRect(20, 20, "TextField"), "nodeB not assigned.", MessageType.Error);
                flagPass = false;
            }
            if (gbPrefabAnchor == null)
            {
                EditorGUI.HelpBox(GUILayoutUtility.GetRect(20, 20, "TextField"), "Prefab Anchor not assigned.", MessageType.Error);
                flagPass = false;
            }
            if (trM == null)
            {
                EditorGUI.HelpBox(GUILayoutUtility.GetRect(20, 20, "TextField"), "Mid Arc not assigned.", MessageType.Error);
                flagPass = false;
            }
            if (minAngle.floatValue <= 0)
            {
                EditorGUI.HelpBox(GUILayoutUtility.GetRect(20, 20, "TextField"), "minAngle must be greater than 0.", MessageType.Error);
                flagPass = false;
            }
            EditorGUI.BeginDisabledGroup(!flagPass);
            flagPass = GUILayout.Button("Generate");
            EditorGUI.EndDisabledGroup();
            if (flagPass)
            {
                float det =
                    2 * (trA.position.x * (trB.position.y - trM.position.y)+
                    trB.position.x * (trM.position.y - trA.position.y) +
                    trM.position.x * (trA.position.y - trB.position.y));
                float sqrA = trA.position.x * trA.position.x + trA.position.y * trA.position.y;
                float sqrB = trA.position.x * trB.position.x + trB.position.y * trB.position.y;
                float sqrM = trM.position.x * trM.position.x + trM.position.y * trM.position.y;
                float x =
                    (sqrA * (trB.position.y - trM.position.y) +
                    sqrB * (trM.position.y - trA.position.y) +
                    sqrM * (trA.position.y - trB.position.y)) / det;
                    ;
                float y =
                    sqrA * (trM.position.y - trB.position.y) +
                    sqrB * (trA.position.y - trM.position.y) +
                    sqrM * (trB.position.y - trA.position.y)
                    ;

                ;
                Debug.Log("generate");
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif