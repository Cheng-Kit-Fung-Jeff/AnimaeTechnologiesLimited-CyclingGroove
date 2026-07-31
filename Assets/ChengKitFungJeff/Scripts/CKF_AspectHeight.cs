using UnityEngine;

public class CKF_AspectHeight : MonoBehaviour
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
            selfRect.SetLocalScaleY(refRect.rect.width * ratio * refRect.localScale.x / selfRect.GetHeight());
        else
            selfRect.SetHeight(refRect.rect.width * ratio);
    }
}

