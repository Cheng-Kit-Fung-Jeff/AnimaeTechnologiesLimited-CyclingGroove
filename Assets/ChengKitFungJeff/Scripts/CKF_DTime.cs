using UnityEngine;
using UnityEngine.Events;

public class CKF_DTime : MonoBehaviour
{
    public UnityEvent<float> getValue = new();
    public bool enable;

    private void Update()
    {
        if (enable)
        {
            getValue?.Invoke(Time.deltaTime);
        }
    }

    public void SetEnable(bool value) { enable = value; }

}
