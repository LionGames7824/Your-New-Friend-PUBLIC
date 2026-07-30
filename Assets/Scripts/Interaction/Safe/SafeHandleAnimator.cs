using UnityEngine;

namespace YourNewFriend
{
    public class SafeHandleAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private static readonly int TryOpenTrigger = Animator.StringToHash("TryOpen");

        public void PlayTryOpen()
        {
            _animator.SetTrigger(TryOpenTrigger);
        }
    }
}