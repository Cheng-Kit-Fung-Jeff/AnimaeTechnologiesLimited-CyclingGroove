using UnityEngine;
using UnityEngine.Events;

public class CKF_MAD : MonoBehaviour
{
    public float mult = 1, add;
    [ReadonlyField] public float lastValue , lastResult;
    public UnityEvent<float> getValue = new();
    public void GetValue(int value) { lastValue = value; lastResult = mult * lastValue + add; getValue?.Invoke(lastResult); }
    public void GetValue(float value) { lastValue = value; lastResult = mult * lastValue + add; getValue?.Invoke(lastResult); }

    public void SetMult(float value) { mult = value; }
    public void SetAdd(float value) { add = value; }
    public void GetLastResult() { getValue?.Invoke(lastResult); }
}
