using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKF_TextArea : MonoBehaviour
{
    [TextArea]
    public string text;
    public string GetText() { return text; }
}
