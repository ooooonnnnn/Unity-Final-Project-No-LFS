using System;
using Interface;
using UnityEngine;

namespace Extra
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        public static PlayerHealth Instance { get; private set; }
        public static event Action OnPlayerDied;
        public float health = 100;
        private bool isDead = false;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            Debug.Log("Player hit. HP: " + health);
            if (health <= 0)
            {
                Die();
            }
        }
        private void Die()
        {
            if (isDead) return;

            isDead = true;
            Debug.Log("Player died");

            OnPlayerDied?.Invoke();
        }
    }
}

