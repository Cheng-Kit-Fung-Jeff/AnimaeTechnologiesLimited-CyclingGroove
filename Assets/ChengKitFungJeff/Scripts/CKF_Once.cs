using UnityEngine;
using UnityEngine.Events;

public class CKF_Once : MonoBehaviour
{
    [ReadonlyField] public bool called = false;
    public UnityEvent Event = new();

    public void Call()
    {
        if (called) return;
        called = true;
        Event?.Invoke();
    }

    public void SetCalled(bool value) { called = value; }
}
