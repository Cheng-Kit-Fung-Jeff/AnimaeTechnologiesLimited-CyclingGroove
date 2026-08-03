using UnityEngine;
using UnityEngine.Events;

public class CKF_CheckProximity : MonoBehaviour
{
    public Transform target;
    public float range, min;
    public bool entered ;
    [ReadonlyField] public float dist;
    public UnityEvent eventEnter, eventExit, eventMin;
    public UnityEvent<float> eventRate;
    [ReadonlyField] public bool inMin = false;
    public void Update()
    {
        dist = Vector3.Distance(transform.position, target.position);

        if (entered)
        {
            if (dist > range)
            {
                eventExit?.Invoke();
                entered = false;
            }
        }
        else
        {
            if (dist < range)
            {
                eventEnter?.Invoke();
                entered = true;
                eventRate?.Invoke(0);
            }
        }

        if (entered)
        {
            if (!inMin && dist < min)
            {
                eventRate?.Invoke(1);
                eventMin?.Invoke();
                inMin = true;
            }
        }
        if (inMin && dist > min)
        {
            inMin = false;
        }

        if (entered && !inMin)
        {
            eventRate?.Invoke((range - dist) / (range - min));
        }
    }
}
