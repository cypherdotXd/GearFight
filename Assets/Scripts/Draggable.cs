using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable :MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event Action OnPickUp;
    public event Action<bool> OnDrop;
    
    private SlotsPlacer slotsPlacer;
    private Vector3 startPosition;

    public void SetSlotsPlacer(SlotsPlacer value)
    {
        slotsPlacer = value;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        OnPickUp?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // World
        // if (Camera.main == null) return;
        // var p = Camera.main.ScreenToWorldPoint(eventData.position);
        // transform.position = new Vector2(p.x, p.y);

        // UI
        var rt = (RectTransform)transform;
        var parentRt = (RectTransform)rt.parent;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRt, eventData.position, null, out var point);
        rt.anchoredPosition = point;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var valid = slotsPlacer.NotifyDrop(this);
        OnDrop?.Invoke(valid);
        if (valid)
            return;
        transform.DOMove(startPosition, 0.2f);
    }
}
