using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_Clamp : MonoBehaviour
{
    public bool clampLower, clampUpper;
    public float lower, upper;
    [ReadonlyField] public float value;
    public UnityEvent<float> getValue = new();
    public UnityEvent eventLower, eventUpper;

    public void GetValue(float value)
    {
        this.value = value;
        bool flagLower = false, flagUpper = true;
        if(clampLower && this.value <= lower) { flagLower = true; this.value = lower; }
        if(clampUpper && this.value >= upper) { flagUpper = true; this.value = upper; }
        getValue?.Invoke(this.value);
        if (flagLower) eventLower?.Invoke();
        if (flagUpper) eventUpper?.Invoke();

    }
}
