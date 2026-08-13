using UnityEngine;
using UnityEngine.Events;

public class CKF_RandomFloat : MonoBehaviour
{
    public float start;
    [Min(0)] public float range;
    public UnityEvent<float> getValue;
    public void GetValue()
    {
        getValue?.Invoke(UnityEngine.Random.Range(start, start + range));
    }
}
