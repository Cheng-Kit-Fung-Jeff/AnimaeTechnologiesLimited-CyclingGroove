using System.Collections.Generic;
using UnityEngine;

public class CKF_CommentList : MonoBehaviour
{
    [Min(1)] public int listSize = 1;
    private float iSizeRatio = 0; // sets wrapper and contents

    public CKF_RectMask2D counterWrapper;

    public GameObject element, dummy;
    public RectTransform CommentLayer;

    public List<ElementProfile> elementProfiles = new();
    public readonly Dictionary<string, ElementProfile> mapElemenProfiles = new();
    [System.Serializable]
    public struct ElementProfile
    {
        public string key;
        public Sprite icon;
    }

    private readonly List<CKF_CommentElement> listElements = new(); // bubble sorting is best;

    private void Awake()
    {
        foreach (var p in elementProfiles)
        {
            mapElemenProfiles.Add(p.key, p);
        }

        // ex listsize =  4
        // fading, shown, shown, shown
        iSizeRatio = 1 / (float)listSize;
        counterWrapper.SetSoftnessY(iSizeRatio);
        for (int i = 0; i++ < listSize;)
        {
            listElements.Add(Instantiate(dummy, CommentLayer).GetComponent<CKF_CommentElement>());
            listElements[^1].refHeight.refRect = CommentLayer;
            listElements[^1].refHeight.SetRatio(iSizeRatio);
        }
    }

    public void AddComment(string key, string comment)
    {
        if (!mapElemenProfiles.ContainsKey(key)) return;
        CKF_CommentElement newEle = Instantiate(element, CommentLayer).GetComponent<CKF_CommentElement>();
        newEle.refHeight.refRect = CommentLayer;
        newEle.refHeight.SetRatio(iSizeRatio);
        newEle.transform.SetAsLastSibling();
        newEle.icon.sprite = mapElemenProfiles[key].icon;
        newEle.text.text = comment;

        listElements.Add(newEle);
        float targetPos = iSizeRatio;
        if (listElements.Count > listSize) { Destroy(listElements[0].gameObject); listElements.RemoveAt(0); }
        for (int i = listElements.Count, v = 1; --i > 0; v++)
        {
            if (newEle.indexState != null) listElements[i].indexState.GetValue(v);
            targetPos += iSizeRatio;
        }
    }
}
