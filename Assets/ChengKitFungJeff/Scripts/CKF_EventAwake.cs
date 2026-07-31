using UnityEngine;
using UnityEngine.Events;

public class CKF_EventAwake : MonoBehaviour
{
    public UnityEvent Event;
    public void Awake()
    {
        Event?.Invoke();
    }
}
