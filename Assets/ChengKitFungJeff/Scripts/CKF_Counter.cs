using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_Counter : MonoBehaviour
{
    public UnityEvent<int> getValue = new();
    public int counter = 0;

    public void GetValue()
    {
        getValue?.Invoke(counter);
    }
    public void Increment(int value)
    {
        counter += value;
        getValue?.Invoke(counter);
    }
}
