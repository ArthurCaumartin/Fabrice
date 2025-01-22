using System.Collections.Generic;
using UnityEngine;

public class Laser : Projectile
{
    [Header("Laser : ")]
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _activationDelay;
    [SerializeField] private float _lenth;
    private float _activationTime;

    public override void Initialize(GameObject shooter)
    {
        transform.parent = shooter.transform;
        shooter.GetComponentInChildren<Weapon>().DisableWeaponForTime(_lifeTime);
        base.Initialize(shooter);
    }

    public struct Path
    {
        public Vector3 start;
        public Vector3 end;
        public Path(Vector3 start, Vector3 end)
        {
            this.start = start;
            this.end = end;
        }
    }

    private void Update()
    {
        Vector3[] pathArray = ComputePath();
        for (int i = 1; i < pathArray.Length; i++)
        {
            Debug.DrawLine(pathArray[i - 1], pathArray[i], Color.red);
        }


        _activationTime += Time.deltaTime;
        if(_activationTime > _activationDelay)
        {
            ActivateLaser();
        }
    }

    public void ActivateLaser()
    {

    }

    public Vector3[] ComputePath()
    {
        List<Vector3> pathList = new List<Vector3>();
        pathList.Add(transform.position);

        Vector3 rayDirection = transform.forward;

        for (float travelDist = 0; travelDist < _lenth; travelDist += .01f)
        {
            Physics.Raycast(pathList[pathList.Count - 1], rayDirection, out RaycastHit hit, _lenth - travelDist, _wallLayer);
            if (hit.collider)
            {
                float dist = Vector3.Distance(pathList[pathList.Count - 1], hit.point);
                travelDist += dist;
                pathList.Add(hit.point);
                rayDirection = Vector3.Reflect(rayDirection, hit.normal);
            }
            else
            {
                pathList.Add(pathList[pathList.Count - 1] + (rayDirection * (_lenth - travelDist)));
                break;
            }
        }

        return pathList.ToArray();
    }
}