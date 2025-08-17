using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Gears/Motor")]
public class MotorRotatable_SO : Rotatable_SO
{
    public override Rotatable CreateRotatable()
    {
        return new MotorRotatable();
    }
}