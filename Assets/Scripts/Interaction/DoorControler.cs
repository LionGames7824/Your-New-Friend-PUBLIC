using System;
using UnityEngine;
using UnityEngine.Events;

namespace YourNewFriend
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onDoorOpened;
        
        private Animator _anim;
        private string _doorState = "Door_State";

        private void Awake()
        {            
            _anim = GetComponent<Animator>();
        }

        public void ToggleDoor()
        {
            // 1. Verificamos o valor real que está vindo do Animator antes de mudar
            Debug.Log($"Valor atual de Door_State ANTES do clique: {_anim.GetBool(_doorState)}");

            bool isOpening = !_anim.GetBool(_doorState);
            _anim.SetBool(_doorState, isOpening);

            Debug.Log($"isOpening calculou como: {isOpening}");

            // Se a sua lógica do Animator estiver invertida (false abre a porta), 
            // mudar para: if (!isOpening)
            if (isOpening)
            {
                _onDoorOpened.Invoke();
                Debug.Log("O evento _onDoorOpened foi invocado!");
            }
        }
    }
}