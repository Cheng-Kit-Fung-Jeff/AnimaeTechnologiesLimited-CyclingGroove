using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CKF_CommentList : MonoBehaviour
{
    [Min(1)] public int listSize = 1;
    public RectTransform refRect;

    //public CKF_RectMask2D counterWrapper;

    public GameObject element;
    public RectTransform CommentLayer;

    public List<ElementProfile> elementProfiles = new();
    public readonly Dictionary<string, ElementProfile> mapElemenProfiles = new();
    [System.Serializable]
    public struct ElementProfile
    {
        public string key;
        public Sprite icon;
    }

    private readonly List<CKF_CommentElement> listElements = new();
    private void Awake()
    {
        foreach (var p in elementProfiles)
        {
            mapElemenProfiles.Add(p.key, p);
        }
    }

    public void AddComment(string key, string comment)
    {
        if (!mapElemenProfiles.ContainsKey(key)) return;
        CKF_CommentElement newEle = Instantiate(element, CommentLayer).GetComponent<CKF_CommentElement>();
        newEle.transform.SetAsLastSibling();
        newEle.icon.sprite = mapElemenProfiles[key].icon;
        newEle.text.text = comment;
        foreach (var c in newEle.rectRefWidth)
        {
            c.refRect = refRect;
            c.Apply();
        }
        foreach (var c in newEle.rectRefHeight)
        {
            c.refRect = refRect;
            c.Apply();
        }
        /*
        foreach (var c in newEle.getRectWidth)
        {
            c.refRect = refRect;
            c.Apply();
        }
        */
        foreach (var c in newEle.getRectHeight)
        {
            c.refRect = refRect;
            c.Apply();
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(CommentLayer);


        listElements.Add(newEle);
        if (listElements.Count > listSize) { Destroy(listElements[0].gameObject); listElements.RemoveAt(0); }
        for (int i = listElements.Count, v = 1; --i > 0; v++)
        {
            if (newEle.indexState != null) listElements[i].indexState.GetValue(v);
        }
    }
}
