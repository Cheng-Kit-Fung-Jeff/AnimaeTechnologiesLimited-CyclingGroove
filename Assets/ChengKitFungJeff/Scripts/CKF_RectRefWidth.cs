using UnityEngine;

public class CKF_RectRefWidth : MonoBehaviour
{
    public RectTransform refRect;
    private float setted = float.NegativeInfinity;
    [Min(0)] public float ratio = 1;
    public bool preservePixel;
    [SerializeField] [GetSelfField] private CKF_RectTransform selfRect;

    public void Update()
    {
        if (setted != refRect.rect.width)
        {
            setted = refRect.rect.width;
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
        if (preservePixel) selfRect.SetLocalScaleX(refRect.rect.width * ratio * refRect.localScale.x / selfRect.GetWidth());
        else selfRect.SetWidth(refRect.rect.width * ratio); ;
    }
}
