using UnityEngine;
using UnityEngine.Events;

public class CKF_RandomLoop : MonoBehaviour
{
    [Min(0)]public float min, max;
    [ReadonlyField] public float currentDelay = 0;

    [ReadonlyField] public bool play = false;

    public UnityEvent eventCall;

    public void Begin()
    {
        play = true;
        currentDelay = UnityEngine.Random.Range(min, max);
    }
    public void Resume()
    {
        play = true;
    }

    public void Stop()
    {
        play = false;
    }

    private void Update()
    {
        if (play)
        {
            if (currentDelay > Time.deltaTime)
            {
                currentDelay -= Time.deltaTime;
            }
            else if (currentDelay > 0)
            {
                eventCall?.Invoke();
                currentDelay = UnityEngine.Random.Range(min, max);
            }
        }
    }

}
