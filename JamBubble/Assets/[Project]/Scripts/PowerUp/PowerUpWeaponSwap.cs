using UnityEngine;

public class PowerUpWeaponSwap : PowerUp
{
    [SerializeField] private GameObject _weaponToSwap;

    public override void OnGrab(GameObject playerRef)
    {
        print("swap weapon");
        Destroy(playerRef.GetComponentInChildren<Weapon>().gameObject);
        Instantiate(_weaponToSwap, playerRef.transform);
        base.OnGrab(playerRef);
    }
}