using Enemy;
using Interface;
using UnityEngine;
using UnityEngine.AI;


    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform visual;
        [SerializeField] private MonoBehaviour attackBehaviour; 
        
        private IEnemyAttack attack;

        private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private Transform target;
        private Transform enemyTransform;

    
       
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
           
            
            attack = attackBehaviour as IEnemyAttack;
            if (attack == null)
            {
                Debug.LogError("AttackBehaviour does not implement IEnemyAttack");
            }
            agent.avoidancePriority = Random.Range(70, 90);
            if (attack is AttackMelee)
            {
                agent.stoppingDistance = 0f;
            }
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
            attack.Initialize(transform, target);
        }

        public void Tick(float dt)
        {
            if (!target) return;

            bool inRange = attack.IsInRange();
            bool isMoving = !inRange;

            bool isAttackingAnim = IsInAttackState();

            if (isAttackingAnim)
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
                agent.isStopped = true;
                attack.Tick(dt);
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
