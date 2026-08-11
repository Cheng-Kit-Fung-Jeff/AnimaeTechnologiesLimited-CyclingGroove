using UnityEngine;
using UnityEngine.UI;

public class CKF_PathSprite : MonoBehaviour
{
    public CKF_RectTransform pathRect;
    public Transform nodeA, nodeB;
    public Vector3 forward;
    public float endPointRadius;
    private Vector3 settedA = Vector3.negativeInfinity, settedB = Vector3.negativeInfinity;
    private float settedLocalScaleY = float.NaN;
    [GetSelfField] public Image image;
    [ReadonlyField] public Material instatiatedMaterial = null;
    public string colorOuter = "_ReBlack";
    public string colorInner = "_ReWhite";

    private void Update()
    {
        if(settedA != nodeA.position || settedB != nodeB.position || settedLocalScaleY != pathRect.GetLocalScale().y)
        {
            settedA = nodeA.position; settedB = nodeB.position;
            settedLocalScaleY = pathRect.GetLocalScale().y;

            pathRect.SetPosition(0.5f * (settedA + settedB));
            if ((settedA - settedB).sqrMagnitude != 0)
            {
                pathRect.SetRotation(Quaternion.LookRotation(forward, settedA - settedB));
                pathRect.SetHeight((Vector3.Distance(settedA, settedB) + endPointRadius) / settedLocalScaleY);
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }
    }

    public void SetWidth(float value)
    {
        if (value == 0) { Debug.Log($"path connecting {nodeA.name}, {nodeB.name} attempted to set path width as 0");  return;}
        pathRect.SetLocalScaleX(value / pathRect.GetWidth());
        pathRect.SetLocalScaleY(pathRect.GetLocalScale().x);
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