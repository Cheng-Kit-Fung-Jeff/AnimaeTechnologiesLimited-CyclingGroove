using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CKF_SetTextSI : MonoBehaviour
{
    [GetSelfField] public TextMeshProUGUI text;
    [Min(1)] public int significant = 2;

    [System.Serializable]
    public class Unit
    {
        public string name;
        [Min(0)] public float value;
    }
    public string unit;
    public List<Unit> units = new();

    public UnityEvent<string> getValue = new();

    private void Awake()
    {
        units.Sort((a,b)=> b.value.CompareTo(a.value));
    }

    public void SetInt(int value)
    {
        SetFloat(value);
    }
    public void SetFloat(float value)
    {
        bool sign = value < 0;
        if (sign) value = -value;
        
        string u = string.Empty;

        if (units.Count != 0)
        {
            GetUnit(Mathf.Abs(value), out u, out float v);
            value /= v;
        }
        string nextText = value.ToString($"G{significant}");
        int dotIndex = nextText.IndexOf('.');
        int length = nextText.Length;
        if (dotIndex != -1) --length;
        if (length < significant)
            nextText += new string('0', significant - length);

        nextText += u + unit;
        if(sign)nextText = '-' + nextText;
        SetText(nextText);
    }
    private void GetUnit(float value, out string u, out float v)
    {
        int check = 0;
        while (check < units.Count && value < units[check].value)
        {
            check++;
        }
        if (check == units.Count) --check;
        v = units[check].value;
        u = units[check].name;
    }
    public void SetText(string t)
    {
        text.text = t;
        getValue?.Invoke(t);
    }
}
