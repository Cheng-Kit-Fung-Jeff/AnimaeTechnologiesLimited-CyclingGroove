using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_CircularPath : MonoBehaviour
{
    public CKF_PathController controller;
    public RectTransform nodeA, nodeB;
    private RectTransform parent;
    public float extent;
    [Min(0)] public float maxAngle;

    public bool enterA, enterB;
    [Min(0)] public float pathMultA = 1, pathMultB = 1;
    public float pathWidth;
    [Color4Field] public Color pathInnerColor = Color.black, pathOuterColor = Color.white;

    private void Awake()
    {
        if (nodeA == null || nodeB == null || maxAngle <= 0) return;
        if (transform.parent is RectTransform rt)
            parent = rt;
        else return;
        Vector2 InvAB = nodeB.anchoredPosition - nodeA.anchoredPosition;
        InvAB = new(-InvAB.y, InvAB.x);
        Vector2 res = extent * InvAB + 0.5f * (nodeB.anchoredPosition + nodeA.anchoredPosition);
        float angle = Fn.Angle2CCD(nodeA.anchoredPosition - res, nodeB.anchoredPosition - res);
        float curAngle = angle;
        int div = 1;
        while (curAngle > maxAngle)
            curAngle = angle / ++div;
        Vector2 nodeAToRes = nodeA.anchoredPosition - res;
        string childname = name + "_child";
        Transform curNode = nodeA;
        CKF_Path newPath;
        for (float a = -curAngle; --div > 0; a += -curAngle)
        {
            Vector2 newNodePos = Fn.Rotate2CCD(nodeAToRes, Mathf.Deg2Rad * a) + res;
            GameObject newNode = new(childname);
            newNode.transform.SetParent(parent);
            RectTransform newRT = newNode.AddComponent<RectTransform>() ;
            CKF_RectTransform newCRT = newNode.AddComponent<CKF_RectTransform>() ;
            newCRT.selfRect = newRT;
            newCRT.SetAnchoredPosition(newNodePos);

            newPath = newNode.AddComponent<CKF_Path>();
            newPath.enterA = enterA;
            newPath.enterB = enterB;
            newPath.pathMultA = pathMultA;
            newPath.pathMultB = pathMultB;
            newPath.pathWidth = pathWidth;
            newPath.pathInnerColor = pathInnerColor;
            newPath.pathOuterColor = pathOuterColor;
            newPath.SetNode(curNode, newNode.transform);
            newPath.SetController(controller);
            curNode = newNode.transform;
        }
        
        newPath = nodeB.AddComponent<CKF_Path>();
        newPath.enterA = enterA;
        newPath.enterB = enterB;
        newPath.pathMultA = pathMultA;
        newPath.pathMultB = pathMultB;
        newPath.pathWidth = pathWidth;
        newPath.pathInnerColor = pathInnerColor;
        newPath.pathOuterColor = pathOuterColor;
        newPath.SetNode(curNode, nodeB);
        newPath.SetController(controller);
    }

    private void OnDrawGizmos()
    {
        if (nodeA == null || nodeB == null || nodeA == nodeB || maxAngle <= 0) return;
        Vector2 InvAB = nodeB.anchoredPosition - nodeA.anchoredPosition;
        InvAB = new(-InvAB.y, InvAB.x);
        float dist = InvAB.magnitude;
        Gizmos.color = Color.cyan;
        Vector2 res = extent * InvAB + 0.5f * (nodeB.anchoredPosition + nodeA.anchoredPosition);
        Gizmos.DrawWireSphere(new(res.x, 0, res.y), 0.1f * dist);
        float angle = Fn.Angle2CCD(nodeA.anchoredPosition - res, nodeB.anchoredPosition - res);
        float curAngle = angle;
        int div = 1;
        while (curAngle > maxAngle)
            curAngle = angle / ++div;
        Vector2 nodeAToRes = nodeA.anchoredPosition - res;
        Vector2 curNode = nodeA.anchoredPosition;
        for (float a = -curAngle; --div > 0; a += -curAngle)
        {
            Vector2 newNode = Fn.Rotate2CCD(nodeAToRes, Mathf.Deg2Rad * a) + res;
            Gizmos.color = Color.magenta;
            CKF_Path.Draw(new(curNode.x, 0, curNode.y), new(newNode.x, 0, newNode.y), transform.forward, pathWidth, enterA, enterB);
            curNode = newNode;
        }
        Gizmos.color = Color.magenta;
        CKF_Path.Draw(new(curNode.x, 0, curNode.y), new(nodeB.anchoredPosition.x, 0, nodeB.anchoredPosition.y), transform.forward, pathWidth, enterA, enterB);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new(nodeA.anchoredPosition.x, 0, nodeA.anchoredPosition.y), 0.1f * dist);
        Gizmos.DrawWireSphere(new(nodeB.anchoredPosition.x, 0, nodeB.anchoredPosition.y), 0.1f * dist);
    }
}
