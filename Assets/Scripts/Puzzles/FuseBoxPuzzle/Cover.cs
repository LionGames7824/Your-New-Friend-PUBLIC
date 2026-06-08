using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace YourNewFriend
{
    public class Cover : MonoBehaviour, IInteractable
    {
        public PlayableDirector cameraDir;
        public Animator anim;
        private bool opened = false;
        public static GameObject Player;
        [SerializeField] private string _interactionPrompt = "Open";
        [SerializeField] private TimelineAsset _boxOpen;
        [SerializeField] private TimelineAsset _boxClose;
        public GameObject canvas;

        public static Action<bool> onOpenCover;
        public string InteractionPrompt => _interactionPrompt;
        /*
        void Awake()
        {
            PlayerController playerController = GetComponent<PlayerController>();
            playerController.SetControl(true);
        }
        */
        void Start()
        {
            anim.SetTrigger("Close");
        }
        
        void Update()
        {
            if (WireSwitch.completed && !opened)
            {
                cameraDir.Play(_boxClose);
                canvas.SetActive(true);
                anim.SetTrigger("Close");
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                WireSwitch.entrance = false;
                opened = true;
                onOpenCover?.Invoke(opened);
            }
        }
        public bool Interact(PlayerController interaction)
        {
            if (!opened)
            {
                cameraDir.Play(_boxOpen);
                canvas.SetActive(false);
                anim.SetTrigger("Open");
                WireSwitch.entrance = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                onOpenCover?.Invoke(opened);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

