using UnityEngine;
using UnityEngine.Events;

public class CKF_ColliderTriggerKey : MonoBehaviour
{
    public UnityEvent<int> eventEnter = new(), eventExit = new();
    public string key;
    [ReadonlyField] public int count;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("CKF_ColliderTriggerKey: "+other.name);
        if (CKF_SceneObjectBucket.instance.ContainsInComponents(key, other))
            eventEnter?.Invoke(++count);
    }

    private void OnTriggerExit(Collider other)
    {
        if (CKF_SceneObjectBucket.instance.ContainsInComponents(key, other))
            eventEnter?.Invoke(--count);
    }
}
