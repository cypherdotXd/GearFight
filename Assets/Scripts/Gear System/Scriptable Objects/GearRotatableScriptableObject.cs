using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gears/Nubmered Gear")]
public class GearRotatable_SO : Rotatable_SO
{
    public bool isMultiplier;
    public float value;

    public override Rotatable CreateRotatable()
    {
        return new GearRotatable(this);
    }
}
