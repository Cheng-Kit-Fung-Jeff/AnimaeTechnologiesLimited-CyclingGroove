using UnityEngine;

public class CKF_PhysicsApplier : MonoBehaviour
{
    [SerializeField] Rigidbody thisRB;
    [SerializeField] Vector3 setPosition = Vector3.zero, setEuler = Vector3.zero, setVelocity = Vector3.zero, setAngularVelocity = Vector3.zero, setImpulse = Vector3.zero;
}
