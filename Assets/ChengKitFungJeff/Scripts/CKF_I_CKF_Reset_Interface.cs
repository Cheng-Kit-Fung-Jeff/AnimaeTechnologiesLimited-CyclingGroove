using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_I_CKF_Reset_Interface : MonoBehaviour
{
    public static void SceneReset()
    {
        foreach (var v in GameObject.FindObjectsOfType(typeof(I_CKF_Reset)))
        {
            ((I_CKF_Reset)v).SceneReset();
        }
    }
}
