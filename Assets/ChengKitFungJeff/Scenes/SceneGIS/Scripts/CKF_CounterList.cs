using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_CounterList : MonoBehaviour, I_CKF_Reset
{
    [Min(1)] public int listSize = 1;
    private float iSizeRatio = 0; // sets wrapper and contents

    public CKF_RectMask2D counterWrapper;

    public GameObject element;
    public List<ElementProfile> elementProfiles = new();
    public readonly Dictionary<string,ElementProfile> mapElemenProfiles = new();
    [System.Serializable]
    public class ElementProfile
    {
        public string key;
        public Sprite icon;
        public Material material;
    }

    private readonly Dictionary<string, int> listElementsIndex = new();
    private readonly List<CountingProfile> listElements = new(); // bubble sorting is best;

    public class CountingProfile
    {
        public string key;
        public int count;
        public CKF_CounterListElement element;
    }

    private void Awake()
    {
        foreach (var p in elementProfiles)
        {
            mapElemenProfiles.Add(p.key, p);
        }
        
        // ex listsize =  4
        // shown, shown, shown, fading
        iSizeRatio = 1 / (float)listSize;
        counterWrapper.SetSoftnessY(iSizeRatio);
    }

    private void Update()
    {
        for (int i = 1; i < listElements.Count; i++)
        {
            if (listElements[i].count > listElements[i - 1].count)
            {
                listElements[i].element.transform.SetSiblingIndex(listElements.Count - i);
                if(i <= listSize)
                {
                    listElements[i].element.positionLerp.SetTarget(1 - (i - 1) * iSizeRatio);
                    listElements[i - 1].element.positionLerp.SetTarget(1 - i * iSizeRatio);
                }
                listElementsIndex[listElements[i].key] = i - 1;
                listElementsIndex[listElements[i-1].key] = i;

                CountingProfile temp = listElements[i];
                listElements[i] = listElements[i - 1];
                listElements[i - 1] = temp;

                if (listElements[i].element.indexState != null)
                    listElements[i].element.indexState.GetValue(i);
                if (listElements[i - 1].element.indexState != null)
                    listElements[i - 1].element.indexState.GetValue(i - 1);
            }
        }
    }

    public void IncrementElement(string key)
    {
        if (!listElementsIndex.ContainsKey(key))
        {
            listElementsIndex.Add(key, listElements.Count);
            CKF_CounterListElement newEle = Instantiate(element, counterWrapper.transform).GetComponent<CKF_CounterListElement>();
            newEle.transform.SetAsFirstSibling();
            newEle.icon.sprite = mapElemenProfiles[key].icon;
            if (mapElemenProfiles[key].material != null) newEle.icon.material = mapElemenProfiles[key].material;
            newEle.refWidth.refRect = counterWrapper.transform as RectTransform;
            newEle.refHeight.refRect = counterWrapper.transform as RectTransform;
            newEle.refHeight.SetRatio(iSizeRatio);
            newEle.positionLerp.SetValues(listElements.Count < listSize? 1 - listElements.Count * iSizeRatio : 0, 0);
            newEle.counter.text = "1";
            CountingProfile newCountingProfile = new() { key = key, count = 1, element = newEle };
            listElements.Add(newCountingProfile);
            if (newEle.indexState != null) newEle.indexState.GetValue(listElements.Count - 1);
        }
        else
        {
            listElements[listElementsIndex[key]].count++;
            listElements[listElementsIndex[key]].element.counter.text = listElements[listElementsIndex[key]].count.ToString();
        }
    }

    public void SceneReset()
    {
        foreach (var v in listElements)
            Destroy(v.element.gameObject);
        listElements.Clear();
    }
}
