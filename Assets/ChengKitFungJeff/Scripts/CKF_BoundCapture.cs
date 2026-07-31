using UnityEngine;
using UnityEngine.Events;

public class CKF_BoundCapture : MonoBehaviour
{
    public Bounds bounds;
    public UnityEvent<Bounds> eventExpand = new();

    public void SetCenter(Vector3 value)
    {
        bounds.center = value;
        eventExpand?.Invoke(bounds);
    }

    public void CapturePoint(Vector3 point)
    {
        if(!bounds.Contains(point))
        {
            bounds.Encapsulate(point);
            eventExpand?.Invoke(bounds);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = new(1,0,0,0.5f);
        Gizmos.DrawCube(bounds.center, bounds.size);
    }
}
