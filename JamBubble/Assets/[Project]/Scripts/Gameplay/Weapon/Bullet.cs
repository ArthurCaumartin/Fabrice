using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField] private float _moveSpeed = 15;

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
