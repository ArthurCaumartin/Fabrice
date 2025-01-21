using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutoScaleOnCollider : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider;

    void Update()
    {
        if(!sphereCollider) return;
        transform.localScale = Vector3.one * sphereCollider.radius * 2;
    }
}
