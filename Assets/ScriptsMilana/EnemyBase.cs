using UnityEngine;
using UnityEngine.AI;

namespace ScriptsMilana
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private Transform firePoint;
        [SerializeField] private NavMeshAgent agent;
        private Transform target;
        private Transform enemyTransform;

    
        private float fireTimer;
        private float pathUpdateTimer;

        public static event System.Action<EnemyBase> OnEnemyKilled;

        private void Awake()
        {
            enemyTransform = transform;
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

            float distSqr = (enemyTransform.position - target.position).sqrMagnitude;

            if (distSqr > data.stoppingDistance * data.stoppingDistance)
            {
                ChasePlayer(dt);
            }
            else
            {
                Attack(target.position, dt);
            }
        }

        private void ChasePlayer(float dt)
        {
            agent.isStopped = false;

            pathUpdateTimer += dt;

            if (pathUpdateTimer < 0.25f)
                return;

            pathUpdateTimer = 0f;

            Vector3 offset = Random.insideUnitSphere * 1.5f;
            offset.y = 0;

            agent.SetDestination(target.position + offset);
        }

        private void Attack(Vector3 targetPos, float dt)
        {
            agent.isStopped = true;

            Vector3 lookDir = (targetPos - enemyTransform.position).normalized;
            lookDir.y = 0;
            enemyTransform.forward = lookDir;

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
}