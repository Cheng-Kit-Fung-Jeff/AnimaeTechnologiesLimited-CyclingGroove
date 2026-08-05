using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_PolyPath : MonoBehaviour
{
    public CKF_PathController controller;
    public Transform nodeA;
    public List<Transform> nodes;
    public Transform nodeB;
    public bool enterA, enterB, enteredA, enteredB;
    [Min(0)] public float pathMultA = 1, pathMultB = 1, width;
    [Color4Field] public Color pathInnerColor = Color.black, pathOuterColor = Color.white;
#if UNITY_EDITOR
    public enum Mode
    {
        Arc,
        Split,
    }
    public Mode mode;
    public GameObject prefabAnchor;
    public Transform midArc;
    public float minAngle;
#endif
#if UNITY_EDITOR

#endif
}
