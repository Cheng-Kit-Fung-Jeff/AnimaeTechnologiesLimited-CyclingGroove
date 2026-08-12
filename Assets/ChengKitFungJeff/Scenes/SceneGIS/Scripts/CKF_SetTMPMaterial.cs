using UnityEngine;

public class CKF_SetTMPMaterial : MonoBehaviour
{
    [GetSelfField] public CKF_InstanceTMPMaterial material;

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
