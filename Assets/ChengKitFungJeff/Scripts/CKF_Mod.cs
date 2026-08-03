using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_Mod : MonoBehaviour
{
    public float value, modulus;
    public UnityEvent<float> getValue = new();

    public void SetValue(float value) { this.value = value % modulus; }

    public void Increment(float value) { this.value = (this.value+value) % modulus; }
    public void GetIncrement(float value)
    {
        this.value = (this.value + value) % modulus;
        getValue?.Invoke(this.value);
    }

    public void GetValue(float value)
    {
        this.value = value % modulus;
        getValue?.Invoke(this.value);
    }

    public void GetValue()
    {
        getValue?.Invoke(this.value);
    }
}
