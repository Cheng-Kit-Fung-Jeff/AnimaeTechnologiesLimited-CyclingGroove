using UnityEngine;
using UnityEngine.UI;

public class CKF_UISetDirty : MonoBehaviour
{
    public void SetDirty(RectTransform target) { LayoutRebuilder.MarkLayoutForRebuild(target); }
    public void SetImmediate(RectTransform target) { LayoutRebuilder.ForceRebuildLayoutImmediate(target); }
}
