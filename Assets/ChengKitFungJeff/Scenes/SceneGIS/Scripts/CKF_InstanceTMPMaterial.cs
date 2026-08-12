using TMPro;
using UnityEngine;

public class CKF_InstanceTMPMaterial : MonoBehaviour
{
    [GetSelfField] public TextMeshProUGUI selfText;

    [ReadonlyField] public Material instance;

    public void Awake()
    {
        instance = new(selfText.fontSharedMaterial);
        selfText.fontSharedMaterial = instance;
    }

    public void OnDisable()
    {
        Destroy(instance);
    }
}
