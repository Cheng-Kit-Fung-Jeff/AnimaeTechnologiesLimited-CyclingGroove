using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CKF_CursorControl : MonoBehaviour
{
    public bool visible = true;
    public CursorLockMode lockState;
    public readonly HashSet<string> reasonDisable = new(), reasonConfined = new(), reasonVisible = new();
    private bool disableByReason = false, confinedByReason = false, visibleByReason = false;
    //AAA disableByReason > confinedByReason; disableByReason > visibleByReason

    public void OnEnable()
    {
        UpdateVisible(); UpdateLock();
    }

    public void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void CursorVisible(bool visible) {
        this.visible = visible;
        UpdateVisible();
    }
    private readonly Dictionary<string, CursorLockMode> mapCursorLockMode
        = ((CursorLockMode[])Enum.GetValues(typeof(CursorLockMode))).ToDictionary(e => e.ToString(), e => e);
    public void CursorLockState(string state) {
        lockState = mapCursorLockMode[state];
        UpdateLock();
    }
    private void OnApplicationFocus(bool focus)
    {
        CKF_Root.applicationFocus = focus;
        UpdateVisible(); UpdateLock();
    }

    private void UpdateVisible() {
        //Debug.Log(""+ !applicationFocus + disableByReason + visibleByReason + visible);
        Cursor.visible = !CKF_Root.applicationFocus || disableByReason || visibleByReason || visible;
    }
    private void UpdateLock() {
        if (CKF_Root.applicationFocus && !disableByReason)
        {
            Cursor.lockState = !confinedByReason && lockState == CursorLockMode.None ? lockState : CursorLockMode.Confined;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void Update()
    {
        if (CKF_Root.applicationFocus && !disableByReason && !confinedByReason && lockState == CursorLockMode.Locked)
        {
            Mouse.current.WarpCursorPosition(new(0.5f * Screen.width , 0.5f * Screen.height));
        }
    }

    public void AddDisableReason(string reason) {
        reasonDisable.Add(reason);
        if (reasonDisable.Count == 1)
        {
            disableByReason = true;
            UpdateVisible(); UpdateLock();
        }
    }

    public void RemoveDisableReason(string reason)
    {
        if (reasonDisable.Count == 0) return;
        reasonDisable.Remove(reason);
        if (reasonDisable.Count == 0)
        {
            disableByReason = false;
            UpdateVisible(); UpdateLock();
        }
    }
    public void AddConfinedReason(string reason)
    {
        reasonConfined.Add(reason);
        if (reasonConfined.Count == 1)
        {
            confinedByReason = true;
            UpdateLock();
        }
    }

    public void RemoveConfinedReason(string reason)
    {
        if (reasonConfined.Count == 0) return;
        reasonConfined.Remove(reason);
        if (reasonConfined.Count == 0)
        {
            confinedByReason = false;
            UpdateLock();
        }
    }
    public void AddVisibleReason(string reason)
    {
        reasonVisible.Add(reason);
        if (reasonVisible.Count == 1)
        {
            visibleByReason = true;
            UpdateVisible();
        }
    }

    public void RemoveVisibleReason(string reason)
    {
        if (reasonVisible.Count == 0) return;
        reasonVisible.Remove(reason);
        if (reasonVisible.Count == 0)
        {
            visibleByReason = false;
            UpdateVisible();
        }
    }
}