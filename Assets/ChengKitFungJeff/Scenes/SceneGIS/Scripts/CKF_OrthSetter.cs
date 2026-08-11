using UnityEngine;
using UnityEngine.Events;

public class CKF_OrthSetter : MonoBehaviour
{
    [Min(0)] public float size;
    public float scaleLeft,scaleRight, scaleTop, scaleBottom;
    public Vector3 positionOffset;
    [ReadonlyField] public Vector3 position;
    public UnityEvent<float> eventResize;
    public UnityEvent<Vector3> eventReposition;

    public void SetByBounds(Bounds bounds)
    {
        Vector3 boundsExtends
            = new(
                bounds.extents.x * (scaleRight + scaleLeft - 1),
                bounds.extents.y,
                bounds.extents.z * (scaleTop + scaleBottom - 1));
        size = Mathf.Max(boundsExtends.z, boundsExtends.x * Screen.height / Screen.width);
        position = bounds.center + positionOffset
            + new Vector3(
                bounds.extents.x * 0.5f * (scaleRight - scaleLeft),
                0 ,
                bounds.extents.z * 0.5f * (scaleTop - scaleBottom));
        eventResize?.Invoke(size);
        eventReposition?.Invoke(position);
    }
}
