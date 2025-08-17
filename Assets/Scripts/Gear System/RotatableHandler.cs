using System;
using UnityEngine;

public class RotatableHandler : MonoBehaviour
{
    public static event Action<RotatableHandler, bool> ActiveChanged; 
    public Transform gearTransform;
    public Rotatable Rotatable { get; private set; }
    
    [SerializeField] private Rotatable_SO rotatableData;
    private Vector3 targetRotation;

    private void Awake()
    {
        Rotatable = rotatableData.CreateRotatable();
    }

    private void Update()
    {
        gearTransform.rotation = Quaternion.Lerp(gearTransform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * 10f);
    }

    public void SetGearActive(bool active)
    {
        ActiveChanged?.Invoke(this, active);
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
