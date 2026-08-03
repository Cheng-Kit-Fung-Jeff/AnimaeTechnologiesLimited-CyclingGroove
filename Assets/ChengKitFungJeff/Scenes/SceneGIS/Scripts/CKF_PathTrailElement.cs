using UnityEngine;
using UnityEngine.Events;

public class CKF_PathTrailElement : MonoBehaviour
{
    [GetSelfField] public CKF_PathRectSprite selfPathRectSprite;
    public CKF_Timer widthLerp;
    public CKF_MAD width;

    public bool ceased = false;

    public UnityEvent eventEnable;

    public void SetCeased(bool value) { ceased = value; }
}
