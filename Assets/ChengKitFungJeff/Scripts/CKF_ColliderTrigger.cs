using UnityEngine;
using UnityEngine.Events;

public class CKF_ColliderTrigger : MonoBehaviour
{
    public Transform target;
    public UnityEvent eventEnter, eventExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
            eventEnter?.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == target)
            eventExit?.Invoke();
    }
}
