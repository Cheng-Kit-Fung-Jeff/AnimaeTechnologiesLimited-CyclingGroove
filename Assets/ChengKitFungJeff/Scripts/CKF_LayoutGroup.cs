using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CKF_LayoutGroup : MonoBehaviour
{
    [GetSelfField] public LayoutGroup selfLayout;

    public void SetPaddingLeft(float value)
    {
        selfLayout.padding =
            new RectOffset(
                (int)value,
                selfLayout.padding.right,
                selfLayout.padding.top,
                selfLayout.padding.bottom);
    }
    public void SetPaddingRight(float value)
    {
        selfLayout.padding =
            new RectOffset(
                selfLayout.padding.left,
                (int)value,
                selfLayout.padding.top,
                selfLayout.padding.bottom);
    }
    public void SetPaddingTop(float value)
    {
        selfLayout.padding =
            new RectOffset(
                selfLayout.padding.left,
                selfLayout.padding.right,
                (int)value,
                selfLayout.padding.bottom);
    }
    public void SetPaddingBottom(float value)
    {
        selfLayout.padding =
            new RectOffset(
                selfLayout.padding.left,
                selfLayout.padding.right,
                selfLayout.padding.top,
                (int)value);
    }
}
