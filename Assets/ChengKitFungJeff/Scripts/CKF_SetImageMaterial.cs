using UnityEngine;

[RequireComponent(typeof(CKF_InstanceImageMaterial))]
public class CKF_SetImageMaterial : MonoBehaviour
{
    [GetSelfField] public CKF_InstanceImageMaterial material;

    public string key;

    public void SetInt(int value)
    {
        material.instance.SetInt(key, value);
    }
    public void SetFloat(float value)
    {
        material.instance.SetFloat(key, value);
    }
    public void SetColor(Color value)
    {
        material.instance.SetColor(key, value);
    }
    public void SetTexture(Texture value)
    {
        material.instance.SetTexture(key, value);
    }
}
