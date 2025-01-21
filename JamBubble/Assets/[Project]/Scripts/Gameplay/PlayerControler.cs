using UnityEngine;
using Rewired;
public class PlayerControler : MonoBehaviour
{
    [SerializeField] private float _aimSpeed = 5;
    private Weapon _weapon;
    private Player _player;
    private Rigidbody _rigidbody;
    private Vector3 _aimInput;
    private bool _canAim;
    private bool _canShoot;

    private void Start()
    {
        _player = GetComponent<PlayerManager>().player;
        _rigidbody = GetComponent<Rigidbody>();
        _weapon = GetComponentInChildren<Weapon>();
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
        transform.forward = Vector3.Lerp(transform.forward, _aimInput, Time.deltaTime * _aimSpeed);
    }

    private void Shoot()
    {
        if (_player.GetButton("Shoot"))
        {
            _weapon.Shoot(out float pushForce);
            _rigidbody.AddForce(-transform.forward * pushForce, ForceMode.Impulse);
        }
    }

    public void EnableControler(bool canShoot, bool canAim)
    {
        _canShoot = canShoot;
        _canAim = canAim;
    }
}


