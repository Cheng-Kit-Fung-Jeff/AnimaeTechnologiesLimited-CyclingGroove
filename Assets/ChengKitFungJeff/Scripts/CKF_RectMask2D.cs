using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(RectMask2D))]
public class CKF_RectMask2D : MonoBehaviour
{
    [GetSelfField]public RectMask2D selfRectMask2D;
    [GetSelfField] public RectTransform selfRectTransform;
    private float settedWidth, settedHeight;
    private Vector4 cachePadding;
    private Vector2Int cacheSoftness;

    public bool maskLeft, maskBottom, maskRight, maskTop;
    public float ratioLeft, ratioBottom, ratioRight, ratioTop;
    public float ratioSoftnessX, ratioSoftnessY;

    private void Update()
    {
        if(settedWidth != selfRectTransform.rect.width || settedHeight != selfRectTransform.rect.height)
        {
            settedWidth = selfRectTransform.rect.width;
            settedHeight = selfRectTransform.rect.height;
            cacheSoftness = new((int)(ratioSoftnessX * settedWidth), (int)(ratioSoftnessY * settedHeight));
            cachePadding = new(
                maskLeft ? ratioLeft * settedWidth : -cacheSoftness.x,
                maskBottom ? ratioBottom * settedHeight : -cacheSoftness.y,
                maskRight ? ratioRight * settedWidth : -cacheSoftness.x,
                maskTop ? ratioTop * settedHeight : -cacheSoftness.y
            );
            selfRectMask2D.padding = cachePadding;
            selfRectMask2D.softness = cacheSoftness;
        }
    }

    public void SetMaskLeft(bool value)
    {
        if (maskLeft == value) return;
        maskLeft = value;
        cachePadding.x = maskLeft ? ratioLeft * settedWidth : -cacheSoftness.x;
        selfRectMask2D.padding = cachePadding;
    }
    public void SetMaskBottom(bool value)
    {
        if (maskBottom == value) return;
        maskBottom = value;
        cachePadding.y = maskBottom ? ratioBottom * settedHeight : -cacheSoftness.y;
        selfRectMask2D.padding = cachePadding;
    }
    public void SetMaskRight(bool value)
    {
        if (maskRight == value) return;
        maskRight = value;
        cachePadding.z = maskRight ? ratioRight * settedWidth : -cacheSoftness.x;
        selfRectMask2D.padding = cachePadding;
    }
    public void SetMaskTop(bool value)
    {
        if (maskTop == value) return;
        maskTop = value;
        cachePadding.w = maskTop ? ratioTop * settedHeight : -cacheSoftness.y;
        selfRectMask2D.padding = cachePadding;
    }

    public void SetSoftnessX(float value)
    {
        if (ratioSoftnessX == value) return;
        ratioSoftnessX = value;
        cacheSoftness.x = (int)(ratioSoftnessX * settedWidth);
        cachePadding.y = maskLeft ? ratioLeft * settedWidth : -cacheSoftness.x;
        cachePadding.w = maskRight ? ratioRight * settedWidth : -cacheSoftness.x;
        selfRectMask2D.padding = cachePadding;
        selfRectMask2D.softness = cacheSoftness;
    }

    public void SetSoftnessY(float value)
    {
        if (ratioSoftnessY == value) return;
        ratioSoftnessY = value;
        cacheSoftness.y = (int)(ratioSoftnessY * settedHeight);
        cachePadding.x = maskBottom ? ratioBottom * settedHeight : -cacheSoftness.y;
        cachePadding.z = maskTop ? ratioTop * settedHeight : -cacheSoftness.y;
        selfRectMask2D.padding = cachePadding;
        selfRectMask2D.softness = cacheSoftness;
    }
}
