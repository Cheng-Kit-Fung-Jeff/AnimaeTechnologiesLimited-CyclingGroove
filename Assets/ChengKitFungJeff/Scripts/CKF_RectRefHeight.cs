using UnityEngine;

public class CKF_RectRefHeight : MonoBehaviour
{
    public RectTransform refRect;
    private float setted = float.NegativeInfinity;
    [Min(0)]public float ratio = 1;
    public bool preservePixel, ignoreRefScale = true;
    [SerializeField] [GetSelfField] private CKF_RectTransform selfRect;

    public void Update()
    {
        if (setted != refRect.rect.height)
        {
            setted= refRect.rect.height;
            Apply();
        }
    }
    public void SetRatio(float value)
    {
        ratio = value;
        Apply();
    }

    public void SetPreservePixels(bool value)
    {
        preservePixel = value;
        Apply();
    }

    public void Apply()
    {
        if (preservePixel)
            if (ignoreRefScale)
                selfRect.SetLocalScaleY(refRect.rect.height * ratio / selfRect.GetHeight());
            else
                selfRect.SetLocalScaleY(refRect.rect.height * ratio * refRect.localScale.y / selfRect.GetHeight());
        else
            if (ignoreRefScale)
                selfRect.SetHeight(refRect.rect.height * ratio);
        else
            selfRect.SetLocalScaleY(refRect.rect.height * ratio * refRect.localScale.y);
    }
}
