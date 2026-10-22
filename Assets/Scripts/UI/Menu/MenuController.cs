
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace YourNewFriend
{
    public class MenuController : MonoBehaviour
    {
        [Header("Menu Video Variables")]
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private GameObject _rawImage;

        [Header("Menu Variables")]
        [SerializeField] private GameObject _menuOptions;

        [Tooltip("Load Game button. Disabled when there is no save to continue.")]
        [SerializeField] private Button _loadButton;

        [Header("Options Menu")]
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _optionsMenuPanel;

        [SerializeField] private string _sceneName = "MainScene";
        [SerializeField] private string _gameStartMusic = "EscapeGuide";

        [SerializeField] private LoadingScene _loadingScene;

        private InputAction _anyKey;
        private Animator _animatorRawImage;

        private void Awake()
        {
            _animatorRawImage = _rawImage.GetComponent<Animator>();
            _rawImage.SetActive(false);

            if (_loadButton != null)
                _loadButton.interactable = SaveSystem.HasSave();

            if (_mainMenuPanel != null)
                _mainMenuPanel.SetActive(false);

            if (_optionsMenuPanel != null)
                _optionsMenuPanel.SetActive(false);
        }

        private void OnEnable()
        {
            _anyKey = new InputAction(binding: "<Keyboard>/anyKey");
            _anyKey.AddBinding("<Mouse>/leftButton");
            _anyKey.AddBinding("<Gamepad>/buttonSouth");
            _anyKey.performed += OnAnyKeyPressed;
            _anyKey.Enable();
        }

        private void OnDisable()
        {
            _anyKey.performed -= OnAnyKeyPressed;
            _anyKey.Disable();
        }

        private void OnAnyKeyPressed(InputAction.CallbackContext context)
        {
            if (!_videoPlayer.isPlaying)
            {
                AudioManager.Instance.PlayMusic("MenuSong", false);
                _videoPlayer.Play();
                _rawImage.SetActive(true);
                _animatorRawImage.SetTrigger("fadeIn");
                _menuOptions.SetActive(true);
            }
        }

        public void OnStartGame()
        {
            SaveSystem.StartNewGame();
            StartGameMusic();
            _loadingScene.LoadScene(_sceneName);
        }

        public void OnLoadGame()
        {
            if (!SaveSystem.HasSave())
            {
                Debug.Log("No save found to load.");
                return;
            }

            SaveSystem.PrepareLoadGame();
            StartGameMusic();
            _loadingScene.LoadScene(_sceneName);
        }
        private void StartGameMusic()
        {
            AudioManager.Instance.ResetBuffer();
            AudioManager.Instance.PlayMusic(_gameStartMusic, false);
        }

        public void OnOptions()
        {
            if (_mainMenuPanel != null)
                _mainMenuPanel.SetActive(false);

            if (_optionsMenuPanel != null)
                _optionsMenuPanel.SetActive(true);
        }

        public void OnBackFromOptions()
        {
            if (_optionsMenuPanel != null)
                _optionsMenuPanel.SetActive(false);

            if (_mainMenuPanel != null)
                _mainMenuPanel.SetActive(true);
        }

        public void OnExitGame()
        {
            Application.Quit();
        }
    }
}
