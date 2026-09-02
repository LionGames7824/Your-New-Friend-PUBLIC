using UnityEngine;
using System;

namespace YourNewFriend
{
    public class BossPhaseManager : MonoBehaviour
    {
        [SerializeField] private BossHealth health;

        [Header("Phase Thresholds")]
        [SerializeField] private float phase2Threshold = 0.60f;
        [SerializeField] private float phase3Threshold = 0.25f;

        public int CurrentPhase { get; private set; } = 1;

        public event Action<int> OnPhaseChanged;

        private void Awake()
        {
            if (health == null)
                health = GetComponent<BossHealth>();
        }

        private void OnEnable()
        {
            health.OnHealthChanged += HandleHealthChanged;
        }

        private void OnDisable()
        {
            health.OnHealthChanged -= HandleHealthChanged;
        }

        private void HandleHealthChanged(int currentHealth, int maxHealth)
        {
            float healthPercent = (float)currentHealth / maxHealth;

            int newPhase;

            if (healthPercent <= phase3Threshold)
                newPhase = 3;
            else if (healthPercent <= phase2Threshold)
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