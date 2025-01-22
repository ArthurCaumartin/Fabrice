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
        if (_shootTime < 1 / _shootPerSecond)
        {
            pushForce = 0;
            return;
        }

        _shootTime = 0;
        pushForce = _selfPushForce;
        _bubbleControler?.UpdateSize(-_bubbleDecayPerShot);
        InstantiateProjectile();
    }

    private void InstantiateProjectile()
    {
        Projectile newProj = Instantiate(_projectile, _shootPoint.position, transform.rotation);
        newProj.Initialize(transform.parent.gameObject);
    }
}
