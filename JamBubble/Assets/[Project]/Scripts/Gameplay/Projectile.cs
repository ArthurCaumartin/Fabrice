using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float _lifeTime = 1;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _pushForce;
    private GameObject _shooterParent;

    public void Initialize(GameObject shooter)
    {
        _shooterParent = shooter;
        Destroy(gameObject, _lifeTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * _moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != _shooterParent)
        {
            Rigidbody r = other.GetComponent<Rigidbody>();
            if (r) r.AddForce(transform.forward * _pushForce, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
}