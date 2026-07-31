using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CKF_SetText : MonoBehaviour
{
    [GetSelfField] public TextMeshProUGUI text;
    [Min(0)]public int floatDecimal = 1;
    public void SetInt(int value)
    {
        text.text = value.ToString();
    }
    public void SetFloat(float value)
    {
        text.text = value.ToString();
        int index = text.text.IndexOf(".");
        if (index != -1)
        {
            text.text = text.text[..index] + (floatDecimal == 0? "" : text.text.Substring(index,floatDecimal+1));
        }
    }
}
