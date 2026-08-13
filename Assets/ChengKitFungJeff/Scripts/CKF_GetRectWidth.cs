using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_GetRectWidth : MonoBehaviour
{
    public RefRectTarget initRectTarget;
    public RectTransform refRect;
    private float setted = float.NaN, settedRatio = float.NaN;
    [Min(0)] public float ratio = 1;
    public bool ignoreRefScale = true;
    [ReadonlyField] public float value;

    public UnityEvent<float> getValue;

    public enum RefRectTarget { custom, parent, self }

    private void Awake()
    {
        UpdateTarget();
    }

    public void UpdateTarget()
    {
        if (initRectTarget == RefRectTarget.parent) refRect = transform.parent as RectTransform;
        else if (initRectTarget == RefRectTarget.self) refRect = transform as RectTransform;
    }
    private void Update()
    {
        if (refRect != null || setted != refRect.rect.width || (!ignoreRefScale && settedRatio != refRect.localScale.x))
        {
            setted = refRect.rect.width;
            settedRatio = refRect.localScale.x;
            Apply();
        }
    }
    public void SetRatio(float value)
    {
        if (ratio == value) return;
        ratio = value;
        Apply();
    }

    public void SetIgnoreRefScale(bool value)
    {
        if (ignoreRefScale == value) return;
        ignoreRefScale = value;
        Apply();
    }

    public void Apply()
    {

        if (ignoreRefScale)
            value = refRect.rect.width * ratio;
        else
            value = refRect.rect.width * ratio * refRect.localScale.x;

        getValue?.Invoke(value);
    }
}
