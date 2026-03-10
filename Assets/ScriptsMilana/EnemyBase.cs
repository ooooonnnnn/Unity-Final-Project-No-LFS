using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    [SerializeField] private Transform firePoint;
    [SerializeField] private NavMeshAgent agent;
    private Transform target;

    
    private float fireTimer;

    public static event System.Action<EnemyBase> OnEnemyKilled;

    private void Awake()
    {
        agent.speed = data.moveSpeed;
        agent.stoppingDistance = data.stoppingDistance;

        
        agent.avoidancePriority = Random.Range(40, 90);
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

    public void Tick(float dt)
    {
        if (!target) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > data.stoppingDistance)
        {
            ChasePlayer();
        }
        else
        {
            Attack(target.position, dt);
        }
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;

        
        Vector3 offset = Random.insideUnitSphere * 1.5f;
        offset.y = 0;

        agent.SetDestination(target.position + offset);
    }

    private void Attack(Vector3 targetPos, float dt)
    {
        agent.isStopped = true;

        Vector3 lookDir = (targetPos - transform.position).normalized;
        lookDir.y = 0;
        transform.forward = lookDir;

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

    public void Die()
    {
        OnEnemyKilled?.Invoke(this);
        gameObject.SetActive(false);
    }
}