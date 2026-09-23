using System;
using UnityEngine;

namespace YourNewFriend
{
    public class BossHealth : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 100;

        private int _currentHealth;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;

        public event Action<int, int> OnHealthChanged;
        public event Action OnDeath;

        private void Awake()
        {
            _currentHealth = _maxHealth;
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0)
                return;

            _currentHealth -= damage;
            _currentHealth = Mathf.Max(_currentHealth, 0);

            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            OnDeath?.Invoke();
        }

        public void ResetHealth()
        {
            _currentHealth = _maxHealth;
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }
    }
}