using System;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private float _shootPerSecond;
    [SerializeField] private float _selfPushForce;
    [SerializeField] private float _bubbleDecayPerShot = .01f;
    private float _shootTime;
    private BubbleControler _bubbleControler;
    private bool _canShoot = true;

    private void Start()
    {
        _bubbleControler = transform.parent.GetComponent<BubbleControler>();
        transform.parent.GetComponent<PlayerControler>().SetWeapon(this);
    }

    private void Update()
    {
        _shootTime += Time.deltaTime;
    }

    public void Shoot(out float pushForce)
    {
        pushForce = 0;
        if (!_canShoot) return;

        if (_shootTime > 1 / _shootPerSecond)
        {
            _shootTime = 0;
            pushForce = _selfPushForce;
            _bubbleControler?.UpdateSize(-_bubbleDecayPerShot);
            InstantiateProjectile();
        }
    }

    private void InstantiateProjectile()
    {
        Projectile newProj = Instantiate(_projectile, _shootPoint.position, transform.rotation);
        newProj.Initialize(transform.parent.gameObject);
    }

    public void DisableWeaponForTime(float time)
    {
        _canShoot = false;
        StartCoroutine(DisableDelay(time));
    }

    private IEnumerator DisableDelay(float time)
    {
        yield return new WaitForSeconds(time);
        _canShoot = true;
    }
}
