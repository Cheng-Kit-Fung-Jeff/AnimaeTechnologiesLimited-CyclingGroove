using UnityEngine;

public class CKF_OrientingRigidBody : MonoBehaviour
{
    public float acceleration = 3, pRate = 4, dRate = 2;
    public Transform trackedObject, parentObject;
    [QuaternionField] public Quaternion trackedQuaternion = Quaternion.identity;
    private Rigidbody selfRB;
    private void Awake()
    {
        selfRB = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Quaternion errorRotation =
            (parentObject ? parentObject.rotation : Quaternion.identity)
            * (trackedObject ? trackedObject.rotation : trackedQuaternion) // target
            * new Quaternion(-selfRB.rotation.x, -selfRB.rotation.y, -selfRB.rotation.z, selfRB.rotation.w); // measure
        if (errorRotation.w < 0) errorRotation = new(-errorRotation.x, -errorRotation.y, -errorRotation.z, -errorRotation.w);
        Vector3 axis = new(errorRotation.x, errorRotation.y, errorRotation.z);
        selfRB
            .AddTorque(
                    Vector3.ClampMagnitude(
                        pRate * Mathf.Acos(Mathf.Clamp(errorRotation.w,-1,1)) * axis.normalized
                        - dRate * selfRB.angularVelocity,
                        acceleration
                    ),
                    ForceMode.Acceleration
                );
    }

    public void GotoTracked() {
        selfRB.rotation = parentObject.rotation * (trackedObject ? trackedObject.rotation : trackedQuaternion);
        transform.rotation = selfRB.rotation;
        selfRB.angularVelocity = Vector3.zero;
    }
}