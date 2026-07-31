using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[ExecuteInEditMode]
public class CKF_EditorLivePlay : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnEnable()
    {
        EditorApplication.update += LivePlay;
    }
    private void OnDisable()
    {
        EditorApplication.update -= LivePlay;
    }
    private void LivePlay() {
        SceneView.RepaintAll();
    }
#endif
}
