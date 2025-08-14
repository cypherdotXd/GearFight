using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SlotsPlacer
{
    public event Action<bool, Transform> OnDrop;
    private Func<Transform, Draggable, bool> TerminateDrop;
    
    private readonly HashSet<Transform> slots;
    private float threshold;

    public SlotsPlacer(HashSet<Transform> slots, float threshold, Func<Transform, Draggable, bool> terminateDrop)
    {
        this.slots = slots;
        this.threshold = threshold;
        TerminateDrop = terminateDrop;
    }

    public void AddSlot(Transform slot)
    {
        slots.Add(slot);
    }

    public bool NotifyDrop(Draggable draggable)
    {
        foreach (var slot in slots)
        {
            if (!slot.gameObject.activeInHierarchy) return false;
            var distance = Vector3.Distance(slot.position, draggable.transform.position);
            if (distance > threshold) continue;
            if (slot == draggable.transform.parent) return false;
            var doTerminate = TerminateDrop?.Invoke(slot, draggable);
            if (doTerminate != null && doTerminate.Value)
                return false;
            draggable.transform.SetParent(slot);
            draggable.transform.DOMove(slot.position, 0.2f);
            OnDrop?.Invoke(true, draggable.transform);
            return true;
        }

        OnDrop?.Invoke(false, draggable.transform);
        return false;
    }
}
