using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CKF_SetByAudio : MonoBehaviour
{
    public AudioSource reference;
    private float[] spectrum = new float[8192];

    void Update()
    {
        if (reference.isPlaying)
        {
            reference.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
            float preLogIndex = Mathf.Log(0), preLogSpectrum = Mathf.Log(spectrum[0]);
            for (int i = 1; i < spectrum.Length - 1; i++)
            {
                float LogIndex = Mathf.Log(i), LogSpectrum = Mathf.Log(spectrum[i]);
                //Debug.DrawLine(new(i - 1, spectrum[i] + 10, 0), new(i, spectrum[i + 1] + 10, 0), Color.red);
                //Debug.DrawLine(new(i - 1, preLogSpectrum + 10, 2), new(i, LogSpectrum + 10, 2), Color.cyan);
                Debug.DrawLine(new(preLogIndex, spectrum[i-1] + 10, 1), new(LogIndex, spectrum[i] + 10, 1), Color.green);
                Debug.DrawLine(new(preLogIndex, preLogSpectrum + 10, 3), new (LogIndex, LogSpectrum + 10, 3), Color.blue);
                preLogIndex = LogIndex; preLogSpectrum = LogSpectrum;
            }
        }
    }
}
