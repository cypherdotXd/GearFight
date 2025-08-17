using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearsGridSimulator
{
    public readonly Rotatable[,] Grid;
    public event Action<Vector2Int, Rotatable, int> OnRotatableStep;

    private Vector2Int _gridSize;

    public GearsGridSimulator(Vector2Int gridSize, List<Rotatable> grid)
    {
        _gridSize = gridSize;
        Grid = new Rotatable[gridSize.x, gridSize.y];
        foreach (var rotatable in grid)
        {
            var p = rotatable.Position;
            Grid[p.y, p.x] = rotatable;
        }
    }

    public void Traverse(Rotatable source, Action<Vector2Int, Rotatable> onVisit, HashSet<Vector2Int> visited)
    {
        var p = source.Position;
        if (visited.Contains(p))
            return;
        onVisit?.Invoke(p, source);
        visited.Add(p);
        
        var neighbors = GetNeighbors(p.x, p.y);
        foreach (var (_, rotatable) in neighbors)
        {
            Traverse(rotatable, onVisit, visited);
        }
    }

    public void UpdateGrid(Rotatable rotatable, Vector2Int? oldPosition = null)
    {
        if (oldPosition != null)
        {
            var old = oldPosition.Value;
            Grid[old.y, old.x] = null;
        }
        var newPosition = rotatable.Position;
        Grid[newPosition.y, newPosition.x] = rotatable;
    }
    
    public IEnumerator MoveStep(float stepDuration = 0.1f)
    {
        for (var i = 0; i < _gridSize.x; i++)
        {
            for (var j = 0; j < _gridSize.y; j++)
            {
                var rotatable = Grid[i, j];
                if (rotatable is not MotorRotatable motor) continue;
                var simulated = new HashSet<Vector2Int>();
                yield return Simulate(motor, stepDuration, simulated);
            }
        }
    }

    private IEnumerator Simulate(Rotatable rotatable, float duration, HashSet<Vector2Int> simulated)
    {
        var source = rotatable.Position;
        if (simulated.Contains(source))
            yield break;
        rotatable.RotateSteps(1);
        OnRotatableStep?.Invoke(source, rotatable, rotatable.Steps);
        simulated.Add(source);
        
        var neighbors = GetNeighbors(source.x, source.y);
        foreach (var (p, neighbor) in neighbors)
        {
            var gearContact = rotatable is not MotorRotatable;
            var motorContact = (rotatable.Steps % 4 == 0 || rotatable.Steps % 4 == 3) &&
            MakesContact(rotatable.Steps, source, p);
            var contact = gearContact || motorContact;
            if(!contact) continue;
            yield return Simulate(neighbor, duration * 0.5f, simulated);
        }
        yield return new WaitForSeconds(duration);
    }

    private bool MakesContact(int steps, Vector2Int source, Vector2Int target)
    {
        var dir = source - target;
        dir.y *= -1;
        if (dir.x > 1 || dir.y > 1) return false;
        var angle = steps * 22.5f * Mathf.Deg2Rad;
        var forward = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
        var dAngle = Vector2.Angle(dir, forward);
        return 180 - dAngle < 23;
    }
    
    private Dictionary<Vector2Int, Rotatable> GetNeighbors(int x, int y)
    {
        List<Vector2Int> neighbors = new() { Vector2Int.left, Vector2Int.up, Vector2Int.right, Vector2Int.down };
        Dictionary<Vector2Int, Rotatable> res = new();
        
        foreach (var n in neighbors)
        {
            var dx = n.x;
            var dy = n.y;
            var newX = x + dx;
            var newY = y + dy;
        
            if (newX >= 0 && newX < _gridSize.y && newY >= 0 && newY < _gridSize.x && Grid[newY, newX] != null) 
                res.Add(new Vector2Int(newX, newY), Grid[newY, newX]);
        }
        
        return res;
    }
    
}
