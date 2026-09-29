using System;
using System.Collections;
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

        public float cooldown = TaserWireEffect.cooldown;
        private Coroutine _cooldown;
        private void Awake()
        {
            _currentHealth = _maxHealth;
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0)
                return;
            if (_cooldown != null)
                return;

            _currentHealth -= damage;
            _currentHealth = Mathf.Max(_currentHealth, 0);
            _cooldown = StartCoroutine(CoolDown(cooldown));

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

        private IEnumerator CoolDown(float cooldown)
        {
            yield return new WaitForSeconds(cooldown);
            _cooldown = null;
        }
    }
}