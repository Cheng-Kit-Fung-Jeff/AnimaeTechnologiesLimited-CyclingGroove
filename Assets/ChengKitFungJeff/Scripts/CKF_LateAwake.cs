using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(10000)]
public class CKF_LateAwake : MonoBehaviour
{
    public UnityEvent Event = new();
    void Awake()
    {
        Event?.Invoke();
    }
}
