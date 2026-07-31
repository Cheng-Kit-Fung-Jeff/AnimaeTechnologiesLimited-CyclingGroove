using UnityEngine;
using UnityEngine.UI;

public class CKF_RectSpriteAspectRatio : MonoBehaviour
{
    private CKF_RectTransform selfRectController;
    private float width, height;
    public float xRatio, yRatio;


    private void Awake()
    {
        selfRectController = GetComponent<CKF_RectTransform>();
        {
            if (GetComponent<Image>() is Image i)
            {
                width = i.sprite.rect.width; height = i.sprite.rect.height;
            }
        }
        {
            if (GetComponent<RawImage>() is RawImage i)
            {
                width = i.texture.width; height = i.texture.height;
            }
        }

        if (width > height)
        {
            selfRectController.SetAnchorMinX(0);
            selfRectController.SetAnchorMaxX(1);
            float ratio = height / width;
            selfRectController.SetAnchorMinY(yRatio * (1 - ratio));
            selfRectController.SetAnchorMaxY((1 - yRatio) * ratio + yRatio);
        }
        else
        {
            selfRectController.SetAnchorMinY(0);
            selfRectController.SetAnchorMaxY(1);
            float ratio = width / height;
            selfRectController.SetAnchorMinX(xRatio * (1 - ratio));
            selfRectController.SetAnchorMaxX((1 - xRatio) * ratio + xRatio);
        }

    }
}
