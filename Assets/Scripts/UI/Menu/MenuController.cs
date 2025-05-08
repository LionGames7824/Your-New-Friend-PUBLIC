using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

namespace YourNewFriend
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private GameObject _menuOptions;
        [SerializeField] private GameObject _rawImage;
        private InputAction _anyKey;
        private Animator _animatorRawImage;

        private void Awake()
        {
            _animatorRawImage = _rawImage.GetComponent<Animator>();
            _rawImage.SetActive(false);
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
                _videoPlayer.Play();
                _rawImage.SetActive(true);
                _animatorRawImage.SetTrigger("fadeIn");
                _menuOptions.SetActive(true);
            }
        }
    }
}