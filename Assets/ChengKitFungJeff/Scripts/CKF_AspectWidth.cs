using UnityEngine;
using UnityEngine.UI;

public class CKF_AspectWidth : MonoBehaviour
{
    public RectTransform refRect;
    [Min(0)] public float ratio = 1;
    public bool preservePixels = false;
    [GetSelfField] [SerializeField] private CKF_RectTransform selfRect;
    [GetSelfField][SerializeField] private LayoutElement selfLayoutElement;
    private float settedHeight = float.NaN, settedLocalScaleY = float.NaN;
    
    public void Update()
    {
        if (preservePixels)
        {
            if (settedHeight != refRect.rect.height || settedLocalScaleY != refRect.localScale.y)
            {
                settedHeight = refRect.rect.height;
                settedLocalScaleY = refRect.localScale.y;
                if(selfRect.GetWidth() != 0)
                    selfRect.SetLocalScaleX(settedHeight * ratio * settedLocalScaleY / selfRect.GetWidth());
            }
        }
        else if (settedHeight != refRect.rect.height)
        {
            settedHeight = refRect.rect.height;
            selfRect.SetWidth(settedHeight * ratio);
            if(selfLayoutElement != null)
                selfLayoutElement.preferredWidth = selfRect.GetWidth();
        }
    }

    public void SetRatio(float value)
    {
        ratio = value;
        if (preservePixels)
        {
            if (selfRect.GetWidth() != 0)
                selfRect.SetLocalScaleX(settedHeight * ratio * settedLocalScaleY / selfRect.GetWidth());
            if (selfLayoutElement != null)
                selfLayoutElement.preferredWidth = selfRect.GetWidth();
        }
        else
        {
            settedHeight = refRect.rect.height;
            selfRect.SetWidth(settedHeight * ratio);
        }
    }
}
