using UnityEngine;
using UnityEngine.Events;

public class CKF_MathDTime : MonoBehaviour
{
    public UnityEvent<float> getDiv = new(), getMul = new(), getAdd = new(), getSub = new(), getValue = new();

    public float multiplier = 1;

    public void SetMultiplier(float value) { multiplier = value; }

    public void GetDiv(float value)
    {
        getDiv?.Invoke(value / (Time.deltaTime * multiplier));
    }
    public void GetMul(float value)
    {
        getMul?.Invoke(value * Time.deltaTime * multiplier);
    }

    public void GetAdd(float value)
    {
        getAdd?.Invoke(value + (Time.deltaTime * multiplier));
    }

    public void GetSub(float value)
    {
        getSub?.Invoke(value - (Time.deltaTime * multiplier));
    }
    public void GetValue()
    {
        getValue?.Invoke(Time.deltaTime * multiplier);
    }
}
