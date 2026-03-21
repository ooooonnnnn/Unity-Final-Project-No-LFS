using System;
using Interface;
using UnityEngine;
using UnityEngine.AI;

namespace Camera
{
    public class CharacterComponents : MonoBehaviour ,IDamageable
    {
        
        public event Action OnPlayerDied;
        public NavMeshAgent navMeshAgent => _navMeshAgent;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        public Transform cameraFollowTarget => _cameraFollowTarget;
        [SerializeField] private Transform _cameraFollowTarget;

        public Collider playerCollider => _PlayerColider;
        [SerializeField] private Collider _PlayerColider;

        [SerializeField] private float health = 100f;

        
        private Vector3 startPosition;
        private Quaternion startRotation;

        
        
        private void Awake()
        {
            if (!_navMeshAgent)
                _navMeshAgent =
                    GetComponent<NavMeshAgent>(); // called only in the rare case it wasn't set in the editor 

            startPosition = transform.position;
            startRotation = transform.rotation;
        }

        private void OnValidate()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>(); //editor time
        }

        public void ResetToStart()
        {
            if (_navMeshAgent && _navMeshAgent.enabled)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.Warp(startPosition);
                _navMeshAgent.velocity = Vector3.zero;
            }

            transform.SetPositionAndRotation(startPosition, startRotation);
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                OnPlayerDied?.Invoke();
            }
        }
    }
}