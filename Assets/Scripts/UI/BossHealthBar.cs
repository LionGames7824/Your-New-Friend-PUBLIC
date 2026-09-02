using System;
using UnityEngine;
using UnityEngine.UI;


namespace YourNewFriend
{
    public class BossHealthBar : MonoBehaviour
    {
        [SerializeField] private BossHealth _bossHealth;
        [SerializeField] private Slider _slider;

        private void OnEnable()
        {
            _bossHealth.OnHealthChanged += UpdateHealth;
            _bossHealth.OnDeath += UpdateOnDeath;
            UpdateHealth(_bossHealth.CurrentHealth, _bossHealth.MaxHealth);
        }

        private void OnDisable()
        {
            _bossHealth.OnHealthChanged -= UpdateHealth;
            _bossHealth.OnDeath -= UpdateOnDeath;
        }

        private void UpdateHealth(int current, int max)
        {
            _slider.value = (float)current / max;
        }

        private void UpdateOnDeath()
        {
            Debug.Log("Call game over screen");
        }
    }
}