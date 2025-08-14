using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gears/Nubmer Gear")]
public class GearRotatable : Rotatable
{
    public bool isMultiplier;
    public float Value = 1;
    public event Action<GearRotatable> Merged;
    
    public void Merge(GearRotatable other)
    {
        Value += other.Value;
        Merged?.Invoke(other);
    }

    public float AccumulateValue(float accumulatedValue)
    {
        return isMultiplier ? (accumulatedValue == 0 ? 1 : accumulatedValue) * Value : accumulatedValue + Value;
    }
}
