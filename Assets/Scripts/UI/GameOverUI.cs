using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace YourNewFriend
{
    public class GameOverUI : MonoBehaviour
    {
        [Tooltip("Called when the player presses Resume on the Game Over screen (e.g. respawning the player).")]
        public UnityEvent onResume;

        private void Start()
        {
            gameObject.SetActive(false);
            GameStateManager.Instance.IsGameOver += HandleGameOver;
        }

        private void OnDestroy()
        {
            if (GameStateManager.Instance == null) return;
            GameStateManager.Instance.IsGameOver -= HandleGameOver;
        }

        private void HandleGameOver(bool isOpen)
        {
            AudioManager.Instance.PlayMusic("GameOver", false);
            gameObject.SetActive(isOpen);
        }

        /// <summary>
        /// Connect this to the Resume button's onClick event in the Inspector.
        /// </summary>
        public void OnResumePressed()
        {
            Debug.Log("Resume button pressed");
            AudioManager.Instance.PlayBuffer();
            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(null);

            try
            {
                onResume.Invoke();
            }
            finally
            {
                GameStateManager.Instance.OpenGameOver(false);
            }
        }
    }
}
