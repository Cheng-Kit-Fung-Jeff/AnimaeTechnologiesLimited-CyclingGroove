using UnityEngine;
using UnityEngine.Events;

public class CKF_SetValue : MonoBehaviour
{
    public float inputOffset, outputOffset, outputScale = 1;
    public AnimationCurve curve;
    public float value;
    public float cache;
    public bool initCurve = false;

    public UnityEvent<float> getValue;

    private void Awake()
    {
        if (initCurve)
        {
            GetValue(value);
        }
    }

    public void GetValue(float value)
    {
        this.value = value;
        cache = curve.Evaluate(this.value + inputOffset);
        getValue?.Invoke(cache * outputScale + outputOffset);
    }
    public void SetInputOffset(float value)
    {
        inputOffset = value;
        cache = curve.Evaluate(this.value + inputOffset);
        getValue?.Invoke(cache * outputScale + outputOffset);
    }

    public void SetOutputOffset(float value)
    {
        outputOffset = value;
        getValue?.Invoke(cache * outputScale + outputOffset);
    }
    public void SetOutputScale(float value)
    {
        outputScale = value;
        getValue?.Invoke(cache * outputScale + outputOffset);
    }
}
