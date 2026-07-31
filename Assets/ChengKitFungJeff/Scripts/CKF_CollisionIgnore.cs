using System.Collections.Generic;
using UnityEngine;

public class CKF_CollisionIgnore : MonoBehaviour
{
    public List<Collider> targetColliders;
    public bool addChildren = false;
    private void OnEnable()
    {
        for (int i = 1; i < targetColliders.Count; i++)
            for (int j = 0; j < i; j++)
                Physics.IgnoreCollision(targetColliders[i], targetColliders[j], true);
    
    }
    private void OnDisable()
    {
        for (int i = 1; i < targetColliders.Count; i++)
            for (int j = 0; j < i; j++)
                Physics.IgnoreCollision(targetColliders[i], targetColliders[j], false);
    }

    public void AddCollider(Collider target) {
        if (enabled)
            foreach (Collider collider in targetColliders)
                if (target.GetInstanceID() == collider.GetInstanceID()) return;
                else Physics.IgnoreCollision(target, collider, true);
        targetColliders.Add(target);
    }
    public void RemoveCollider(Collider target) {
        for (int i = targetColliders.Count; i > 0;) {
            --i;
            if (target.GetInstanceID() == targetColliders[i].GetInstanceID())
            { 
                targetColliders.RemoveAt(i);
                if (enabled)
                    foreach (Collider other in targetColliders)
                        Physics.IgnoreCollision(target, other, false);
            }
        }
    }
}
