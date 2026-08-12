using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_TimeVelocityScaleController : MonoBehaviour
{
    public CKF_CountFloat remainingTime, scaledRemainingTime;
    public CKF_DTime remainingTimeScaler;
    public CKF_MAD explorer, realDistance;
    public List<CKF_MAD> scalersMAD = new();
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
        foreach (var s in scalersMAD) s.mult = newScale;
    }

    public void IncrementFactor(float value)
    {
        if (value < 0) return;
        Increment(scaledRemainingTime.counter * value);
    }

    public void SetInitTime() { SetInitTime(initTime); }

    public void SetInitTime(float value)
    {
        Debug.Log("Set init time: " + value);
        initTime = value;
        remainingTime.counter = initTime;
        scaledRemainingTime.counter = initTime;
        foreach (CKF_Timer timer in timers)
        {
            timer.setTimer(initTime);
        }
    }
}
