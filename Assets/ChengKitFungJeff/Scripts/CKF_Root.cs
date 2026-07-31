using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(-10000)]
public class CKF_Root : MonoBehaviour
{
    private static CKF_Root sceneRoot = null;
    private static Action<CKF_Root> deferredCallRoot = delegate { };
    public static void CallRoot(Action<CKF_Root> act)
    {
        if (sceneRoot == null) { deferredCallRoot += act; return; }
        act(sceneRoot);
    }

    public UnityEvent onAwake = null;
    public static event Action OnUpdate = () => { }, OnFixedUpdate = () => { };
    private void Awake()
    {
        if (sceneRoot != null) { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
        sceneRoot = this;
        deferredCallRoot.Invoke(this);
        deferredCallRoot = delegate { };
        onAwake?.Invoke();
    }
    public static bool applicationFocus = false;
    private void OnApplicationFocus(bool focus)
    {
        applicationFocus = focus;
    }
    private void Update()
    {
        OnUpdate();
    }
    private void FixedUpdate()
    {
        OnFixedUpdate();
    }

    /*public void PiercingColliderAddGlobalIgnoreTag(string tag) {
        CKF_PiercingCollider.globalIgnoreTagSet.Add(tag);
    }
    public void PiercingColliderRemoveGlobalIgnoreTag(string tag)
    {
        CKF_PiercingCollider.globalIgnoreTagSet.Remove(tag);
    }*/

    public event Action<InputAction.CallbackContext>
        Jump = delegate { },
        Move = delegate { },
        Crouch = delegate { },
        Escape = delegate { },
        ActionL = delegate { },
        ActionR = delegate { },
        PickL = delegate { },
        PickR = delegate { },
        Menu = delegate { };
    public event Action<Vector2>
        MouseDelta = delegate { },
        MousePosition = delegate { };
    public void JumpContext(InputAction.CallbackContext context) => Jump.Invoke(context);
    public void MoveContext(InputAction.CallbackContext context) => Move.Invoke(context);
    public void CrouchContext(InputAction.CallbackContext context) => Crouch.Invoke(context);
    public void EscapeContext(InputAction.CallbackContext context) => Escape.Invoke(context);
    public void MouseDeltaContext(InputAction.CallbackContext context) { MouseDelta.Invoke(0.05f * context.ReadValue<Vector2>()); }
    public void MousePositionContext(InputAction.CallbackContext context) => MousePosition.Invoke(context.ReadValue<Vector2>());
    public void ActionLContext(InputAction.CallbackContext context) => ActionL.Invoke(context);
    public void ActionRContext(InputAction.CallbackContext context) => ActionR.Invoke(context);
    public void PickLContext(InputAction.CallbackContext context) => PickL.Invoke(context);
    public void PickRContext(InputAction.CallbackContext context) => PickR.Invoke(context);
    public void MenuContext(InputAction.CallbackContext context) => Menu.Invoke(context);

    private void OnDestroy()
    {
        if(sceneRoot == this)
        {
            sceneRoot = null;
            deferredCallRoot = delegate { };
        }
    }
}