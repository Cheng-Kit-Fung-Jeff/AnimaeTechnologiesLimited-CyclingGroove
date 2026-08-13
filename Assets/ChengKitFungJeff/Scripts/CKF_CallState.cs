using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CKF_CallState : MonoBehaviour
{
    public string currentState;

    [System.Serializable]
    public class Profile
    {
        public string key;
        public UnityEvent Event;
    }
    public List<Profile> events = new();

    private readonly Dictionary<string, UnityEvent> mapEvents = new();

    private bool awake = false;
    private readonly List<string> deferredCall = new();
    private void Awake()
    {
        awake = true;
        foreach (var e in events)
        {
            mapEvents[e.key] = e.Event;
        }
        foreach (string k in deferredCall)
        {
            currentState = k;
            CallEvent(k);
        }
    }
    public void CallEvent() // should change to CallState
    {
        CallEvent(currentState);
    }
    public void CallEvent(string key) // should change to CallState
    {
        if (!awake)
        {
            deferredCall.Add(key);
            return;
        }
        currentState = key;
        if(mapEvents.ContainsKey(currentState))
            mapEvents[currentState]?.Invoke();
    }

    public void SetState(string value) { currentState = value; }
}
