using UnityEngine;

namespace YourNewFriend
{
    public class BossArenaTrigger : MonoBehaviour
    {
        [SerializeField] private BossController _bossController;

        [Tooltip("If true, walking in a second time (after already starting the fight once) does nothing.")]
        [SerializeField] private bool _triggerOnce = true;

        private bool _hasTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (_triggerOnce && _hasTriggered)
                return;

            if (!other.CompareTag("Player"))
                return;

            _hasTriggered = true;
            _bossController.StartFight();
        }
    }
}
