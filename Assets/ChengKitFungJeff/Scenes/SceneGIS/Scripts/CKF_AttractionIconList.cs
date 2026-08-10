using System.Collections.Generic;
using UnityEngine;

public class CKF_AttractionIconList : MonoBehaviour
{
    [GetSelfField] [SerializeField] private RectTransform selfRT;
    
    public GameObject element;

    [System.Serializable]
    public struct Profile
    {
        public string key;
        public Sprite icon;
    }

    public List<Profile> icons = new();
    public Dictionary<string, Profile> mapIcons = new();

    public List<CKF_AttractionIconElement> tracked = new();

    private float settedWidth = float.NaN, settedHeight = float.NaN;

    [Min(0)]public float revealInterval = 0.1f;
    private bool isRevealing = false;

    private float revealTimer;

    public void Awake()
    {
        foreach (Profile profile in icons) {
            mapIcons.Add(profile.key, profile);
        }
    }

    private void Update()
    {
        if (isRevealing)
        {
            revealTimer -= Time.deltaTime;
            if (revealTimer <= 0)
            {
                while (revealTimer <= 0)
                {
                    revealTimer += revealInterval;
                }
                CallRevealed();
            }
        }
        
        if (settedHeight != selfRT.rect.width || settedHeight != selfRT.rect.height)
        {
            settedWidth = selfRT.rect.width;
            settedHeight = selfRT.rect.height;
            ElementUpdate();
        }
    }

    public void AddElement(string key)
    {
        tracked.Add(Instantiate(element, selfRT).GetComponent<CKF_AttractionIconElement>());
        tracked[^1].refHeight.refRect = selfRT;
        if(tracked[^1].icon != null)
            tracked[^1].icon.sprite = mapIcons[key].icon;
        if (tracked.Count > 1)
        {
            tracked[^2].size.SetTarget(0.5f);
            ElementUpdate();
        }
    }

    public void ElementUpdate()
    {
        if (tracked.Count > 1)
        {
            float curAnchor = settedHeight / settedWidth,
                contract_dA = (1 - curAnchor - curAnchor) / (tracked.Count - 1),
                dA = 0.5f * curAnchor;
            if (tracked.Count > 2 && contract_dA < dA)
            {
                dA = contract_dA;
                if (!isRevealing)
                {
                    isRevealing = true;
                    CallRevealed();
                }
            }
            else
            {
                isRevealing = false;
            }
            for (int i = tracked.Count - 2; i > -1; --i)
            {
                tracked[i].positionX.SetTarget(curAnchor);
                curAnchor += dA;
            }
        }
    }

    private int nextReveal, currentReveal;

    public void CallRevealed()
    {
        revealTimer = revealInterval;
        
        if (tracked.Count == 3)
        {
            tracked[0].reveal.setTimer(revealInterval);
            return;
        }
        if (nextReveal == 0)
        {
            currentReveal = 0;
            nextReveal = 1;
            tracked[0].reveal.setTimer(revealInterval);
        }
        else if (nextReveal == tracked.Count - 3)
        {
            currentReveal = tracked.Count - 3;
            nextReveal = tracked.Count - 4;
            tracked[^3].reveal.setTimer(revealInterval);
        }
        else
        {
            int temp = nextReveal;
            nextReveal += nextReveal - currentReveal;
            currentReveal = temp;
            tracked[currentReveal].reveal.setTimer(revealInterval);
        }
    }
}
