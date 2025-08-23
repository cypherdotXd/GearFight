using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Transform _target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_target) return;
        RotateAroundTarget();
    }

    void RotateAroundTarget()
    {
        var offset = 2f *  new Vector3(Mathf.Sin(Time.time * speed), transform.position.y, Mathf.Cos(Time.time * speed));
        transform.position = _target.position + offset;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
