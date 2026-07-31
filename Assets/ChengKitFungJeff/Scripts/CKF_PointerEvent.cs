using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class CKF_PointerEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IDeselectHandler
{
    public bool focused = false;
    public List<GameObject> focusGameObjects = new();
    public HashSet<int> focusGameObjectIDsHashset = new();
    public UnityEvent eventFocus = new(), eventDefocus = new();
    public UnityEvent<PointerEventData> eventEnter = new(), eventExit = new(), eventDown = new(), eventUp = new(), eventClick = new();
    public UnityEvent<BaseEventData> eventDeselect = new();

    public void OnDeselect(BaseEventData eventData)
    {
        eventDeselect?.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        eventClick?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        eventDown?.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventEnter?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventExit?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       eventUp?.Invoke(eventData);
    }

    private void Awake()
    {
        foreach (var gb in focusGameObjects) { focusGameObjectIDsHashset.Add(gb.GetInstanceID()); }
    }

    private void Update()
    {
        if (focused)
        {
            if (EventSystem.current.currentSelectedGameObject == null || !focusGameObjectIDsHashset.Contains(EventSystem.current.currentSelectedGameObject.GetInstanceID()))
            {
                focused = false;
                eventDefocus?.Invoke();
            }
        }
        else
        {
            if (EventSystem.current.currentSelectedGameObject != null && focusGameObjectIDsHashset.Contains(EventSystem.current.currentSelectedGameObject.GetInstanceID()))
            {
                focused = true;
                eventFocus?.Invoke();
            }
        }
    }
}
