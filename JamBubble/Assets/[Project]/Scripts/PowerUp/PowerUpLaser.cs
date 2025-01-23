using UnityEngine;

public class PowerUpLaser : PowerUp
{
    [SerializeField] private Laser _laser;

    public override void OnGrab(GameObject playerRef)
    {
        if(playerRef.GetComponentInChildren<Laser>()) return;
        Laser newLaser = Instantiate(_laser, playerRef.transform);
        newLaser.Initialize(playerRef);
        base.OnGrab(playerRef);
    }
}
