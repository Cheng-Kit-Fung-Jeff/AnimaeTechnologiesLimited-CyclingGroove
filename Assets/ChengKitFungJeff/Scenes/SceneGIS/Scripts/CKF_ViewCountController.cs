using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_ViewCountController : MonoBehaviour
{
    public CKF_SetText counter;

    public CKF_CountFloat distance, totalTimePassed;


    public float distanceFactor, timePassFactor, viewCount;

    private float settedDistance, settedTotalTimePassed;

    public UnityEvent<float> getViewCount = new();

    private void Update()
    {
        viewCount += (distance.counter - settedDistance) * distanceFactor;
        settedDistance = distance.counter;
        viewCount += (Mathf.Floor(totalTimePassed.counter) - settedTotalTimePassed) * timePassFactor;
        settedTotalTimePassed = Mathf.Floor(totalTimePassed.counter);
        counter.SetInt((int)viewCount);
    }

    public void Multiply(float value)
    {
        viewCount *= value;
    }

    public void GetViewCount()
    {
        getViewCount?.Invoke(viewCount);
    }
}
