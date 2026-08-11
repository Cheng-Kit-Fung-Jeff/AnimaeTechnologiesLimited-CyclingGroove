using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CKF_CommentElement : MonoBehaviour
{
    public Image icon;

    public List<CKF_RectRefHeight> rectRefHeight;
    public List<CKF_RectRefWidth> rectRefWidth;
    public List<CKF_GetRectHeight> getRectHeight;

    public CKF_IntState indexState; // base one

    public TextMeshProUGUI text;
}
