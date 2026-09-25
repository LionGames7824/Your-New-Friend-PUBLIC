using UnityEngine;
using UnityEngine.Events;

namespace YourNewFriend
{
    public class BossArenaTrigger : MonoBehaviour
    {
        [SerializeField] private BossController _bossController;

        [Tooltip("Shown the moment the fight starts - e.g. the boss health bar. Left untouched if not set.")]
        [SerializeField] private GameObject _uiToShowOnStart;

        [Tooltip("If true, walking in a second time (after already starting the fight once) does nothing.")]
        [SerializeField] private bool _triggerOnce = true;
        [SerializeField] private UnityEvent _onStart;

        private bool _hasTriggered;

        private void OnEnable()
        {
            _bossController.OnFightReset += HandleFightReset;
        }

        private void OnDisable()
        {
            _bossController.OnFightReset -= HandleFightReset;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            TriggerBattle();
        }

        public void TriggerBattle()
        {
            if (_triggerOnce && _hasTriggered)
                return;

            _hasTriggered = true;

            if (_uiToShowOnStart != null)
                _uiToShowOnStart.SetActive(true);

            _onStart?.Invoke();
            AudioManager.Instance.PlayMusic("RoboTrap", true);
            _bossController.StartFight();
        }

        private void HandleFightReset()
        {
            _hasTriggered = false;

            if (_uiToShowOnStart != null)
                _uiToShowOnStart.SetActive(false);
        }
    }
}
