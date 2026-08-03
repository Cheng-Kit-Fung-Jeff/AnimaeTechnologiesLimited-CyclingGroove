using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_IntState : MonoBehaviour
{
    public int current, init;
    public bool jumpState = true;
    [System.Serializable]
    public class Profile
    {
        public int value;
        public UnityEvent eventIn, eventOut;
        [ReadonlyField] public int lower, upper;
    }

    public List<Profile> profiles = new();

    [ReadonlyField] public int index;

    private void Awake()
    {
        int counter = 0;
        for (int i = 0; i < profiles.Count; ++i)
        {
            profiles[i].lower = i == 0 ? int.MinValue : profiles[i-1].upper;
            counter += profiles[i].value;
            profiles[i].upper = i == profiles.Count - 1 ? int.MaxValue : counter;
            if (profiles[i].lower < current && profiles[i].upper <= current) index = i;
        }
        GetValue(init);
    }

    public void GetValue(float value) { GetValue((int)value); }

    public void GetValue(int value)
    {
        current = value;
        
        if (jumpState)
        {
            if (profiles[index].lower >= current)
            {
                profiles[index].eventOut?.Invoke();
                while (profiles[--index].lower >= current);
                profiles[index].eventIn?.Invoke();
            }
            else if (profiles[index].upper < current)
            {
                profiles[index].eventOut?.Invoke();
                while (profiles[++index].upper < current);
                profiles[index].eventIn?.Invoke();
            }
        }
        else
        {
            while (profiles[index].lower >= current)
            {
                profiles[--index].eventOut?.Invoke();
                profiles[index].eventIn?.Invoke();
            }
            while (profiles[index].upper < current)
            {
                profiles[++index].eventOut?.Invoke();
                profiles[index].eventIn?.Invoke();
            }
        }
    }
}
