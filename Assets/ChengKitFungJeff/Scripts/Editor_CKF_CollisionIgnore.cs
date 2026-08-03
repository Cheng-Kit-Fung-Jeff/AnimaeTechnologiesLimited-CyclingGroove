#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(CKF_CollisionIgnore))]
public class EditorCollisionIgnore :Editor
{
    SerializedProperty targetColliders, addChildren;
    private void OnEnable()
    {
        targetColliders = serializedObject.FindProperty("targetColliders");
        addChildren = serializedObject.FindProperty("addChildren");
    }

    override public void OnInspectorGUI() {
        EditorGUILayout.PropertyField(targetColliders);
        EditorGUILayout.PropertyField(addChildren);
        bool flagNotSelected = Selection.activeGameObject == null;
        EditorGUI.BeginDisabledGroup(flagNotSelected);
        if (GUILayout.Button("Add from selected gameobjects")) {
            HashSet<int> colliderIds = new(Enumerable.Repeat(0, targetColliders.arraySize).Select((v, i) => {
                return ((Collider)targetColliders.GetArrayElementAtIndex(i).boxedValue).GetInstanceID();
            }));
            
            if (addChildren.boolValue) {
                foreach (GameObject selectedGameObject in Selection.gameObjects)
                    foreach (Collider col in selectedGameObject.GetComponentsInChildren<Collider>(true))
                    {
                        if (colliderIds.Contains(col.GetInstanceID())) continue;
                        colliderIds.Add(col.GetInstanceID());
                        targetColliders.InsertArrayElementAtIndex(targetColliders.arraySize);
                        SerializedProperty nextEle = targetColliders.GetArrayElementAtIndex(targetColliders.arraySize - 1);
                        nextEle.boxedValue = col;
                    }
            }
            foreach (GameObject selectedGameObject in Selection.gameObjects)
                foreach (Collider col in selectedGameObject.GetComponents<Collider>())
                {
                    if (colliderIds.Contains(col.GetInstanceID())) continue;
                    colliderIds.Add(col.GetInstanceID());
                    targetColliders.InsertArrayElementAtIndex(targetColliders.arraySize);
                    SerializedProperty nextEle = targetColliders.GetArrayElementAtIndex(targetColliders.arraySize - 1);
                    nextEle.boxedValue = col;
                }

        }
        EditorGUI.BeginDisabledGroup(false);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif