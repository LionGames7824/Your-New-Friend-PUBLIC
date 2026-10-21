using System.Collections;
using UnityEngine;
using TMPro;

namespace YourNewFriend
{
    public class LoadingTextAnimation : MonoBehaviour
    {
        [SerializeField] private TMP_Text _loadingText;
        [SerializeField] private float _animationSpeed = 0.5f;

        private Coroutine animationCoroutine;

        private void OnEnable()
        {
            animationCoroutine = StartCoroutine(AnimateLoadingText());
        }

        private void OnDisable()
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
            }
        }

        private IEnumerator AnimateLoadingText()
        {
            int dots = 0;

            while (true)
            {
                _loadingText.text = new string('.', dots);

                dots = (dots + 1) % 4;

                yield return new WaitForSeconds(_animationSpeed);
            }
        }
    }
}