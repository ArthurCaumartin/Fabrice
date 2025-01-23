using UnityEngine;
using Rewired;
public class PlayerControler : MonoBehaviour
{
    [SerializeField] private float _aimSpeed = 5;
    private Weapon _weapon;
    private Player _player;
    private Rigidbody _rigidbody;
    private Vector3 _aimInput;
    private bool _canAim = true;
    private bool _canShoot = true;
    private SphereCollider _sphereCollider;

    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _player = GetComponent<PlayerManager>().player;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        SetInputValue();

        if (_canAim) Aim();
        if (_canShoot) Shoot();
    }

    public void SetInputValue()
    {
        _aimInput = new Vector3(_player.GetAxis("Horizontal"), 0, _player.GetAxis("Vertical"));
    }

    private void Aim()
    {
        if (_aimInput == Vector3.zero) return;
        transform.forward = Vector3.Lerp(transform.forward, _aimInput, Time.deltaTime * _aimSpeed);
    }

    private void Shoot()
    {
        if (_player.GetButton("Shoot"))
        {
            _weapon.Shoot(out float pushForce);
            _rigidbody.AddForce(-transform.forward * pushForce * GetComponent<BubbleControler>().ForceMult, ForceMode.Impulse);
        }
    }

    public void EnableControler(bool canShoot, bool canAim)
    {
        _canShoot = canShoot;
        _canAim = canAim;
    }

    public void SetWeapon(Weapon weapon)
    {
        _weapon = weapon;
    }

    void OnCollisionEnter(Collision collision)
    {
        GameManager.Instance?.ControllerVibration(GetComponent<PlayerManager>().joystickId, collision.relativeVelocity.magnitude * 0.01f, .1f);
    }
}
