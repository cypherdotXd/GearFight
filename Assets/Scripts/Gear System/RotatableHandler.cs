using System;
using System.Collections;
using UnityEngine;

public class RotatableHandler : MonoBehaviour
{
    
    public event Action<RotatableHandler> GearPositionChanged;
    public Transform gearTransform;
    public Rotatable Rotatable;
    
    private Draggable draggable;
    private Vector3 targetRotation;

    private void Awake()
    {
        Rotatable = Instantiate(Rotatable);
        draggable = GetComponent<Draggable>();
    }
    
    private void OnEnable()
    {
        draggable.OnDrop += OnDrop;
    }

    private void OnDisable()
    {
        draggable.OnDrop -= OnDrop;
    }

    private void Update()
    {
        gearTransform.rotation = Quaternion.Lerp(gearTransform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * 10f);
    }

    private void OnDrop(bool slotted)
    {
        if (!slotted) return;
        GearPositionChanged?.Invoke(this);
    }

    public void RotateSteps(int steps)
    {
        targetRotation = new Vector3(0, 0, steps * -22.5f);
        
    }

    public void UpdateRotatable(Vector2Int gridPosition)
    {
        Rotatable.SetPosition(gridPosition);
        RotateSteps(Rotatable.Steps);
    }
}
