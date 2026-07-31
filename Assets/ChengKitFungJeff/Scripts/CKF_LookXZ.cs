using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_LookXZ : MonoBehaviour
{
    public Transform target;
    public float offset;

    private void Awake()
    {
        Apply();
    }

    public void Apply()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            offset +Mathf.Atan2(target.position.x - transform.position.x, target.position.z - transform.position.z) * Mathf.Rad2Deg,
            transform.rotation.eulerAngles.z);
    }
}
