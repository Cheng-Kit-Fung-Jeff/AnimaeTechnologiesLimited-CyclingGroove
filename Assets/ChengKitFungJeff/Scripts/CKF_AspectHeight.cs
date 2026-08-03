using System.Reflection;
using UnityEngine;

public class CKF_AspectHeight : MonoBehaviour
{
    public RectTransform refRect;
    [Min(0)] public float ratio = 1;
    public bool preservePixels = false;
    private CKF_RectTransform selfRect;
    private float settedWidth = float.NaN, settedLocalScaleX = float.NaN;

    public void Awake()
    {
        selfRect = GetComponent<CKF_RectTransform>();
    }
    public void Update()
    {
        if (preservePixels)
        {
            if(settedWidth != refRect.rect.width || settedLocalScaleX != refRect.localScale.x)
            {
                settedWidth = refRect.rect.width;
                settedLocalScaleX = refRect.localScale.x;
                if(selfRect.GetHeight() != 0)
                    selfRect.SetLocalScaleY(settedWidth * ratio * settedLocalScaleX / selfRect.GetHeight());
            }
        }
        else if(settedWidth != refRect.rect.width)
        {
            settedWidth = refRect.rect.width;
            selfRect.SetHeight(settedWidth * ratio);
        }
    }

    public void SetRatio(float value)
    {
        ratio = value;
        if (preservePixels)
        {
            if (selfRect.GetHeight() != 0)
                selfRect.SetLocalScaleY(settedWidth * ratio * settedLocalScaleX / selfRect.GetHeight());
        }
        else
        {
            selfRect.SetHeight(settedWidth * ratio);
        }
    }
}

