using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GearSpawnable : MonoBehaviour
{
    [SerializeField] private TMP_Text thresholdText;
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private Image spriteFill;
    

    private RotatableHandler rotatableHandler;
    private float chainValue;
    private float baseThreshold => 0.21f;
    private float spawnThreshold => chainValue > 0 ? baseThreshold + chainValue : 0;
    private float stepsAccumulator;

    private void Awake()
    {
        rotatableHandler = GetComponent<RotatableHandler>();
        thresholdText.text = spawnThreshold.ToString(CultureInfo.InvariantCulture);
    }

    private void OnEnable()
    {
        Rotatable.StepChanged += Spawn;
    }

    private void OnDisable()
    {
        Rotatable.StepChanged -= Spawn;
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
    
    private void Spawn(Rotatable rotatable)
    {
        if (rotatableHandler.Rotatable != rotatable)
            return;
        stepsAccumulator += spawnThreshold;
        spriteFill.DOFillAmount(stepsAccumulator, 0.1f);
        if(stepsAccumulator < 1)
            return;
        for (int i = 0; i < Mathf.FloorToInt(stepsAccumulator); i++)
        {
            stepsAccumulator -= 1;
            var rt = (RectTransform)transform;
            var point = Camera.main.ScreenToWorldPoint(rt.position);
            point.z = 0;
            Instantiate(spawnPrefab, point, Quaternion.identity);
        }
    }

}
