using System.Collections.Generic;
using UnityEngine;

public class Stuff : MonoBehaviour
{
    [SerializeField, Range(1, 100)] int _count = 1;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _distance = 5;
    [SerializeField] private Transform _stuff;
    private List<Transform> _stuffList = new List<Transform>();
    private float time;

    void Update()
    {
        time += Time.deltaTime * _speed;

        if (_count != _stuffList.Count)
        {
            for (int i = 0; i < _stuffList.Count; i++)
            {
                Destroy(_stuffList[i].gameObject);
            }
            _stuffList.Clear();

            for (int i = 0; i < _count; i++)
            {
                _stuffList.Add(Instantiate(_stuff));
            }
        }


        for (int i = 0; i < _stuffList.Count; i++)
        {
            float countTime = Mathf.Lerp(0, 360, Mathf.InverseLerp(0, _stuffList.Count, i)) * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(time + countTime), Mathf.Sin(time + countTime * _speed), Mathf.Sin(time + countTime)) * _distance;
            _stuffList[i].transform.position = pos;
        }
    }
}
