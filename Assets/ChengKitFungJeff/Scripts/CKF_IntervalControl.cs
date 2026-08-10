using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CKF_IntervalControl : MonoBehaviour
{
    [ReadonlyField] public float pulseInterval = 0;
    public float maxInterval;
    public UnityEvent<float> getInterval = new();

    private void Update()
    {
        if (pulseInterval < maxInterval)
        {
            pulseInterval += Time.deltaTime;
            if (pulseInterval > maxInterval) pulseInterval = maxInterval;
        }
    }

    public void Action(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        getInterval?.Invoke(Mathf.Min(pulseInterval, maxInterval));
        pulseInterval = 0;
    }
    public void Action()
    {
        getInterval?.Invoke(Mathf.Min(pulseInterval, maxInterval));
        pulseInterval = 0;
    }
}
