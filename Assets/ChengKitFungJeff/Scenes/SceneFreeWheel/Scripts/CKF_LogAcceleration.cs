using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_LogAcceleration : MonoBehaviour
{
    private readonly List<float> intervalBuffer = new(3);

    public UnityEvent<float> outAcceleration;

    public void AddInterval(float value)
    {
        if(intervalBuffer.Count == 3)
        {
            intervalBuffer.RemoveAt(0);
        }
        intervalBuffer.Add(value);
        if (intervalBuffer.Count == 3)
            EstimateAcceleration();
    }

    private void EstimateAcceleration()
    {
        float
            b1 = (1 / intervalBuffer[1] - 1 / intervalBuffer[0]) / (intervalBuffer[1] + intervalBuffer[0]),
            b2 = (1 / intervalBuffer[2] - 1 / intervalBuffer[1]) / (intervalBuffer[2] + intervalBuffer[1]);
        outAcceleration?.Invoke(2 * b1 + 2 * (b2 - b1) / (intervalBuffer[2] + intervalBuffer[1] + intervalBuffer[0]));
    }
}
