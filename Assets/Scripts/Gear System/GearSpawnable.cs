using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GearSpawnable : MonoBehaviour
{
    public Rotatable GearRotatable => rotatableHandler.Rotatable;
    
    [SerializeField] private TMP_Text thresholdText;
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private Image spriteFill;
    

    private RotatableHandler rotatableHandler;
    private float chainValue;
    private float baseThreshold => 0.21f;
    private float spawnThreshold => chainValue > 0 ? baseThreshold + chainValue : baseThreshold;
    private float stepsAccumulator;

    private void Awake()
    {
        rotatableHandler = GetComponent<RotatableHandler>();
        thresholdText.text = spawnThreshold.ToString(CultureInfo.InvariantCulture);
    }

    private void OnEnable()
    {
        GearRotatable.StepChanged += Spawn;
    }

    private void OnDisable()
    {
        GearRotatable.StepChanged -= Spawn;
    }

    private void Start()
    {
        spriteFill.DOFillAmount(stepsAccumulator / spawnThreshold, 0.1f);
    }

    public void SetChainValue(float newThreshold)
    {
        chainValue = newThreshold;
        thresholdText.text = spawnThreshold.ToString("F2");
    }

    private void Spawn(int steps)
    {
        stepsAccumulator += spawnThreshold;
        spriteFill.DOFillAmount(stepsAccumulator / spawnThreshold, 0.1f);
        if(stepsAccumulator < 1)
            return;
        stepsAccumulator -= 1;
        spriteFill.DOFillAmount(0, 0.1f);
        var rt = (RectTransform)transform;
        var parentRt = (RectTransform)transform.parent;
        var point = Camera.main.ScreenToWorldPoint(rt.position);
        point.z = 0;
        Instantiate(spawnPrefab, point, Quaternion.identity);
    }

}
