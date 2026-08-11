using UnityEngine;
using UnityEngine.Events;

public class CKF_ViewCountController : MonoBehaviour
{
    public CKF_CountFloat distance, totalTimePassed;


    public float distanceFactor, timePassFactor, viewCount;

    private float settedDistance, settedTotalTimePassed;

    public UnityEvent<int> getViewCount = new();

    private void Update()
    {
        viewCount += (distance.counter - settedDistance) * distanceFactor;
        settedDistance = distance.counter;
        viewCount += (Mathf.Floor(totalTimePassed.counter) - settedTotalTimePassed) * timePassFactor;
        settedTotalTimePassed = Mathf.Floor(totalTimePassed.counter);
        getViewCount?.Invoke((int)viewCount);
    }

    public void Multiply(float value)
    {
        viewCount *= value;
    }

    public void GetViewCount()
    {
        getViewCount?.Invoke((int)viewCount);
    }
}
