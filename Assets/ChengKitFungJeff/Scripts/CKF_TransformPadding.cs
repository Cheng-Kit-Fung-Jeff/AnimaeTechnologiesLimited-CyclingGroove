using System.Drawing;
using UnityEngine;

public class CKF_TransformPadding : MonoBehaviour
{
    public Vector3 selfSize, parentSize, padding;
    public bool autoInit = true, autoUpdate, autoLateUpdate;
    private Vector3 iSelfSize;

    private void Awake()
    {
        SetSelfSize(selfSize);
        if (autoInit) TransformUpdate();
    }
    private void OnEnable()
    {
        if (autoInit) TransformUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if(autoUpdate) TransformUpdate();
    }
    private void LateUpdate()
    {
        if(autoLateUpdate) TransformUpdate();
    }
    public void TransformUpdate() {
        transform.localScale = Vector3.Scale(
            parentSize - Vector3.Scale(
                padding,
                new(1 / transform.parent.localScale.x, 1 / transform.parent.localScale.y, 1 / transform.parent.localScale.z)),
            iSelfSize
            );
    }
    public void SetSelfSize(Vector3 size)
    { selfSize = size; iSelfSize = new(1 / size.x, 1 / size.y, 1 / size.z); }
}
