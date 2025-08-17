using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GearsShopManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> shoppables;
    [SerializeField] private Transform shoppablesParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private HorizontalLayoutGroup shoppablesLayout;
    [SerializeField] private GearsGridManager gearsGridManager;

    private void Start()
    {
        CreateShoppables(4);
    }

    public void CreateShoppables(int n = 3)
    {
        for (int i = 0; i < n; i++)
        {
            var rI = Random.Range(0, shoppables.Count);
            var slot = Instantiate(slotPrefab, shoppablesParent).transform;
            var go = Instantiate(shoppables[rI], slot);
            go.name = "slot" + i;
            gearsGridManager.RegisterGear(go);
            GearsGridManager.RegisterSlot(slot);
        }
    }
}
