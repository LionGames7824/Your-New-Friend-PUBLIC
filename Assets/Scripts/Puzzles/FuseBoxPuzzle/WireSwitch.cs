using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace YourNewFriend
{
    public class WireSwitch : MonoBehaviour
    {
        public static bool entrance = false;
        private static bool check = true;
        public static bool completed = false;
        private static int order = 0;
        private bool state = false;
        private bool isPlaying = false;
        public int color = 0; // R=1, G=2, B=7, O=4, Y=6, C=5, P=3

        [Header("Materials")]
        [SerializeField] private Material red_select;
        [SerializeField] private Material red_deselect;
        [SerializeField] private Material green;

        [Header("Events")]
        [SerializeField] public UnityEvent _lightOn;
        [SerializeField] public UnityEvent _lightOff;
        [SerializeField] public Vector3 _flickUp = new Vector3 (315, 90, 90); //off
        [SerializeField] public Vector3 _flickDown = new Vector3 (315, 270, 270); //on

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Transform>().rotation = Quaternion.Euler(_flickUp);
            state = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isPlaying)
            {
                if (entrance) GetComponent<Renderer>().material = red_deselect;
                Vector2 screenPos = Mouse.current.position.ReadValue();
                Vector3 mousePos = new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane);
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                if (Physics.Raycast(ray, out RaycastHit hitInfo) && hitInfo.transform == transform)
                {
                    if (entrance) GetComponent<Renderer>().material = red_select;
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        if (entrance)
                        {
                            if (!state)
                            {
                                state = !state;
                                GetComponent<Transform>().rotation = Quaternion.Euler(_flickDown);
                                _lightOn.Invoke();
                                order++;
                                if (color != order)
                                {
                                    Debug.Log("Wrong Switch");
                                    check = false;
                                }
                            }
                        }
                    }
                }
                if (order == 7)
                {
                    if (check)
                    {
                        StartCoroutine(Right());
                    }
                    else
                    {
                        StartCoroutine(Wrong());
                    }
                }
            }
        }

        IEnumerator Wrong()
        {
            isPlaying = true;
            yield return new WaitForSeconds(2f);
            //For Future: play an "incorrect" buzz sound
            GetComponent<Renderer>().material = red_select;
            yield return new WaitForSeconds(0.5f);
            GetComponent<Renderer>().material = red_deselect;
            yield return new WaitForSeconds(0.5f);
            GetComponent<Renderer>().material = red_select;
            yield return new WaitForSeconds(0.5f);
            GetComponent<Renderer>().material = red_deselect;
            yield return new WaitForSeconds(1f);
            GetComponent<Transform>().rotation = Quaternion.Euler(_flickUp);
            _lightOff.Invoke();
            order = 0;
            state = false;
            check = true;
            isPlaying = false;
        }

        IEnumerator Right()
        {
            isPlaying = true;
            yield return new WaitForSeconds(2f);
            //For Future: play a "correct" buzz sound
            GetComponent<Renderer>().material = green;
            yield return new WaitForSeconds(2f);
            completed = true;
            isPlaying = false;
        }
    }
}

