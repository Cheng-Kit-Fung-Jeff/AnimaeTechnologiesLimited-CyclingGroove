using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CKF_RectTransform))]
public class CKF_UIPhysics : MonoBehaviour
{
    [GetSelfField] public CKF_RectTransform selfRect;
    public bool useAnchor = true, lookFoward = true;
    public float lookOffset; // Vector.zero would not change rotation

    public Vector2 velocity;
    public float maxVelocity;

    [ReadonlyField] public Vector2 acceleration;
    public Vector2 gravity;
    public List<CKF_UIAttractor> attractors;

    private void Awake()
    {
        selfRect = GetComponent<CKF_RectTransform>();
    }

    public Vector2 GetAnchor() { return selfRect.GetAnchorMin(); }

    private void Update()
    {

        selfRect.SetAnchorMinMaxX(selfRect.GetAnchorMin().x + velocity.x * Time.deltaTime);
        selfRect.SetAnchorMinMaxY(selfRect.GetAnchorMin().y + velocity.y * Time.deltaTime);

        if (lookFoward && (velocity.x != 0 || velocity.y != 0))
        {
            Quaternion rotation = Quaternion.LookRotation(velocity, Vector3.forward);
            selfRect.SetLocalRotation(Quaternion.Euler(0,0, (rotation.eulerAngles.z < 180 ? 180 - rotation.eulerAngles.x : rotation.eulerAngles.x) + lookOffset));
        }

        velocity += (acceleration + gravity) * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, maxVelocity);
        acceleration.Set(0, 0);
        foreach (var a in attractors) { acceleration += a.GetAcceleration(this); }
    }
}
