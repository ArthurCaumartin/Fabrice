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

    [Header("_lifeTime 0 = for ever")]
    [SerializeField] private float _lifeTime = 0;

    private float _shootTime;
    private BubbleControler _bubbleControler;
    private bool _canShoot = true;

    public float LifeTime { get => _lifeTime; }
    public float SelfPushForce { get => _selfPushForce; }

    private void Start()
    {
        if (_lifeTime != 0) Destroy(gameObject, _lifeTime);

        _bubbleControler = transform.parent.GetComponent<BubbleControler>();
        transform.parent.GetComponent<PlayerControler>().SetWeapon(this);
        _shootTime = 100;
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
            AudioManager.Instance?.PlaySFX("shoot", .25f, UnityEngine.Random.Range(.9f, 1.1f));
            GameManager.Instance?.Shake(0.1f, 0.05f);
            //GameManager.Instance.ControllerVibration(transform.parent.GetComponent<PlayerManager>().joystickId, 0.05f, 0.05f);
        }

        if (_projectile is Laser) pushForce = 0;
    }

    private void InstantiateProjectile()
    {
        if(!_projectile) return;
        //! y'a une erreure de temps en temps... si on ferme les yeux elle existe pas
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
        transform.parent.GetComponent<PlayerControler>().SetWeapon(this);
    }
}
