using UnityEngine;
using UnityEngine.Events;

public class CKF_PDDerivative : MonoBehaviour
{
    public float maxAccelation, pRate, dRate, target;
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
        float nextAcc = controller.Update(target, Time.deltaTime);
        nextAcc = maxAccelation >= 0 && Mathf.Abs(nextAcc) > maxAccelation ? (nextAcc > 0 ? nextAcc : -nextAcc) : nextAcc;
        float dp = Time.deltaTime * velocity;
        target -= dp;
        velocity += Time.deltaTime * nextAcc;
        getValue?.Invoke(dp);
    }

    public void AddTarget(float value) { target += value; }
}
