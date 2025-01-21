using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Animation :")]
    [SerializeField] private float _yOffSet = .5f;
    [SerializeField] private float _speed = 5;
    private float _randomness;

    private void Start() => _randomness = Random.Range(1f, 1.5f);

    void Update()
    {
        Animation();
    }

    private void Animation()
    {
        transform.localPosition = new Vector3(0, Mathf.InverseLerp(-1, 1, Mathf.Sin(Time.time * _speed * _randomness)), 0);
        transform.Rotate(new Vector3(0, 5 * _speed * Time.deltaTime * _randomness, 0));
    }

    public void OnTriggerEnter(Collider other)
    {
        print("Trigger : " + other.name);
        PlayerManager m = other.GetComponent<PlayerManager>();
        if (m) OnGrab(m.gameObject);
    }

    public virtual void OnGrab(GameObject playerRef)
    {
        Destroy(gameObject);
    }
}
