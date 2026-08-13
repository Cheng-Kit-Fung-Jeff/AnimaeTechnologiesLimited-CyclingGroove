using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class CKF_OrthSetter : MonoBehaviour
{
    [Min(0)] public float size;
    public float scaleLeft,scaleRight, scaleTop, scaleBottom;
    public Vector3 positionOffset, minimum, maximum;
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
        position = bounds.center
            + new Vector3(
                bounds.extents.x * 0.5f * (scaleRight - scaleLeft),
                0 ,
                bounds.extents.z * 0.5f * (scaleTop - scaleBottom));

        /*Vector3 min = Vector3.Max(position - boundsExtends, minimum),
            max = Vector3.Min(position + boundsExtends, maximum);*/
        float hw = (float)(Screen.height) / (float)(Screen.width), wh = 1 / hw;
        size = Mathf.Max(boundsExtends.z, boundsExtends.x * hw); // cover
        //float[] deb = new float[4];
        float d = Mathf.Max(minimum.z - position.z + size, 0);
        //deb[0] = d;
        size -= 0.5f * d;
        position.z += d;
        d = Mathf.Max(position.z + size - maximum.z, 0);
        //deb[1] = d;
        size -= 0.5f * d;
        position.z -= d;
        d = Mathf.Max(minimum.x - position.x + size * wh, 0) * hw;
        //deb[2] = d;
        size -= 0.5f * d;
        position.x += d;
        d = Mathf.Max(position.x + size * wh - maximum.x, 0) * hw;
        //deb[3] = d;
        size -= 0.5f * d;
        position.x -= d;
        //Debug.Log($"{deb[0]},{deb[1]},{deb[2]},{deb[3]}");
        /*Vector3 min = new(position.x - size * Screen.width / Screen.height, 0, position.z - size),
            max = new(position.x + size * Screen.width / Screen.height, 0, position.z + size);*/
        eventResize?.Invoke(size);
        eventReposition?.Invoke(position + positionOffset);
        //eventReposition?.Invoke(0.5f * (max + min) + positionOffset);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(0.5f * (maximum + minimum), maximum - minimum);
    }
}
