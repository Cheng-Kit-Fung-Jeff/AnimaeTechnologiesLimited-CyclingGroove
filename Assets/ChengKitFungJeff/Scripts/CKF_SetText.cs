using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CKF_SetText : MonoBehaviour
{
    [GetSelfField] public TextMeshProUGUI text;
    [Min(0)]public int floatDecimal = 1;
    [System.Serializable] public class Unit
    {
        public string name;
        public float value;
        [Min(0)]public float range;
        [ReadonlyField] public float lower;
    }
    public List<Unit> units = new();

    public UnityEvent<string> getValue = new();

    private void Awake()
    {
        float inc = 0;
        for (int i = 0; i < units.Count; ++i)
        {
            units[i].lower = i == 0 ? float.NegativeInfinity : inc;
            inc += units[i].range;
            //units[i].upper = i == (units.Count - 1) ? float.PositiveInfinity : inc;
        }
        
    }

    public void SetInt(int value)
    {
        SetFloat(value);
    }
    public void SetFloat(float value)
    {
        string u = string.Empty;

        if (units.Count != 0)
        {
            GetUnit(Mathf.Abs(value), out u, out float v);
            value /= v;
        }

        string nextText = value.ToString();
        int index = nextText.IndexOf(".");
        if (index != -1)
        {
            nextText = nextText[..index] + (floatDecimal == 0? "" : nextText.Substring(index,Mathf.Min(floatDecimal + 1, nextText.Length - index - 1)));
        }
        nextText += u;
        SetText(nextText);
    }
    private void GetUnit(float value, out string u, out float v)
    {
        int check = 0;
        v = float.NaN;
        u = string.Empty;
        while (check < units.Count && value > units[check].lower)
        {
            v = units[check].value;
            u = units[check].name;
            check++;
        }
    }
    public void SetText(string t)
    {
        text.text = t;
        getValue?.Invoke(t);
    }
}
