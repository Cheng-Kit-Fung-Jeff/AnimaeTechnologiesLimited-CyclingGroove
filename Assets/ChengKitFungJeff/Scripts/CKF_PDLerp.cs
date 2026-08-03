using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_PDLerp : MonoBehaviour
{
    public float maxAccelation, pRate, dRate, target, current;
    [ReadonlyField] public float velocity;
    public UnityEvent<float> getValue = new();
#if UNITY_EDITOR
    [SerializeField] private bool active = false;
#endif
    private Fn.PD controller;

    public void Awake()
    {
        controller = new(pRate, dRate);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (active)
        {
            controller.p = pRate;
            controller.d = dRate;
        }
#endif
        float nextAcc = controller.Update(target - current, Time.deltaTime);
        nextAcc = Mathf.Abs(nextAcc) > maxAccelation ? Mathf.Sign(nextAcc) * maxAccelation : nextAcc;
        current += Time.deltaTime * velocity;
        velocity += Time.deltaTime * nextAcc;
        getValue?.Invoke(current);
    }

    public void SetTarget(float value) { target = value; }
    public void SetCurrent(float value) { current = value; }
}
