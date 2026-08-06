using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_TimeVelocityScaleController : MonoBehaviour
{
    public CKF_CountFloat remainingTime, scaledRemainingTime;
    public CKF_DTime remainingTimeScaler;
    public CKF_MAD explorer, realDistance;
    public CKF_MathDTime velocityTimeScaler;

    [Min(0)]public float initTime;
    public List<CKF_Timer> timers = new();

    public void Increment(float value)
    {
        if (remainingTime.counter <= 0) return;

        scaledRemainingTime.counter += value;

        float newScale = scaledRemainingTime.counter / remainingTime.counter;
        remainingTimeScaler.multiplier = newScale;
        explorer.mult = newScale;
        realDistance.mult = newScale;
        velocityTimeScaler.multiplier = newScale;
    }

    public void SetInitTime() { SetInitTime(initTime); }

    public void SetInitTime(float value)
    {
        remainingTime.counter = value;
        scaledRemainingTime.counter = value;
        foreach (CKF_Timer timer in timers)
        {
            timer.setTimer(value);
        }
    }
}
