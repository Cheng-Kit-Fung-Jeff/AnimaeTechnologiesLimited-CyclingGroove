using UnityEngine;
using UnityEngine.Events;

public class CKF_Counter : MonoBehaviour
{
    public UnityEvent<int> getValue = new();
    public int counter = 0;

    public void SetValue(int value) { counter = value; }

    public void GetValue()
    {
        getValue?.Invoke(counter);
    }
    public void GetValue(int value)
    {
        counter = value;
        getValue?.Invoke(counter);
    }

    public void SetIncrement(int value)
    {
        counter += value;
    }
    public void Increment(int value)
    {
        counter += value;
        getValue?.Invoke(counter);
    }
}
