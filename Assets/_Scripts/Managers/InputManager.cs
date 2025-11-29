using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

namespace WhatIf
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        
        private Vector2 movementInput;
        
        
        private bool jumpPressed;
        
        private bool runHeld;
        
        private bool attackPressed;
        
        private bool interactPressed;

        public Vector2 MovementInput => movementInput;
        
        public bool JumpPressed => jumpPressed;
        
        public bool RunHeld => runHeld;
        
        public bool AttackPressed => attackPressed;
        
        public bool InteractPressed => interactPressed;
        
        public bool HasMovementInput => movementInput.magnitude > 0.1f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            UpdateInputs();
        }
        
        private void UpdateInputs()
        {

            
            
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
            {
                if (EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() != null)
                {
                    ResetInputs();
                    return;
                }
            }
            float horizontal = 0f;
            float vertical = 0f;
            if (Input.GetKey(KeyCode.W)) vertical += 1f;
            if (Input.GetKey(KeyCode.S)) vertical -= 1f;
            if (Input.GetKey(KeyCode.A)) horizontal -= 1f;
            if (Input.GetKey(KeyCode.D)) horizontal += 1f;
            
            movementInput = new Vector2(horizontal, vertical);
            
            jumpPressed = Input.GetKeyDown(KeyCode.Space);
            
            runHeld = Input.GetKey(KeyCode.LeftShift);
            
            interactPressed = Input.GetKeyDown(KeyCode.E);
            
            attackPressed = Input.GetKeyDown(KeyCode.J);
        }
        
        private void ResetInputs()
        {
            movementInput = Vector2.zero;
            jumpPressed = false;
            runHeld = false;
            attackPressed = false;
            interactPressed = false;
        }
    }
}