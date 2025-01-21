using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private float _shootPerSecond;
    [SerializeField] private float _selfPushForce;
    private float _shootTime;

    private void Start()
    {
        transform.parent.GetComponent<PlayerControler>().SetWeapon(this);
    }

    private void Update()
    {
        _shootTime += Time.deltaTime;
    }

    public void Shoot(out float pushForce)
    {
        if (_shootTime < 1 / _shootPerSecond)
        {
            pushForce = 0;
            return;
        }

        _shootTime = 0;
        pushForce = _selfPushForce;
        InstantiateProjectile();
    }

    private void InstantiateProjectile()
    {
        Projectile newProj = Instantiate(_projectile, _shootPoint.position, transform.rotation);
        newProj.Initialize(transform.parent.gameObject);
    }
}
