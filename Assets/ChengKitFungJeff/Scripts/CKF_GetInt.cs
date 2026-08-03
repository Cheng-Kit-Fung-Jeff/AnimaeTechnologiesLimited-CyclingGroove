using UnityEngine;
using UnityEngine.Events;

public class CKF_GetInt : MonoBehaviour
{
    public UnityEvent<int> getValue = new();

    public void GetValue(float value)
    {
        getValue?.Invoke((int)value);
    }

    public void GetValue(int value)
    {
        getValue?.Invoke(value);
    }
}
