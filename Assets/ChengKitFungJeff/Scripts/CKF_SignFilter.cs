using UnityEngine;
using UnityEngine.Events;

public class CKF_SignFilter : MonoBehaviour
{
    public UnityEvent<float>
        getFloatPositiveInclusive = new (),
        getFloatPositiveExclusive = new (),
        getFloatNegativeInclusive = new (),
        getFloatNegativeExclusive = new ()
        ;

    public void PassFloatPositiveInclusive(float value)
    {
        if (value < 0) return;
        getFloatPositiveInclusive?.Invoke(value);
    }
    public void PassFloatPositiveExclusive(float value)
    {
        if (value > 0)
            getFloatPositiveExclusive?.Invoke(value);
    }

    public void PassFloatNegativeInclusive(float value)
    {
        if (value > 0) return;
        getFloatNegativeInclusive?.Invoke(value);
    }
    public void PassFloatNegativeExclusive(float value)
    {
        if (value < 0)
            getFloatNegativeExclusive?.Invoke(value);
    }
}
