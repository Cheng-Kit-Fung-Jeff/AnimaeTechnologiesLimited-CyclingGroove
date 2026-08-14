using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_Mod : MonoBehaviour
{
    public float value, modulus;
    public UnityEvent<float> getValue = new();

    public void SetValue(float value)
    {
        this.value = value % modulus;
        if (this.value < 0) this.value += Mathf.Abs(modulus);
    }

    public void Increment(float value)
    {
        this.value = (this.value + value) % modulus;
        if (this.value < 0) this.value += Mathf.Abs(modulus);
    }
    public void GetIncrement(float value)
    {
        this.value = (this.value + value) % modulus;
        if (this.value < 0) this.value += Mathf.Abs(modulus);
        getValue?.Invoke(this.value);
    }

    public void GetValue(float value)
    {
        this.value = value % modulus;
        if (this.value < 0) this.value += Mathf.Abs(modulus);
        getValue?.Invoke(this.value);
    }

    public void GetValue()
    {
        getValue?.Invoke(value);
    }
}
