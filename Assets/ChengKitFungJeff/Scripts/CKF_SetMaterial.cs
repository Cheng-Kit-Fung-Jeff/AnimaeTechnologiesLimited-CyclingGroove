using UnityEngine;

public class CKF_SetMaterial : MonoBehaviour
{
    public Renderer targetRenderer;
    public int materialIndex;
    public Material material;
    public string key;
    private void Awake()
    {
        material = material == null ? targetRenderer.materials[materialIndex] : material;
    }
    public void SetKey(string value) { key = value; }
    public void SetFloat(float value) { material.SetFloat(key, value); }
    public void SetColor(Color value) { material.SetColor(key, value); }
    public void SetInt(int value) { material.SetInteger(key,value); }
}
