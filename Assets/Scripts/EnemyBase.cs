using UnityEngine;
using UnityEngine.AI;

namespace ScriptsMilana
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private Transform firePoint;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform visual;

        private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private Transform target;
        private Transform enemyTransform;

    
        private float fireTimer;
        private float pathUpdateTimer;
        private float stoppingDistanceSqr;
        private bool isAttacking;
        private bool isRotationLocked;
        private Quaternion lockedRotation;
        public float rotationSpeed = 360f;

        public static event System.Action<EnemyBase> OnEnemyKilled;

        private void Awake()
        {
            enemyTransform = transform;
            agent.speed = data.moveSpeed;
            agent.stoppingDistance = data.stoppingDistance;
            agent.updateRotation = false;
            stoppingDistanceSqr = data.stoppingDistance * data.stoppingDistance;

        
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

            
            var enemyPos = enemyTransform.position;
            var targetPos = target.position;
            float distSqr = (enemyPos - targetPos).sqrMagnitude;
            bool isMoving = distSqr > stoppingDistanceSqr;

            bool isAttacking = IsInAttackState();

            if (isAttacking)
            {
                if (!isRotationLocked)
                {
                    lockedRotation = enemyTransform.rotation;
                    isRotationLocked = true;
                }

                enemyTransform.rotation = lockedRotation;
            }
            else
            {
                isRotationLocked = false;

                if (isMoving && agent.hasPath && agent.velocity.sqrMagnitude > 0.1f)
                {
                    RotateTowardsTarget(dt);
                }
            }


            

            
            
            animator.SetBool(IsMovingHash, isMoving);

            if (isMoving)
            {
                ChasePlayer(dt);
            }
            else
            {
                Attack(targetPos, dt);
            }
        }
        private void RotateTowardsTarget(float dt)
        {
            if (agent.velocity.sqrMagnitude < 0.1f)
                return;

            Vector3 direction = agent.velocity;
            direction.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            enemyTransform.rotation = Quaternion.RotateTowards(enemyTransform.rotation, targetRotation, rotationSpeed * dt
            );
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
            
            fireTimer += dt;

            if (fireTimer < data.fireRate)
                return;

            fireTimer = 0f;
            isAttacking = true;

            animator.SetTrigger(AttackHash); 
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
        
        private bool IsInAttackState()
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            return state.IsName("Attack");
        }

        public void Die()
        {
            OnEnemyKilled?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}