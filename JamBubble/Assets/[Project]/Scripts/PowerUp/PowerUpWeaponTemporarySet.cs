using UnityEngine;

public class PowerUpWeaponTemporarySet : PowerUp
{
    [Header("Weapon Swap : ")]
    [SerializeField] private Weapon _weaponToSwap;

    public override void OnGrab(GameObject playerRef)
    {
        if (playerRef.GetComponentsInChildren<Weapon>().Length > 1) return;
        playerRef.GetComponentInChildren<Weapon>().DisableWeaponForTime(_weaponToSwap.LifeTime);

        Instantiate(_weaponToSwap, playerRef.transform);
    }


}