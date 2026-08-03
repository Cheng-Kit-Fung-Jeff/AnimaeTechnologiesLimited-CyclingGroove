using UnityEngine;
using UnityEngine.Events;

public class CKF_IntToFloat : MonoBehaviour
{
    public UnityEvent<float> getValue;
    public void GetValue(int value) { getValue?.Invoke(value); }
}
