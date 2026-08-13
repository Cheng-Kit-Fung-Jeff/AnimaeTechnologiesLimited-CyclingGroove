using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_CountString : MonoBehaviour
{
    public string value = string.Empty;

    public UnityEvent<string> getValue;

    public void GetAppend(string value)
    {
        this.value += value;
        getValue?.Invoke(this.value);
    }
    public void GetPrepend(string value)
    {
        this.value = value + this.value;
        getValue?.Invoke(this.value);
    }
    public void GetAppend(int value)
    {
        this.value += value.ToString();
        getValue?.Invoke(this.value);
    }
    public void GetPrepend(int value)
    {
        this.value = value.ToString() + this.value;
        getValue?.Invoke(this.value);
    }
    public void SetAppend(string value)
    {
        this.value += value;
    }
    public void SetPrepend(string value)
    {
        this.value = value + this.value;
    }
    public void SetAppend(int value)
    {
        this.value += value.ToString();
    }
    public void SetPrepend(int value)
    {
        this.value = value.ToString() + this.value;
    }
    public void GetValue()
    {
        getValue?.Invoke(value);
    }
    public void Get(string value)
    {
        getValue?.Invoke(value);
    }
    public void GetSet(string value)
    {
        this.value = value;
        getValue?.Invoke(this.value);
    }
    public void Set(string value)
    {
        this.value = value;
    }
    public void Get(int value)
    {
        getValue?.Invoke(value.ToString());
    }
    public void GetSet(int value)
    {
        this.value = value.ToString();
        getValue?.Invoke(this.value);
    }
    public void Set(int value)
    {
        this.value = value.ToString();
    }
}
