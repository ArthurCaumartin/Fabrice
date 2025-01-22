using UnityEngine;

public class PowerUpWeaponSwap : PowerUp
{
    [Header("Weapon Swap : ")]
    [SerializeField] private GameObject _weaponToSwap;

    public override void OnGrab(GameObject playerRef)
    {
        Destroy(playerRef.GetComponentInChildren<Weapon>().gameObject);
        Instantiate(_weaponToSwap, playerRef.transform);
        base.OnGrab(playerRef);
    }
}