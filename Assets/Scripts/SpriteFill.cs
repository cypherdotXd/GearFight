using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpriteFill : MonoBehaviour
{
    [SerializeField] private Vector2 axis;
    [SerializeField] private Transform maskSprite;

    private Vector3 fullFillScale;
    private Tween scaleTween;
    
    private void Awake()
    {
        fullFillScale = maskSprite.localScale;
    }

    private void Start()
    {
        
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(1);
        yield return SetFill(0, 2).WaitForCompletion();
        yield return new WaitForSeconds(1);
        yield return SetFill(1, 2).WaitForCompletion();
        yield return new WaitForSeconds(1);
        yield return SetFill(0.5f, 2).WaitForCompletion();
    }

    public Tween SetFill(float fill, float duration)
    {
        scaleTween?.Complete();
        var xScale = Mathf.Lerp(fullFillScale.x, fill * fullFillScale.x, axis.x);
        var yScale = Mathf.Lerp(fullFillScale.y, fill * fullFillScale.y, axis.y);
        var targetScale = new Vector3(xScale, yScale, 1);
        scaleTween = maskSprite.DOScale(targetScale, duration);
        return scaleTween;
    }
}
