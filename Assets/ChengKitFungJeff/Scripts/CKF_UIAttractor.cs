using UnityEngine;

public class CKF_UIAttractor : MonoBehaviour
{
    [GetSelfField] public RectTransform selfRect;

    public float closeStrength = 1, farStrength = 0.1f, acceleration;
    public Vector2 GetAcceleration(CKF_UIPhysics target)
    {
        if (target.useAnchor)
        {
            float dist = Vector2.Distance(selfRect.anchorMin, target.GetAnchor());
            return acceleration * (closeStrength / (dist * dist) + farStrength * dist) * (selfRect.anchorMin - target.GetAnchor());
        }
        return default;
    }
}
