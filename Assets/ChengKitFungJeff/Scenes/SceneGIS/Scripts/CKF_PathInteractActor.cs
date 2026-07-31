using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CKF_PathInteractActor : MonoBehaviour, I_CKF_Reset
{   // only execute once
    public bool interactionEnabled = true; // Removed ReaadonlyField for quicker setting
    public UnityEvent eventInteract = new();
    public List<CKF_Path> interactPaths = new();

    public float distance;

#if UNITY_EDITOR
    [Header("Editor interface")]
    [SerializeField] private CKF_PathController targetController;
#endif
    public void Start()
    {
        UpdatePaths();
    }

    public void UpdatePaths()
    {
        foreach (var p in interactPaths)
            UpdatePath(p);
    }

    public void UpdatePath(CKF_Path p)
    {
        p.AddInteractActor(this);
    }

    public void Interact()
    {
        if (!interactionEnabled) return;
        interactionEnabled = false;
        eventInteract?.Invoke();
    }

    public void SetInteractionEnable(bool value) { interactionEnabled = value; }

    public void SceneReset()
    {
        interactionEnabled = true;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);

        foreach (var p in interactPaths)
        {
            if (p.nodeA != null && p.nodeB != null && p.nodeA != p.nodeB)
            {
                Fn.SphereIntersectLine(p.nodeA.position, p.nodeB.position, transform.position, distance, out float lerpA, out float lerpB);
                if (0 <= lerpA && lerpA <= 1)
                {
                    Gizmos.DrawWireSphere(Vector3.Lerp(p.nodeA.position, p.nodeB.position,lerpA), distance * 0.2f);
                }
                if (0 <= lerpB && lerpB <= 1)
                {
                    Gizmos.DrawWireSphere(Vector3.Lerp(p.nodeB.position, p.nodeA.position, lerpB), distance * 0.2f);
                }
            }
        }
    }
}
