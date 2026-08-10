using UnityEngine;
using UnityEngine.Events;

public class CKF_Timer : MonoBehaviour
{
    [ReadonlyField] public float duration = 0;
    [ReadonlyField] public float curTime = 0;
    [ReadonlyField] public bool pause = false;

    public UnityEvent<float> eventSet;
    public UnityEvent<float> eventSetRate;
    public UnityEvent eventEnd;

    public void setTimer(float time)
    {
        if (time < 0) time = 0;
        curTime = time;
        duration = time;
        if(time != 0)
            eventSet?.Invoke(0);
        pause = false;
    }

    public void ToTime(float time)
    {
        if (time < 0) time = 0;
        curTime = time;
        if (duration != 0)
        {
            eventSet?.Invoke(duration - curTime);
            eventSetRate?.Invoke(1 - curTime / duration);
        }
    }
    public void ToTimeStart()
    {
        curTime = duration;
        if (duration != 0)
        {
            eventSet?.Invoke(0);
            eventSetRate?.Invoke(0);
        }
    }

    public void Pause()
    {
        pause = true;
    }

    public void Resume()
    {
        pause = false;
    }

    private void Update()
    {
        if (pause) return;
        if (curTime > Time.deltaTime)
        {
            curTime -= Time.deltaTime;
            eventSet?.Invoke(duration - curTime);
            eventSetRate?.Invoke(1 - curTime / duration);
        }
        else if (curTime > 0)
        {
            curTime = 0;
            eventSet?.Invoke(duration);
            eventSetRate?.Invoke(1);
            eventEnd?.Invoke();
        }
    }

    public void CallSet(float value)
    {
        eventSet?.Invoke(value);
    }

    public void CallSetRate(float value)
    {
        eventSetRate?.Invoke(value);
    }
}
