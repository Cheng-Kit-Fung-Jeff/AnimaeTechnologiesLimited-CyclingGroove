using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CKF_PathTrail : MonoBehaviour
{
    [GetSelfField] public RectTransform selfRT;
    public RectTransform pathLayer;
    public GameObject trailElement;
    [Min(0)]public float duration, width, minDistance;
    [Color4Field] public Color innerColor, outerColor;
    private readonly List<CKF_PathTrailElement> trail = new();

    private RectTransform lastPathAnchor = null;

    private Vector3 settedPosition = Vector3.negativeInfinity;

    private void Update()
    {
        if (Vector3.Distance(settedPosition,transform.position) > minDistance)
        {
            settedPosition = transform.position;
            RectTransform newPathAnchor;
            if (trail.Count > 0 && trail[0].ceased)
            {
                newPathAnchor = trail[0].selfPathRectSprite.nodeA;
            }
            else
                newPathAnchor = new GameObject(name + "_PathAnchor").AddComponent<RectTransform>();
            newPathAnchor.SetParent(transform);
            newPathAnchor.localPosition = Vector3.zero;
            newPathAnchor.SetParent(pathLayer);

            if (lastPathAnchor != null)
            {
                if (trail.Count > 0 && trail[0].ceased)
                {
                    trail.Add(trail[0]);
                    trail.RemoveAt(0);
                    trail[^1].ceased = false;
                }
                else
                    trail.Add(Instantiate(trailElement, pathLayer).GetComponent<CKF_PathTrailElement>());
                trail[^1].selfPathRectSprite.nodeA = lastPathAnchor;
                trail[^1].selfPathRectSprite.nodeB = newPathAnchor;
                trail[^1].selfPathRectSprite.SetWidth(width);
                trail[^1].selfPathRectSprite.SetInnerColor(innerColor);
                trail[^1].selfPathRectSprite.SetOuterColor(outerColor);
                trail[^1].width.SetMult(width);
                trail[^1].widthLerp.setTimer(duration);
                trail[^1].eventEnable?.Invoke();
            }
            lastPathAnchor = newPathAnchor;
        }
    }
}
