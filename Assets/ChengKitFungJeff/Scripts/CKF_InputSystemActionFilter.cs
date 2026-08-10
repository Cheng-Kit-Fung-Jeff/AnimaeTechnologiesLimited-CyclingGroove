using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CKF_InputSystemActionFilter : MonoBehaviour
{
    public bool started, performed, canceled;

    public UnityEvent action;

    public void Action(InputAction.CallbackContext context)
    {
        if (started && context.started)
        {
            action?.Invoke();
        }
        if (performed && context.performed)
        {
            action?.Invoke();
        }
        if (canceled && context.canceled)
        {
            action?.Invoke();
        }
    }
}
