using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class CKF_MouseDrag : MonoBehaviour
{
    public MouseButton activationKey;
    public bool showGizmos;
    private bool hitted, dragged;
    private Vector3 hitPoint, referenceHitPoint, preDragPoint;
    private float dragDistance;
    private Transform hitTransform, dragTransform;
    private void Update()
    {
        
        Vector2 mousePosition2 = new(Input.mousePosition.x / Camera.main.scaledPixelWidth, Input.mousePosition.y / Camera.main.scaledPixelHeight);
        if (Input.mousePosition.x < 0 || Input.mousePosition.y < 0 || Input.mousePosition.x >= Camera.main.scaledPixelWidth || Input.mousePosition.y >= Camera.main.scaledPixelHeight) return;
        if (!EventSystem.current.IsPointerOverGameObject()
            && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            hitPoint = hit.point;
            hitTransform = hit.transform;
            hitted = true;
        }
        else
        {
            hitted = false;
        }

        if (Input.GetMouseButtonUp((int)activationKey))
        {
            dragged = false;
        }

        if (Input.GetMouseButtonDown((int)activationKey))
        {
            if (hitted)
            {
                referenceHitPoint = Camera.main.transform.InverseTransformPoint(hitPoint);
                dragDistance = referenceHitPoint.z;
                preDragPoint = hitPoint;
                dragTransform = hitTransform;
                dragged = true;
            }
        }
        if (Input.GetMouseButton((int)activationKey))
        {
            if (dragged)
            {
                Vector3 nextDragPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition+new Vector3(0,0, dragDistance));
                dragTransform.position += nextDragPoint - preDragPoint;
                preDragPoint = nextDragPoint;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos || !hitted) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitPoint, 0.1f);
    }

}
