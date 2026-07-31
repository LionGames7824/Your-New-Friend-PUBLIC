using UnityEngine;

namespace YourNewFriend
{
    public class DoorSafeAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private static readonly int OpenTrigger = Animator.StringToHash("OpenSafe");

        public void PlayOpen()
        {
            _animator.SetTrigger(OpenTrigger);
            Debug.Log("PlayOpen chamado às " + Time.time);
        }
    }
}