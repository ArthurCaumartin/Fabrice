using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _spawnDelay = 10;
    [SerializeField] private List<GameObject> _prefabList;
    [SerializeField] private Transform[] _spawnPointArray;
    private GameObject[] _spawnArray;
    private float _spawnTime;

    private void Start()
    {
        _spawnArray = new GameObject[_spawnPointArray.Length];
    }

    private void Update()
    {
        _spawnTime += Time.deltaTime;
        if (_spawnTime >= _spawnDelay)
        {
            _spawnTime = 0;
            SpawnObject();
        }
    }

    public void SpawnObject()
    {
        int index = Random.Range(0, _spawnPointArray.Length);
        if (_spawnArray[index] != null) return;

        GameObject newPowerUp = Instantiate(_prefabList[Random.Range(0, _prefabList.Count)], _spawnPointArray[index]);
        _spawnArray[index] = newPowerUp;
    }
}