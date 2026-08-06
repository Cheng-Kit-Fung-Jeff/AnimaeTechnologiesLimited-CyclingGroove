using UnityEngine;
using UnityEngine.Events;

public class CKF_CountFloat : MonoBehaviour
{
    public UnityEvent<float> getValue = new();
    public float counter = 0;

    public void SetValue(int value) { counter = value; }
    public void SetValue(float value) { counter = value; }

    public void GetValue()
    {
        getValue?.Invoke(counter);
    }
    public void GetValue(int value)
    {
        counter = value;
        getValue?.Invoke(counter);
    }
    public void GetValue(float value)
    {
        counter = value;
        getValue?.Invoke(counter);
    }
    public void Increment(int value)
    {
        counter += value;
        getValue?.Invoke(counter);
    }
    public void Increment(float value)
    {
        counter += value;
        getValue?.Invoke(counter);
    }
}
