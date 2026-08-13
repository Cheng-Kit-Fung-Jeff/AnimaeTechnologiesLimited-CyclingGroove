using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CKF_AttractionImageController : MonoBehaviour
{
    public Image image, icon;

    [Min(0)]public float minShowDuration, overlayTime;
    [ReadonlyField] public float showTimeRemaining;

    public CKF_Timer overlayTimer;
    public TextMeshProUGUI text;
    public CKF_RefImageCover imageRefImageCover;
    public CKF_RefImageContain iconRefImageContain;
    public bool overlayIsVisible = true;

    [System.Serializable]
    public class Profile
    {
        public string key;
        [TextArea]
        public string name;
        public Sprite image;
        public Sprite icon;
    }

    public List<Profile> attractions;

    private readonly Dictionary<string, Profile> mapAttractions = new();

    private readonly List<string> buffer = new();
    public UnityEvent becomeVisible;

    private void Awake()
    {
        foreach (var attraction in attractions)
        {
            mapAttractions.Add(attraction.key,attraction);
        }
    }

    private void Update()
    {
        if (showTimeRemaining > Time.deltaTime)
        {
            showTimeRemaining -= Time.deltaTime;
        }
        else
        {
            if (buffer.Count > 0)
            {
                while(showTimeRemaining <= Time.deltaTime)
                    showTimeRemaining += minShowDuration;
                string key = buffer[0];
                buffer.RemoveAt(0);
                overlayTimer.setTimer(overlayTime);
                if (overlayIsVisible)
                {
                    overlayIsVisible = false;
                    overlayTimer.ToTime(0.5f * overlayTime);
                    SetProfile(mapAttractions[key]);
                    becomeVisible?.Invoke();
                }
                else
                {
                    StartCoroutine(AddImage(mapAttractions[key]));
                }
            }
            else
            {
                showTimeRemaining = -1;
            }
        }
    }

    IEnumerator AddImage(Profile profile)
    {
        yield return new WaitForSeconds(0.5f * overlayTime);
        SetProfile(profile);
        yield return null;
    }
    private void SetProfile(Profile profile)
    {
        image.sprite = profile.image;
        icon.sprite = profile.icon;
        text.text = profile.name;
        imageRefImageCover.UpdateImage();
        iconRefImageContain.UpdateImage();
    }
    public void AddTexture(Texture t)
    {
    }
    public void AddTexture(string key)
    {
        buffer.Add(key);
    }
}
