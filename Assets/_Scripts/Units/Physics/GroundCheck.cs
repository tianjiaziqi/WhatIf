using UnityEngine;

namespace WhatIf
{
    public class GroundCheck : MonoBehaviour
    {
        [Header("Grounded Check Settings")] [SerializeField]
        private Transform groundCheck;

         [SerializeField]
        private float groundCheckRadius = 0.25f;

        [SerializeField]
        private LayerMask groundLayer;
        
        public bool IsGrounded { get; private set; }
        

        private void Update()
        {
            CheckGroundedStatus();
        }
        
        private void CheckGroundedStatus()
        {
            if (groundCheck == null)
            {
                Debug.LogError("GroundCheck transform has not been assigned on " + gameObject.name);
                IsGrounded = false;
                return;
            }
            
            IsGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        }
    }
}