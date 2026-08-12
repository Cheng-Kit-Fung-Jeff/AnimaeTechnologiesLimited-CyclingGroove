using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(-12000)]
public class CKF_InstanceImageMaterial : MonoBehaviour
{
    [GetSelfField] public Image selfImage;

    [ReadonlyField] public Material instance;

    public void Awake()
    {
        instance = new(selfImage.material);
        selfImage.material = instance;
    }

    public void OnDisable()
    {
        Destroy(instance);
    }
}
