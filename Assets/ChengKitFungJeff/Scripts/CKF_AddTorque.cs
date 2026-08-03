using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_AddTorque : MonoBehaviour
{
    public Vector3 torque;
    public Rigidbody target;

    private void Awake()
    {
        if (target == null) target = GetComponent<Rigidbody>();
    }

    public void AddInterval(float interval)
    {

        target.AddTorque(torque / interval, ForceMode.Impulse);
    }
}
