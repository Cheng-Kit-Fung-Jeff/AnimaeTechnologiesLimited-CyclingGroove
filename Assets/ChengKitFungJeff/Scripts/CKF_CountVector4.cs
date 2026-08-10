using UnityEngine;
using UnityEngine.Events;

public class CKF_CountVector4 : MonoBehaviour
{
    public UnityEvent<Vector4> getValue;
    public UnityEvent<Color> getColor;
    [Color4Field] public Vector4 counter = Vector4.zero;

    public void SetValue(Color value) { counter = value; }
    public void SetValue(Vector4 value) { counter = value; }

    public void GetValue()
    {
        getValue?.Invoke(counter);
        getColor?.Invoke(counter);
    }
    public void GetValue(Color value)
    {
        counter = value;
        getValue?.Invoke(counter);
        getColor?.Invoke(counter);
    }
    public void GetValue(Vector4 value)
    {
        counter = value;
        getValue?.Invoke(counter);
        getColor?.Invoke(counter);
    }
    public void Increment(Vector4 value)
    {
        counter += value;
        getValue?.Invoke(counter);
        getColor?.Invoke(counter);
    }
}
