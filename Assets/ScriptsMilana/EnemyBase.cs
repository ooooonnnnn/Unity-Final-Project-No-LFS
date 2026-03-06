using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    [SerializeField] private Transform firePoint;

    [SerializeField] Transform target;
    private Transform _transform;

    private float fireTimer;
    private float stoppingDistanceSqr;

    private void Awake()
    {
        _transform = transform;
        stoppingDistanceSqr = data.stoppingDistance * data.stoppingDistance;
    }

    private void OnEnable()
    {
        EnemyManager.Register(this);
    }

    private void OnDisable()
    {
        EnemyManager.Unregister(this);
    }

    public void Initialize(Transform playerTarget)
    {
        target = playerTarget;
    }

    public void Tick(float deltaTime)
    {
        if (!target) return;

        Vector3 enemyPos = _transform.position;
        Vector3 targetPos = target.position;

        Vector3 toTarget = targetPos - enemyPos;
        float sqrDistance = toTarget.sqrMagnitude;

        if (sqrDistance > stoppingDistanceSqr)
        {
            Move(toTarget, deltaTime);
            return;
        }

        Attack(targetPos, deltaTime);
    }

    private void Move(Vector3 toTarget, float dt)
    {
        Vector3 dir = toTarget.normalized;

        _transform.position += dir * (data.moveSpeed * dt);
        _transform.forward = dir;
    }

    private void Attack(Vector3 targetPos, float dt)
    {
        fireTimer += dt;

        if (fireTimer < data.fireRate)
            return;

        fireTimer = 0f;
        Shoot(targetPos);
    }

    private void Shoot(Vector3 targetPos)
    {
        Projectile proj = ProjectilePool.Instance.Get();

        proj.transform.SetPositionAndRotation(
            firePoint.position,
            firePoint.rotation
        );

        proj.Initialize(
            targetPos,
            data.projectileSpeed,
            data.projectileDamage
        );
    }
}