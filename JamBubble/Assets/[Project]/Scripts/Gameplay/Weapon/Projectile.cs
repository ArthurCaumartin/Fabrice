using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float _lifeTime = 1;
    [SerializeField] protected float _pushForce;
    protected GameObject _shooterParent;

    public virtual void Initialize(GameObject shooter)
    {
        _shooterParent = shooter;
        Destroy(gameObject, _lifeTime);
    }
}
