using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private FighterHandler enemy;
    
    private readonly Queue<FighterHandler> spawnQueue = new();
    private bool isTargetSet;
    
    private void OnEnable()
    {
        enemy.OnKill.AddListener(SetNextTarget);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.TryGetComponent(out FighterHandler fighter)) return;
        if (!enemy) return;
        spawnQueue.Enqueue(fighter);
        fighter.SetTarget(enemy);
        if (!isTargetSet)
            SetNextTarget();
    }

    private void SetNextTarget()
    {
        if(spawnQueue.Count < 0) return;
        isTargetSet = true;
        print($"new target");
        var fighter = spawnQueue.Dequeue();
        enemy.SetTarget(fighter);
    }
}
