using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Gears/Default")]
public class Rotatable : ScriptableObject
{
    public event Action<int> StepChanged;
    public int Steps
    {
        get => steps;
        private set => steps = Nfmod(value, 16);
    }

    public Vector2Int Position
    {
        get;
        private set;
    } = -Vector2Int.one;

    
    private bool rotateClockWise;
    private int Direction => rotateClockWise ? -1 : 1;
    private int steps;

    public void RotateSteps(int stepsCount)
    {
        Steps += stepsCount * Direction;
        StepChanged?.Invoke(steps);
    }

    private static int Nfmod(int a, int b)
    {
        return (a % b + b) % b;
    }

    public void SetPosition(Vector2Int position)
    {
        Position = position;
        var even = (position.x % 2 == 0) ^ (position.y % 2 == 0);
        if ((even && Steps % 2 == 0) || (!even && Steps % 2 != 0)) 
            Steps += even ? -1 : 1;
        rotateClockWise = even;
    }
}
