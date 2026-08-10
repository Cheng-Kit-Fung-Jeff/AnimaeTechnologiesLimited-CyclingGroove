using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_RectRefPosition : MonoBehaviour
{
    [GetSelfField] public RectTransform selfRect;

    //private readonly Dictionary<int, int> parents = new();
    //private readonly List<RectTransform> parentRects = new();

    public RectTransform refRect;
    private Vector3 settedPosition;

    public void Update()
    {
        if(settedPosition != refRect.position || settedPosition != selfRect.position)
        {
            settedPosition = refRect.position;
            selfRect.position = refRect.position;
            
        }
    }

    /*private void Awake()
    {
        UpdateParents();
    }

    public void UpdateParents()
    {
        Transform parent = transform.parent;
        while (parent is RectTransform rt)
        {
            Debug.Log(rt.GetInstanceID());
            parents.Add(rt.GetInstanceID(), parentRects.Count);
            parentRects.Add(rt);
            parent = parent.parent;
        }
    }

    private void Update()
    {
        Transform root = refRect.parent;
        Vector3 anchoredPosition3D = refRect.anchoredPosition3D;
        int parentIndex = -1;
        string debParents = "";
        debParents += refRect.anchoredPosition3D + ";";
        while (root is RectTransform rt)
        {
            
            anchoredPosition3D = Vector3.Scale(rt.localScale, anchoredPosition3D) + rt.anchoredPosition3D;
            debParents += $"{root.GetInstanceID()}:{rt.localScale},{rt.anchoredPosition3D};";
            if (parents.ContainsKey(root.GetInstanceID()))
            {
                parentIndex = parents[root.GetInstanceID()];
                break;
            }
            root = rt.parent;
        }
        if(parentIndex != -1)
        {
            debParents += parentIndex + ";";
            while (true)
            {

                anchoredPosition3D =
                    Vector3.Scale(new(
                        1 / parentRects[parentIndex].localScale.x,
                        1 / parentRects[parentIndex].localScale.y,
                        1 / parentRects[parentIndex].localScale.z
                        ), anchoredPosition3D);
                anchoredPosition3D -= parentRects[parentIndex].anchoredPosition3D;
                if (--parentIndex == 0) break;
            }
            selfRect.SetAnchoredPosition(anchoredPosition3D);
            Debug.Log(anchoredPosition3D + ";"+ debParents);
        }
        else Debug.Log("Different ancestors: "+ debParents);

    }*/
}
