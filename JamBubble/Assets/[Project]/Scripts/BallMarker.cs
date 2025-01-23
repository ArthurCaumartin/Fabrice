using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMarker : MonoBehaviour
{
    private Transform parent;

    void Awake(){
        parent = transform.parent;
    }

    void Update()
    {
        float yPos = Mathf.Clamp(parent.position.y, 1.65f, 7.5f);
        //float clamp = Mathf.Clamp01(yPos);
        float lerpInverseYPos = Mathf.InverseLerp(1.65f, 7.5f, yPos);
        float scaleMarker = Mathf.Lerp(3.95f, 2f, lerpInverseYPos);
        transform.localScale = new Vector3(scaleMarker, scaleMarker, scaleMarker);
        transform.position = new Vector3(parent.position.x, -0.3f, parent.position.z);
        transform.localRotation = Quaternion.Euler(90,0,0);
    }
}
