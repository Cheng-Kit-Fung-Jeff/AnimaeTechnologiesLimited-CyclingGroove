using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_SceneObjectBucketInterface : MonoBehaviour
{
    public string key;
    public List<Component> targets = new();

    private void Awake()
    {
        foreach (var target in targets)
        {
            CKF_SceneObjectBucket.instance.AddToComponents(key, target);
        }
    }
}
