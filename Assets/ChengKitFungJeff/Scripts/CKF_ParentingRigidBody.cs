using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class ParentingRigidBody : MonoBehaviour
{
    private Rigidbody thisRB;
    private Transform target = null;
    private Vector3 prePosition;
    private Quaternion preRotation;

    public UnityEvent awake;

    private void Awake()
    {
        thisRB = GetComponent<Rigidbody>();
        awake.Invoke();
    }

    void FixedUpdate()
    {
        if(target)
        {
            thisRB.Move(thisRB.position + target.position - prePosition, target.rotation * Quaternion.Inverse(preRotation) * thisRB.rotation);
            prePosition = target.position;
            preRotation = target.rotation;
        }
    }

    public void SetParent(Transform parent) {
        if (target != parent)
        {
            target = parent;
            if (target != null)
            {
                prePosition = target.position;
                preRotation = target.rotation;
            }
        }
    }

    
}
