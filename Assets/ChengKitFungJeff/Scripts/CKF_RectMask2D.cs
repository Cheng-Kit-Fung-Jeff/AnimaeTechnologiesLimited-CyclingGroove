using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CKF_RectMask2D : MonoBehaviour
{
    public RectMask2D target;
    public RectTransform parent;
    private int settedParentID = int.MinValue;
    private float settedWidth, settedHeight;
    private Vector4 cachePadding;
    private Vector2Int cacheSoftness;

    public bool maskLeft, maskBottom, maskRight, maskTop;
    public float ratioLeft, ratioBottom, ratioRight, ratioTop;
    public float ratioSoftnessX, ratioSoftnessY;

    private void Update()
    {
        if(parent != null && (settedParentID != parent.GetInstanceID() || settedWidth != parent.rect.width || settedHeight != parent.rect.height))
        {
            settedParentID = parent.GetInstanceID();
            settedWidth = parent.rect.width;
            settedHeight = parent.rect.height;
            cacheSoftness = new((int)(ratioSoftnessX * settedWidth), (int)(ratioSoftnessY * settedHeight));
            cachePadding = new(
                maskLeft ? ratioLeft * settedWidth : -cacheSoftness.x,
                maskBottom ? ratioBottom * settedHeight : -cacheSoftness.y,
                maskRight ? ratioRight * settedWidth : -cacheSoftness.x,
                maskTop ? ratioTop * settedHeight : -cacheSoftness.y
            );
            target.padding = cachePadding;
            target.softness = cacheSoftness;
        }
    }

    public void SetMaskLeft(bool value)
    {
        if (maskLeft == value) return;
        maskLeft = value;
        cachePadding.x = maskLeft ? ratioLeft * settedWidth : -cacheSoftness.x;
        target.padding = cachePadding;
    }
    public void SetMaskBottom(bool value)
    {
        if (maskBottom == value) return;
        maskBottom = value;
        cachePadding.y = maskBottom ? ratioBottom * settedHeight : -cacheSoftness.y;
        target.padding = cachePadding;
    }
    public void SetMaskRight(bool value)
    {
        if (maskRight == value) return;
        maskRight = value;
        cachePadding.z = maskRight ? ratioRight * settedWidth : -cacheSoftness.x;
        target.padding = cachePadding;
    }
    public void SetMaskTop(bool value)
    {
        if (maskTop == value) return;
        maskTop = value;
        cachePadding.w = maskTop ? ratioTop * settedHeight : -cacheSoftness.y;
        target.padding = cachePadding;
    }

    public void SetSoftnessX(float value)
    {
        if (ratioSoftnessX == value) return;
        cacheSoftness.x = (int)(ratioSoftnessX * settedWidth);
        cachePadding.x = maskBottom ? ratioBottom * settedHeight : -cacheSoftness.y;
        cachePadding.z = maskTop ? ratioTop * settedHeight : -cacheSoftness.y;
        target.padding = cachePadding;
        target.softness = cacheSoftness;
    }

    public void SetSoftnessY(float value)
    {
        if (ratioSoftnessY == value) return;
        cacheSoftness.y = (int)(ratioSoftnessY * settedHeight);
        cachePadding.y = maskLeft ? ratioLeft * settedWidth : -cacheSoftness.x;
        cachePadding.w = maskRight ? ratioRight * settedWidth : -cacheSoftness.x;
        target.padding = cachePadding;
        target.softness = cacheSoftness;
    }
}
