using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutoScaleOnCollider : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private float _scale = 1;

    void Update()
    {
        if(!sphereCollider) return;
        transform.localScale = Vector3.one * sphereCollider.radius * 2 * _scale;
    }
}
