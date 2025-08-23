using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Rotator rotator;
    [SerializeField] private List<Transform> rotateAroundTargets;

    private Transform root;

    private void Start()
    {
        // rotator.SetTarget(rotateAroundTargets[0]);
        StartCoroutine(RotateSequence());
    }

    public void CountE()
    {
        var names = "";
        var children = root.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            names += child.name;
        }
        
        int count = 0;
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i] == 'e' || names[i] == 'E')
                count++;
        }
        print($"E count {count}");
    }

    public IEnumerator RotateSequence()
    {
        var i = 0;
        print("start revolution");
        rotator.SetTarget(rotateAroundTargets[i]);
        yield return new WaitForSeconds(2);
        print("new target");
        i++;
        rotator.SetTarget(rotateAroundTargets[i]);
        yield return new WaitForSeconds(3);
        print("new target");
        i++;
        rotator.SetTarget(rotateAroundTargets[i]);

    }
}
