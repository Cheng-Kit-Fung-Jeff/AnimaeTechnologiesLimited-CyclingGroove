using UnityEngine;
using UnityEngine.Events;

public class CKF_DTime : MonoBehaviour
{
    public UnityEvent<float> getValue = new();
    public bool enable;
    public float multiplier = 1;

    private void Update()
    {
        if (enable)
        {
            getValue?.Invoke(Time.deltaTime * multiplier);
        }
    }

    public void SetEnable(bool value) { enable = value; }

    public void SetMultiplier(float value) { multiplier = value; }

}
