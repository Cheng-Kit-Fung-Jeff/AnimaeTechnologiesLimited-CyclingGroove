using UnityEngine;
using UnityEngine.Events;

public class CKF_MeasureScaler : MonoBehaviour
{
    public float RealDistance;
    [ReadonlyField] public float measuredDistance, measureToReal, realToMeasure;
    public Transform nodeA, nodeB;
    private Vector3 settedA = Vector3.negativeInfinity, settedB = Vector3.negativeInfinity;
    public UnityEvent<float> getMeasure = new(), getReal = new();

    public void Measure()
    {
        if (settedA == nodeA.position && settedB == nodeB.position) return;
        settedA = nodeA.position;
        settedB = nodeB.position;
        measuredDistance = Vector3.Distance(settedA, settedB);
        realToMeasure = measuredDistance / RealDistance;
        measureToReal = RealDistance / measuredDistance;
    }

    public void GetMeasure(float distance)
    {
        Measure();
        getMeasure?.Invoke(distance * realToMeasure);
    }
    public void GetReal(float distance)
    {
        Measure();
        getMeasure?.Invoke(distance * measureToReal);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (nodeA != null)
            Gizmos.DrawWireSphere(nodeA.position, 0.02f);
        if (nodeB != null)
            Gizmos.DrawWireSphere(nodeB.position, 0.02f);
        if (nodeA != null && nodeB != null)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawLine(nodeA.position, nodeB.position);
        }
    }
}
