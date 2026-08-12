using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(2000)]
public class CKF_PathRectSprite : MonoBehaviour
{
    [GetSelfField] public CKF_RectTransform pathRect;
    public RectTransform nodeA, nodeB;
    public float endPointRadius;
    private Vector3 settedA = Vector3.negativeInfinity, settedB = Vector3.negativeInfinity;
    private float settedLocalScaleY = float.NaN;

    [GetSelfField] public Image image;
    [ReadonlyField] public Material instatiatedMaterial = null;
    public string colorOuter = "_ReBlack";
    public string colorInner = "_ReWhite";

    private void Update()
    {
        if (settedA != nodeA.anchoredPosition3D || settedB != nodeB.anchoredPosition3D || settedLocalScaleY != pathRect.GetLocalScale().y)
        {
            UpdateHeight();
        }
    }

    public void SetWidth(float value)
    {
        pathRect.SetLocalScaleX(value / pathRect.GetWidth());
        pathRect.SetLocalScaleY(pathRect.GetLocalScale().x);
        //image.pixelsPerUnitMultiplier = Mathf.Min(endPointRadius / value, endPointRadius);
    }

    public void UpdateHeight()
    {
        settedA = nodeA.anchoredPosition3D; settedB = nodeB.anchoredPosition3D;
        settedLocalScaleY = pathRect.GetLocalScale().y;
        pathRect.SetAnchoredPosition(0.5f * (settedA + settedB));
        float sqrDist = (settedA - settedB).sqrMagnitude;
        if (sqrDist != 0)
        {
            pathRect.SetLocalRotationZ(Mathf.Rad2Deg * Mathf.Atan2(settedB.y - settedA.y, settedB.x - settedA.x) + 90);
            pathRect.SetHeight(Mathf.Sqrt(sqrDist) / settedLocalScaleY + endPointRadius);
        }
        else
        {
            pathRect.SetHeight(0);
        }
    }

    public void SetOuterColor(Color value)
    {
        if (instatiatedMaterial == null)
        {
            instatiatedMaterial = Instantiate(image.material);
            image.material = instatiatedMaterial;
        }
        instatiatedMaterial.SetColor(colorOuter, value);
    }
    public void SetInnerColor(Color value)
    {
        if (instatiatedMaterial == null)
        {
            instatiatedMaterial = Instantiate(image.material);
            image.material = instatiatedMaterial;
        }
        instatiatedMaterial.SetColor(colorInner, value);
    }
}
