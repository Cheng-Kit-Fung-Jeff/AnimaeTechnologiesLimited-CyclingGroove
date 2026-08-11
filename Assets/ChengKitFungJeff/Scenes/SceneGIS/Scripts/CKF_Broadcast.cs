using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_Broadcast : MonoBehaviour
{
    public string key;
    public void Call()
    {
        if (CKF_Subscribe.mapVoid.ContainsKey(key))
        {
            CKF_Subscribe.mapVoid[key].Invoke();
        }
    }
    public void Call(int value)
    {
        if (CKF_Subscribe.mapInt.ContainsKey(key))
        {
            CKF_Subscribe.mapInt[key].Invoke(value);
        }
    }
    public void CallInt(float value)
    {
        if (CKF_Subscribe.mapInt.ContainsKey(key))
        {
            CKF_Subscribe.mapInt[key].Invoke((int)value);
        }
    }
    public void Call(float value)
    {
        if (CKF_Subscribe.mapFloat.ContainsKey(key))
        {
            CKF_Subscribe.mapFloat[key].Invoke(value);
        }
    }
    public void CallFloat(int value)
    {
        if (CKF_Subscribe.mapFloat.ContainsKey(key))
        {
            CKF_Subscribe.mapFloat[key].Invoke(value);
        }
    }
}
