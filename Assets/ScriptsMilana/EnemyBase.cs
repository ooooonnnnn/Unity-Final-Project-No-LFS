using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float separationRadius = 2f;
    [SerializeField] private float separationStrength = 4f;

    [SerializeField] Transform target;
    private Transform _transform;

    private float _fireTimer;
    private float _stoppingDistanceSqr;

    private void Awake()
    {
        _transform = transform;
        _stoppingDistanceSqr = data.stoppingDistance * data.stoppingDistance;
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

        Vector3 separation = CalculateSeparation();

        if (sqrDistance > _stoppingDistanceSqr)
        {
            Move(toTarget, separation, deltaTime);
        }
        else
        {
            ApplySeparation(separation, deltaTime);
            Attack(targetPos, deltaTime);
        }

        EnforceMinimumDistance(); 
    }

    private void Move(Vector3 toTarget, Vector3 separation, float dt)
    {
        Vector3 targetDir = toTarget.normalized;

        Vector3 finalDir = (targetDir + separation).normalized;

        _transform.position += finalDir * (data.moveSpeed * dt);

        if (finalDir != Vector3.zero)
            _transform.forward = finalDir;
    }

    private void Attack(Vector3 targetPos, float dt)
    {
        Vector3 lookDir = (targetPos - _transform.position).normalized;
        lookDir.y = 0;
        _transform.forward = lookDir;
        _fireTimer += dt;

        if (_fireTimer < data.fireRate)
            return;

        _fireTimer = 0f;
        Shoot(targetPos);
    }
    private void ApplySeparation(Vector3 separation, float dt)
    {
        if (separation == Vector3.zero)
            return;

        _transform.position += separation * dt;
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
    private Vector3 CalculateSeparation()
    {
        Vector3 force = Vector3.zero;

        foreach (var enemy in EnemyManager.Enemies)
        {
            if (enemy == this) continue;

            Vector3 diff = _transform.position - enemy.transform.position;
            float dist = diff.magnitude;

            if (dist < separationRadius && dist > 0.01f)
            {
                float strength = (separationRadius - dist) / separationRadius;
                force += diff.normalized * strength;
            }
        }

        return force * separationStrength;
    }
    
    private void EnforceMinimumDistance()
    {
        float minDist = separationRadius * 0.6f;   
        float minDistSqr = minDist * minDist;

        foreach (var enemy in EnemyManager.Enemies)
        {
            if (enemy == this) continue;

            Vector3 diff = _transform.position - enemy.transform.position;
            float distSqr = diff.sqrMagnitude;

            if (distSqr < minDistSqr && distSqr > 0.0001f)
            {
                float dist = Mathf.Sqrt(distSqr);
                Vector3 pushDir = diff / dist;

                float pushAmount = (minDist - dist) * 0.5f;

                _transform.position += pushDir * pushAmount;
                enemy.transform.position -= pushDir * pushAmount;
            }
        }
    }
}