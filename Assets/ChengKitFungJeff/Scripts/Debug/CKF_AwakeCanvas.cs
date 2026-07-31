using UnityEngine;
[DefaultExecutionOrder(-9000)]
public class CKF_AwakeCanvas : MonoBehaviour
{
    private void Awake()
    {
        Canvas.ForceUpdateCanvases();
    }
}
