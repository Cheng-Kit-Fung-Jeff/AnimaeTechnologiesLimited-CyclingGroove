using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_ErrorGenerator : MonoBehaviour
{
    [SerializeField] bool div0 = false, customError = false;
    [SerializeField] string customMessage = "Custom Error";

    // Update is called once per frame
    void Update()
    {
        if (div0) { div0 = false ; int zero = 0; zero = zero / zero; }
        if (customError) { customError = false; throw new Exception(customMessage); }
    }
}