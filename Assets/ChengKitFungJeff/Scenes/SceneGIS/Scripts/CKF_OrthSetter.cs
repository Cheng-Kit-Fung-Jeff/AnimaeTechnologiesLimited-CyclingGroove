using UnityEngine;
using UnityEngine.Events;

public class CKF_OrthSetter : MonoBehaviour
{
    [Min(0)] public float size;
    public Vector3 margin, positionOffset;
    [ReadonlyField] public Vector3 position;
    public UnityEvent<float> eventResize;
    public UnityEvent<Vector3> eventReposition;

    public void SetByBounds(Bounds bounds)
    {
        Vector3 boundsExtends = bounds.extents + margin;
        size = Mathf.Max(boundsExtends.z, boundsExtends.x * Screen.height / Screen.width);
        position = bounds.center + positionOffset;
        eventResize?.Invoke(size);
        eventReposition?.Invoke(position);
    }
}
