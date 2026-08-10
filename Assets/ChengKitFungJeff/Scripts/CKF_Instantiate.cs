using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_Instantiate : MonoBehaviour
{
    public GameObject prefab;
    public Transform parent;

    public void Instantiate()
    {
        Instantiate(prefab, parent);
    }
}
