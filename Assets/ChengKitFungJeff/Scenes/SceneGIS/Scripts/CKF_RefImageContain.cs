using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CKF_RectTransform))]
public class CKF_RefImageContain : MonoBehaviour
{
    [GetSelfField] public CKF_RectTransform selfRect;
    public RectTransform target;

    private float width, height, setted_dX = float.NaN, setted_dY = float.NaN;
    private void Awake()
    {
        UpdateImage();
    }

    private void Update()
    {
        if (width == 0 || height == 0) UpdateImage();
        if (width == 0 || height == 0) return;

        float dX = target.rect.width * height, dY = target.rect.height * width;



        if (setted_dX != dX || setted_dY != dY)
        {
            setted_dX = dX;
            setted_dY = dY;
            if (dX > dY)
            {
                selfRect.SetAnchorMinY(0);
                selfRect.SetAnchorMaxY(1);

                float d = 0.5f * dY / dX;
                selfRect.SetAnchorMinX(0.5f - d);
                selfRect.SetAnchorMaxX(0.5f + d);
            }
            else
            {
                selfRect.SetAnchorMinX(0);
                selfRect.SetAnchorMaxX(1);

                float d = 0.5f * dX / dY;
                selfRect.SetAnchorMinY(0.5f - d);
                selfRect.SetAnchorMaxY(0.5f + d);
            }
        }
    }

    public void UpdateImage()
    {
        {
            if (GetComponent<Image>() is Image i)
            {
                if (i.sprite != null)
                {
                    width = i.sprite.rect.width;
                    height = i.sprite.rect.height;
                }
            }
        }
        {
            if (GetComponent<RawImage>() is RawImage i)
            {
                if (i.texture != null)
                {
                    width = i.texture.width;
                    height = i.texture.height;
                }
            }
        }
    }
}
