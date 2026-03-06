using UnityEngine;


public class Projectile : MonoBehaviour
{
    private float speed;
    private float damage;
    private Vector3 direction;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    public void Initialize(Vector3 targetPos, float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;

        direction = (targetPos - _transform.position).normalized;
    }

    private void Update()
    {
        _transform.position += direction * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }

        ProjectilePool.Instance.Return(this);
    }
}