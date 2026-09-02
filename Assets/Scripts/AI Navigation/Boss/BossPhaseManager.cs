using UnityEngine;
using System;

namespace YourNewFriend
{
    public class BossPhaseManager : MonoBehaviour
    {
        [SerializeField] private BossHealth _health;

        [Header("Phase Thresholds")]
        [SerializeField] private float _phase2Threshold = 0.60f;
        [SerializeField] private float _phase3Threshold = 0.25f;

        public int CurrentPhase { get; private set; } = 1;

        public event Action<int> OnPhaseChanged;

        private void Awake()
        {
            if (_health == null)
                _health = GetComponent<BossHealth>();
        }

        private void OnEnable()
        {
            _health.OnHealthChanged += HandleHealthChanged;
        }

        private void OnDisable()
        {
            _health.OnHealthChanged -= HandleHealthChanged;
        }

        private void HandleHealthChanged(int currentHealth, int maxHealth)
        {
            float healthPercent = (float)currentHealth / maxHealth;

            int newPhase;

            if (healthPercent <= _phase3Threshold)
                newPhase = 3;
            else if (healthPercent <= _phase2Threshold)
                newPhase = 2;
            else
                newPhase = 1;

            if (newPhase != CurrentPhase)
            {
                CurrentPhase = newPhase;
                OnPhaseChanged?.Invoke(CurrentPhase);
            }
        }
    }
}