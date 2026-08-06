using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_Clamp : MonoBehaviour
{
    public bool clampLower, clampUpper;
    public float lower, upper;

    public UnityEvent<float> getValue = new();
    public UnityEvent eventLower = new(), eventUpper = new();

    public void GetValue(float value)
    {
        float res = value;
        bool flagLower = false, flagUpper = true;
        if(clampLower && value <= lower) { flagLower = true; res = lower; }
        if(clampUpper && value >= upper) { flagUpper = true;  res = upper; }
        getValue?.Invoke(res);
        if (flagLower) eventLower?.Invoke();
        if (flagUpper) eventUpper?.Invoke();

    }
}
