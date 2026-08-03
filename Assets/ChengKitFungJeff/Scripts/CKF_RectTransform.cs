using UnityEngine;

public class CKF_RectTransform : MonoBehaviour
{
    [GetSelfField] public RectTransform selfRect = null;

    public Vector3 GetLocalScale()
    {
        return selfRect.localScale;
    }

    public void SetLocalScaleX(float value)
    {
        selfRect.localScale = new(value, selfRect.localScale.y, selfRect.localScale.z);
    }
    public void SetLocalScaleY(float value)
    {
        selfRect.localScale = new(selfRect.localScale.x, value, selfRect.localScale.z);
    }
    public void SetLocalScaleZ(float value)
    {
        selfRect.localScale = new(selfRect.localScale.x, selfRect.localScale.y, value);
    }

    public void SetRotation(Quaternion value)
    {
        selfRect.rotation = value;
    }
    public void SetLocalRotation(Quaternion value)
    {
        selfRect.localRotation = value;
    }

    public void SetLocalRotationX(float value)
    {
        selfRect.localRotation = Quaternion.Euler(value, selfRect.localRotation.eulerAngles.y, selfRect.localRotation.eulerAngles.z);
    }
    public void SetLocalRotationY(float value)
    {
        selfRect.localRotation = Quaternion.Euler(selfRect.localRotation.eulerAngles.x, value, selfRect.localRotation.eulerAngles.z);
    }
    public void SetLocalRotationZ(float value)
    {
        selfRect.localRotation = Quaternion.Euler(selfRect.localRotation.eulerAngles.x, selfRect.localRotation.eulerAngles.y, value);
    }

    public void SetPosition(Vector3 value)
    {
        selfRect.position = value;
    }

    public Vector3 GetAnchoredPosition()
    {
        return selfRect.anchoredPosition3D;
    }

    public void SetAnchoredPositionX(float value)
    {
        selfRect.anchoredPosition3D = new(value, selfRect.anchoredPosition3D.y, selfRect.anchoredPosition3D.z);
    }

    public void SetAnchoredPositionY(float value)
    {
        selfRect.anchoredPosition3D = new(selfRect.anchoredPosition3D.x, value, selfRect.anchoredPosition3D.z);
    }

    public void SetAnchoredPositionZ(float value)
    {
        selfRect.anchoredPosition3D = new(selfRect.anchoredPosition3D.x, selfRect.anchoredPosition3D.y, value);
    }

    public void SetAnchoredPosition(Vector3 value)
    {
        selfRect.anchoredPosition3D = value;
    }

    public void SetAnchorMinX(float value)
    {
        Vector3 temp = selfRect.anchoredPosition3D;
        selfRect.anchorMin = new(value, selfRect.anchorMin.y);
        selfRect.anchoredPosition3D = temp;
    }
    public void SetAnchorMinY(float value)
    {
        Vector3 temp = selfRect.anchoredPosition3D;
        selfRect.anchorMin = new(selfRect.anchorMin.x, value);
        selfRect.anchoredPosition3D = temp;
    }
    public void SetAnchorMaxX(float value)
    {
        Vector3 temp = selfRect.anchoredPosition3D;
        selfRect.anchorMax = new(value, selfRect.anchorMax.y);
        selfRect.anchoredPosition3D = temp;
    }
    public void SetAnchorMaxY(float value)
    {
        Vector3 temp = selfRect.anchoredPosition3D;
        selfRect.anchorMax = new(selfRect.anchorMax.x, value);
        selfRect.anchoredPosition3D = temp;
    }
    public void SetAnchorMinMaxX(float value)
    {
        SetAnchorMinX(value);
        SetAnchorMaxX(value);
    }
    public void SetAnchorMinMaxY(float value)
    {
        SetAnchorMinY(value);
        SetAnchorMaxY(value);
    }
    public Vector2 GetAnchorMin()
    {
        return selfRect.anchorMin;
    }
    public Vector2 GetAnchorMax()
    {
        return selfRect.anchorMax;
    }

    public void SetWidth(float value)
    {
        selfRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
    }
    public float GetWidth()
    {
        return selfRect.rect.width;
    }
    public void SetHeight(float value)
    {
        selfRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
    }
    public float GetHeight()
    {
        return selfRect.rect.height;
    }

    public void SetOffsetMinX(float value)
    {
        selfRect.offsetMin = new(value, selfRect.offsetMin.y);
    }
    public void SetOffsetMinY(float value)
    {
        selfRect.offsetMin = new(selfRect.offsetMin.x, value);
    }
    public void SetOffsetMaxX(float value)
    {
        selfRect.offsetMax = new(value, selfRect.offsetMax.y);
    }
    public void SetOffsetMaxY(float value)
    {
        selfRect.offsetMax = new(selfRect.offsetMax.x, value);
    }
}
