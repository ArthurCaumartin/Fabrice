using UnityEngine;

public class PowerUpSpawner : PowerUp
{
    [Header("Spawner : ")]
    [SerializeField] private GameObject _prefabToSpawn;

    public override void OnGrab(GameObject playerRef)
    {
        Instantiate(_prefabToSpawn, transform.position, Quaternion.identity);
        base.OnGrab(playerRef);
    }
}
