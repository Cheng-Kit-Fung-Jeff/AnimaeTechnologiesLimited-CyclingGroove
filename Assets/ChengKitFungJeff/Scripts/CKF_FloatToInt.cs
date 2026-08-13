using UnityEngine;
using UnityEngine.Events;

public class CKF_FloatToInt : MonoBehaviour
{
    public UnityEvent<int> getValue;

    public void GetValue(float value)
    {
        getValue?.Invoke((int)value);
    }
}
