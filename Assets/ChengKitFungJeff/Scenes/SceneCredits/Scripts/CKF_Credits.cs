using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CKF_Credits : MonoBehaviour
{
    [GetSelfField] public TextMeshProUGUI text;
    public string file;

    private readonly List<string> buffer = new();

    public float duration;
    public CKF_Timer timer;
    public CKF_TimeLerp reveal;
    public UnityEvent atEnd;
    private void Awake()
    {
        string credits = Rw.Read(file, out string err);
        if (err != null)
        {
            buffer.Add(err);
        }
        else if (credits != string.Empty)
        {
            buffer.AddRange(credits.Split('\n'));
            buffer.Reverse();
        }
    }
    public void SetDuration(float value) { duration = value; }
    public void AddCredits(string value) { buffer.Add(value); }
    public void NextLine()
    {
        if (buffer.Count > 0)
        {
            string line = buffer[^1];
            buffer.RemoveAt(buffer.Count - 1);
            text.text = line;
            timer.setTimer(duration);
            reveal.SetTarget(1);
        }
        else
        {
            atEnd?.Invoke();
        }
    }
}
