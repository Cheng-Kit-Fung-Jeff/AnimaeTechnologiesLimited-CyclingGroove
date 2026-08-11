using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CKF_GridLayout : CKF_LayoutGroup
{
    public void SetCellSizeX(float value)
    {
        ((GridLayoutGroup)selfLayout).cellSize
            = new(value, ((GridLayoutGroup)selfLayout).cellSize.y);
    }
    public void SetCellSizeY(float value)
    {
        ((GridLayoutGroup)selfLayout).cellSize
            = new(((GridLayoutGroup)selfLayout).cellSize.x, value);
    }
    public void SetCellSizeXY(float value)
    {
        ((GridLayoutGroup)selfLayout).cellSize
            = new(value, value);
    }
}
