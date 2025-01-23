using UnityEngine;

public class PowerUpSpawner : PowerUp
{
    [Header("Spawner : ")]
    [SerializeField] private GameObject _prefabToSpawn;

    public override void OnGrab(GameObject playerRef)
    {
        GameObject o = Instantiate(_prefabToSpawn, transform.position, Quaternion.identity);
        GameManager.Instance?.objectsToDestroy.Add(o);
        base.OnGrab(playerRef);
    }
}
