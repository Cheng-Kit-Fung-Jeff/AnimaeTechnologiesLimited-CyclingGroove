using UnityEngine;
using UnityEngine.UI;

public class CKF_AttractionIconElement : MonoBehaviour
{
    [GetSelfField] public CKF_RectRefHeight refHeight;
    public CKF_TimeLerp positionX, size;
    public CKF_Timer reveal;
    public Image icon;
}
