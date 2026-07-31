using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class CKF_HomingRigidBody : MonoBehaviour
{
    public float acceleration = 0, pRate = 0,  dRate = 0;
    public Transform trackedObject;
    public Vector3 offset = Vector3.zero;
    [SerializeField] private bool verbose = false;
    private Fn.PD errX, errY, errZ;
    private Rigidbody selfRB;
    private void Awake()
    {
        errX = new (pRate, dRate);
        errY = new (pRate, dRate);
        errZ = new (pRate, dRate);
        selfRB = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Vector3 targetPosition = trackedObject ? trackedObject.TransformPoint(offset):offset;
        Vector3 accelerateVector = new(
        errX.Update(targetPosition.x - selfRB.position.x, Time.fixedDeltaTime),
        errY.Update(targetPosition.y - selfRB.position.y, Time.fixedDeltaTime),
        errZ.Update(targetPosition.z - selfRB.position.z, Time.fixedDeltaTime));
        selfRB.AddForce(Vector3.ClampMagnitude(accelerateVector, acceleration), ForceMode.Acceleration);
        if (verbose) Debug.Log($"{name}(HomingRigidBody) pos:{selfRB.position.ToString()} acc:{Vector3.ClampMagnitude(accelerateVector, acceleration).ToString()}");
    }

    public void SetAcceleration(float a) { acceleration = a; }

    public void SetPRate(float pRate) {
        this.pRate = pRate;
        errX.p = pRate;
        errY.p = pRate;
        errZ.p = pRate;
    }
    public void SetDRate(float dRate)
    {
        this.dRate = dRate;
        errX.d = dRate;
        errY.d = dRate;
        errZ.d = dRate;
    }
    public void SetOffset(Vector3 value) { offset = value; }
}
