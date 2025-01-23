using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class Laser : Projectile
{
    [Header("Laser : ")]
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _lenth;

    [Header("Line Render : ")]
    [SerializeField] private Gradient _gradient;
    private float _activationTime;
    private LineRenderer _lineRenderer;

    public override void Initialize(GameObject shooter)
    {
        _shooterParent = shooter;
        transform.parent = shooter.transform;
        // shooter.GetComponentInChildren<Weapon>().DisableWeaponForTime(_lifeTime);
        _shooterParent.GetComponent<PlayerControler>().EnableControler(false, true);
        _lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    private void Update()
    {
        Vector3[] pathArray = ComputePath();
        // for (int i = 1; i < pathArray.Length; i++)
        // {
        //     Debug.DrawLine(pathArray[i - 1], pathArray[i], Color.red);
        // }

        UpdateVisual(Mathf.InverseLerp(0, _lifeTime, _activationTime), pathArray);
        _activationTime += Time.deltaTime;
        if (_activationTime > _lifeTime)
        {
            PushBodyOnPath(pathArray);
        }
    }

    public void UpdateVisual(float time, Vector3[] path)
    {
        _lineRenderer.positionCount = path.Length;
        _lineRenderer.SetPositions(path);

        Color colorTime = _gradient.Evaluate(time);

        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(colorTime, 0),
                                              new GradientColorKey(colorTime, 1) }
                    , new GradientAlphaKey[] { new GradientAlphaKey(colorTime.a, 0),
                                               new GradientAlphaKey(colorTime.a, 1) });

        _lineRenderer.colorGradient = grad;
    }

    private void PushBodyOnPath(Vector3[] path)
    {
        //! dsl c moche :)
        List<Rigidbody> banList = new List<Rigidbody>();
        banList.Add(transform.parent.GetComponent<Rigidbody>());
        for (int i = 1; i < path.Length; i++)
        {
            Vector3 dir = path[i] - path[i - 1];

            //TODO faire un shpere cast
            RaycastHit[] hits = Physics.RaycastAll(path[i - 1], dir, Vector3.Distance(path[i - 1], path[i]));

            Debug.DrawRay(path[i - 1], dir, Color.green, 5f);
            if (hits.Length == 0) continue;

            for (int j = 0; j < hits.Length; j++)
            {
                Rigidbody r = hits[j].collider.GetComponent<Rigidbody>();
                if (!r || banList.Contains(r)) continue;
                r.AddForce(dir.normalized * _pushForce, ForceMode.Impulse);
                banList.Add(r);
                print("Add force to : " + r.name);
            }
        }

        transform.parent.GetComponent<Rigidbody>()
        .AddForce(-transform.parent.forward * transform.parent.GetComponentInChildren<Weapon>().SelfPushForce, ForceMode.Impulse);

        _shooterParent.GetComponent<PlayerControler>().EnableControler(true, true);
        Destroy(gameObject);
    }

    private Vector3[] ComputePath()
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