using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static CKF_UnityEvent.MapSequence;

public class CKF_UnityEvent : MonoBehaviour
{
    public List<MapUnityEvent> events;
    public List<MapSequence> sequences;
    public Dictionary<string, UnityEvent> mapUnityEvent;
    public Dictionary<string, List<MapMapEvent>> mapSequence;
    [Serializable]
    public struct MapUnityEvent {
        public string key;
        public UnityEvent Event;
    }
    [Serializable]
    public struct MapSequence
    {
        public string key;
        [SerializeField]
        public List<MapMapEvent> events;
        [Serializable]
        public struct MapMapEvent {
            public string key;
            public float waitBefore;
        }
    }
    private bool notAwake = true;
    private List<string> deferredCallEvent = new(), defferedCallSequence = new();
    public void Awake()
    {
        mapUnityEvent = events.ToDictionary(e => e.key, e => e.Event);
        mapSequence= sequences.ToDictionary(e => e.key, e => e.events);
        notAwake = false;
        foreach (string key in deferredCallEvent)
            { CallEvent(key); Debug.Log("deferredCallEvent: " + key); }
        foreach (string key in defferedCallSequence)
            CallSequence(key);
    }

    public void CallEvent(string key)
    {
        if (notAwake) { deferredCallEvent.Add(key); return; }
        if (mapUnityEvent.ContainsKey(key)) mapUnityEvent[key].Invoke();
    }

    public void CallSequence(string key)
    {
        if (notAwake) { defferedCallSequence.Add(key); return; }
        if (mapSequence.ContainsKey(key)) StartCoroutine(UnityEventSequence(mapSequence[key]));
    }

    private readonly List<Fn.SingleSerialised<float>> pauseTimes = new();
    private IEnumerator UnityEventSequence(List<MapMapEvent> sequence)
    {
        Fn.SingleSerialised<float> pauseTime = new(0);
        pauseTimes.Add(pauseTime);

        foreach (var e in sequence)
        {
            if (e.waitBefore > 0)
                yield return new WaitForSeconds(e.waitBefore);
            while (pauseTime.data > 0)
            {
                float temp = pauseTime.data;
                pauseTime.data = 0;
                yield return new WaitForSeconds(temp);
            }
            CallEvent(e.key);
        }
        pauseTimes.Remove(pauseTime);
        yield return null;
    }

    private bool isPaused = false;
    public void SetPause(bool pause)
    {
        isPaused = pause;
    }
    public void Update()
    {
        if (isPaused)
        {
            foreach (var e in pauseTimes)
                e.data += Time.deltaTime;
        }
    }

    public void StopAllAndCallSequence(string key)
    {
        StopAllCoroutines();
        CallSequence(key);
    }
}
