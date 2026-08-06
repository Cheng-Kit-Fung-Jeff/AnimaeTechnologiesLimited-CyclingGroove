using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_FlipFlopLadder: MonoBehaviour
{
    public float currentValue;
    public float initValue;
    public bool jumpState = true;
    [System.Serializable] public class Profile
    {
        public float value;
        public UnityEvent eventIn, eventOut;
        [ReadonlyField] public float lower, upper;
    }

    public List<Profile> profiles;
    [ReadonlyField] public int indexState = 0;

    public void Awake()
    {
        float check = 0;
        for (int i = 0; i < profiles.Count; i++)
        {
            if (i == 0) { profiles[i].lower = float.NegativeInfinity; }
            else { profiles[i].lower = check; }
            check += profiles[i].value;
            if (i + 1 < profiles.Count) { profiles[i].upper = check; }
            else { profiles[i].upper = float.PositiveInfinity; }
            if (check < currentValue)
            {
                indexState = i;
            }
        }

        setValue(initValue);
    }

    public void setValue(int value)
    {
        setValue((float)value);
    }

    public void setValue(float value) //change name to GetValue
    {
        currentValue = value;
        if (jumpState)
        {
            if (value <= profiles[indexState].lower)
            {
                profiles[indexState--].eventOut?.Invoke();
                while (value <= profiles[indexState].lower) indexState--;
                profiles[indexState].eventIn?.Invoke();
            }
            else if (value > profiles[indexState].upper)
            {
                profiles[indexState++].eventOut?.Invoke();
                while (value >= profiles[indexState].upper) indexState++;
                profiles[indexState].eventIn?.Invoke();
            }
            return;
        }
        
        while (value <= profiles[indexState].lower)
        {
            profiles[indexState--].eventOut?.Invoke();
            profiles[indexState].eventIn?.Invoke();
        }
        while (value > profiles[indexState].upper)
        {
            profiles[indexState++].eventOut?.Invoke();
            profiles[indexState].eventIn?.Invoke();
        }
    }
}
