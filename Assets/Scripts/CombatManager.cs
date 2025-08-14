using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private FighterHandler enemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.TryGetComponent(out FighterHandler fighter)) return;
        if (!enemy) return;
        fighter.SetTarget(enemy);
        if(enemy.IsIdle)
            enemy.SetTarget(fighter);
    }
}
