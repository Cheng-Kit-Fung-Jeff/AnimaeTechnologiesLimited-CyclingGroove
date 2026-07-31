using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_TimeLerp : MonoBehaviour
{
    public float targetValue, currentValue, timeMult = 1;

    public UnityEvent<float> getValue;

    [ReadonlyField] public bool paused;

    public void SetValues(float target, float current) { targetValue = target; SetCurrent(current); }
    public void SetTarget(float target) { targetValue = target; }
    public void SetCurrent(float current) { currentValue = current; getValue?.Invoke(currentValue); }

    public void Pause() { paused = true; }
    public void Resume() { paused = false; }


    private void Update()
    {
        if (currentValue < targetValue)
        {
            float dt = Time.deltaTime * timeMult;
            if ((targetValue - currentValue) > dt)
            {
                currentValue += dt;
            }
            else
            {
                currentValue = targetValue;
            }
            getValue?.Invoke(currentValue);
        }
        else if (currentValue > targetValue)
        {

            float dt = Time.deltaTime * timeMult;
            if ((currentValue - targetValue) > dt)
            {
                currentValue -= dt;
            }
            else
            {
                currentValue = targetValue;
            }
            getValue?.Invoke(currentValue);
        }
    }
}
