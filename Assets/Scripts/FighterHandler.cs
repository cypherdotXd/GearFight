using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class FighterHandler : MonoBehaviour
{
    public UnityEvent OnDeath;
    public UnityEvent OnKill;
    [Range(0, 1)] public float agility = 10;
    [Range(1, 100)] public float attackDamage = 10;
    public float speed = 1;
    public float health = 100;
    public Rigidbody2D rb;
    public Vector2 attackRange = Vector2.one;
    public FighterHandler target;
    public bool IsIdle => !target;
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SpriteFill healthFill;
    
    private Vector2 targetVelocity;
    private Sequence attackSequence;
    private Sequence damageSequence;

    private void Start()
    {
        if(target)
            SetTarget(target);
        
        damageSequence = DOTween.Sequence().SetAutoKill(false);
        var defaultScale = transform.localScale;
        damageSequence
            .Append(transform.DOScale(0.8f * defaultScale, 0.1f))
            .Append(transform.DOScale(defaultScale, 0.1f));
    }

    private void FixedUpdate()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, groundLayer);
        if (hit.collider == null)
            return;
        rb.velocity = targetVelocity;
    }

    public void SetTarget(FighterHandler fighter)
    {
        target = fighter;
        StartCoroutine(SetTargetRoutine(target));
    }
    
    private IEnumerator SetTargetRoutine(FighterHandler other)
    {
        while (other && other.health > 0)
        {
            var d = Vector3.Distance(transform.position, other.transform.position);
            if (d > Random.Range(attackRange.x, attackRange.y))
            {
                MoveTo(other);
            }
            else
            {
                targetVelocity = Vector2.zero;
                Attack(other);
            }
            yield return new WaitForSeconds(1 - agility);
        }
    }

    private void MoveTo(FighterHandler other)
    {
        var dir = other.transform.position - transform.position;
        targetVelocity = speed * dir.normalized;
    }

    private void Attack(FighterHandler other)
    {
        if(!other)
            return;
        attackSequence?.Complete();
        attackSequence = DOTween.Sequence();
        var defaultRotation = transform.localEulerAngles;
        var dir = Mathf.Sign(other.transform.position.x - transform.position.x);
        attackSequence
            .Append(transform.DORotate(defaultRotation + new Vector3(0, 0, dir * 15f), 0.05f))
            .Append(transform.DORotate(defaultRotation + new Vector3(0, 0, -dir * 45f), 0.1f))
            .Append(transform.DORotate(defaultRotation, 0.1f));
        var dead = other.TakeDamage(this);
        if (dead)
            OnKill?.Invoke();
    }

    private bool TakeDamage(FighterHandler other)
    {
        health -= other.attackDamage;
        if (health <= 0)
        {
            OnDeath.Invoke();
            StopAllCoroutines();
            attackSequence?.Kill(true);
            damageSequence?.Kill(true);
            Destroy(gameObject);
            return true;
        }
        healthFill.SetFill(health / 100f, 0.2f);
        damageSequence.Restart();
        return false;
    }
}
