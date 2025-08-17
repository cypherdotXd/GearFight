using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GearRotatable : Rotatable
{
    public bool IsMultiplier {get; }
    public float Value { get; private set; }
    public static event Action<GearRotatable, GearRotatable> Merged;

    public GearRotatable(GearRotatable_SO data)
    {
        IsMultiplier = data.isMultiplier;
        Value = data.value;
    }
    
    public void Merge(GearRotatable other)
    {
        Value += other.Value;
        Merged?.Invoke(this, other);
    }

    public float AccumulateValue(float accumulatedValue)
    {
        return IsMultiplier ? (accumulatedValue == 0 ? 1 : accumulatedValue) * Value : accumulatedValue + Value;
    }
}
