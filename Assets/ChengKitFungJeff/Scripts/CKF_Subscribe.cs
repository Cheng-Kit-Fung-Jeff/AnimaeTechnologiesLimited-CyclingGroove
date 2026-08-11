using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-15000)]
public class CKF_Subscribe : MonoBehaviour
{
    public static readonly Dictionary<string, UnityEvent> mapVoid = new();
    public static readonly Dictionary<string, UnityEvent<int>> mapInt = new();
    public static readonly Dictionary<string, UnityEvent<float>> mapFloat = new();

    public string key;

    public UnityEvent get;
    public UnityEvent<int> getInt;
    public UnityEvent<float> getFloat;

    private void Awake()
    {
        if(get != null && get.GetPersistentEventCount() > 0)
        {
            if (!mapVoid.ContainsKey(key))
                mapVoid[key] = new();
            mapVoid[key].AddListener(Get);
        }
        if (getInt != null && getInt.GetPersistentEventCount() > 0)
        {
            if (!mapInt.ContainsKey(key))
                mapInt[key] = new();
            mapInt[key].AddListener(GetInt);
        }
        if (getFloat != null && getFloat.GetPersistentEventCount() > 0)
        {
            if (!mapFloat.ContainsKey(key))
                mapFloat[key] = new();
            mapFloat[key].AddListener(GetFloat);
        }
    }

    public void Get()
    {
        get?.Invoke();
    }
    public void GetInt(int value)
    {
        getInt?.Invoke(value);
    }
    public void GetFloat(float value)
    {
        getFloat?.Invoke(value);
    }
}
