using UnityEngine;

public class CKF_AspectWidth : MonoBehaviour
{
    public RectTransform refRect;
    [Min(0)] public float ratio = 1;
    public bool preservePixels = false;
    private CKF_RectTransform selfRect;

    public void Awake()
    {
        selfRect = GetComponent<CKF_RectTransform>();
    }
    public void Update()
    {
        if (preservePixels)
            selfRect.SetLocalScaleX(refRect.rect.height * ratio * refRect.localScale.y / selfRect.GetWidth());
        else
            selfRect.SetWidth(refRect.rect.height * ratio);
    }
}
