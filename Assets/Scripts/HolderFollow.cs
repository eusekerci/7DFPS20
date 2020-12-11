using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _headBone;
    
    [SerializeField]
    private Vector3 _offset;
    
    [SerializeField]
    private float _offsetModifier;

    void Update()
    {
        transform.position = _headBone.position + _offset;
    }

    public void UpdateOffset(Transform target)
    {
        _offset = (target.position - _headBone.position).normalized * _offsetModifier;
    }
}
