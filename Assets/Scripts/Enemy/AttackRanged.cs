
using Interface;
using UnityEngine;


    public class AttackRanged : MonoBehaviour, IEnemyAttack
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Animator animator;
        [SerializeField] private EnemySpellExecutor spellExecutor;

        private Transform _target;
        private Transform _owner;
        private float _fireTimer;

        private static readonly int AttackHash = Animator.StringToHash("Attack");

        public void Initialize(Transform owner, Transform target)
        {
            _owner = owner;
            _target = target;
        }

        public void Tick(float dt)
        {
            _fireTimer += dt;

            if (_fireTimer < data.fireRate)
                return;

            _fireTimer = 0f;

            PerformAttack();
        }

        public bool IsInRange()
        {
            float distSqr = (_owner.position - _target.position).sqrMagnitude;

            float min = data.minAttackRange * data.minAttackRange;
            float max = data.maxAttackRange * data.maxAttackRange;

            return distSqr >= min && distSqr <= max;
        }

        private void PerformAttack()
        {
            animator.SetTrigger(AttackHash);

            if (!spellExecutor)
            {
                Debug.LogWarning($"{name}: No EnemySpellExecutor assigned.");
                return;
            }

            spellExecutor.Execute(_owner, _target, firePoint);
        }
    }
