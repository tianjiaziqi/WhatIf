using System;
using UnityEngine;

namespace WhatIf
{
    [Serializable]
    public class MoveState : StateBaseNoParam
    {
        public override string StateName => "Move";
        
        private bool wasGrounded;

        protected override void OnEnterNoParam(StateBase oldState)
        {
            wasGrounded = true;
        }

        public override void OnUpdate()
        {
            if (wasGrounded && !ownerUnit.isGrounded)
            {
                ownerFsm.ChangeState<AirState, AirborneStateParam>(AirborneStateParam.FromFall());
            }
            wasGrounded = ownerUnit.isGrounded;

            //HandleInput();
            HandleJumpInput();
            HandleMovement();
        }

        public override void OnExit() { }

        

        void HandleInput()
        {
            if (!ownerUnit.ShouldRespondToInput())
                return;
            if (InputManager.Instance.AttackPressed && ownerUnit.isGrounded)
            {
                ownerFsm.ChangeState<AttackState>();
            }
        }

        void HandleJumpInput()
        {
            if (!ownerUnit.ShouldRespondToInput())
                return;
        
            if (InputManager.Instance.JumpPressed && ownerUnit.isGrounded)
            {
                ownerFsm.ChangeState<AirState, AirborneStateParam>(AirborneStateParam.FromJump());
            }
            
            
        }

        void HandleMovement()
        {
            Vector2 inputVector = Vector2.zero;
            
            if (ownerUnit.ShouldRespondToInput())
            {
                inputVector = InputManager.Instance.MovementInput;
            }
            

            if (inputVector.magnitude < 0.1f)
            {
                if (ownerUnit.PlanarVelocity.magnitude > 0.1f)
                {
                    ownerUnit.PlanarVelocity = Vector3.MoveTowards(
                        ownerUnit.PlanarVelocity,
                        Vector3.zero,
                        ownerUnit.runAcceleration * Time.fixedDeltaTime
                    );
                    ownerUnit.animator.SetFloat("Speed", ownerUnit.PlanarVelocity.magnitude/ownerUnit.walkSpeed*0.5f);
                }
                else if (ownerUnit.unitType == EUnitType.Player)
                {
                    ownerFsm.ChangeState<IdleState>();
                }
                return;
            }
            float maxSpeed =  ownerUnit.walkSpeed;
            
            
            if (ownerUnit.isGrounded)
            {
                Vector3 targetVelocity = new Vector3(inputVector.x, 0 , inputVector.y) * maxSpeed;
                ownerUnit.PlanarVelocity = Vector3.MoveTowards(
                    ownerUnit.PlanarVelocity,
                    targetVelocity,
                    ownerUnit.runAcceleration * Time.fixedDeltaTime
                );
                ownerUnit.transform.rotation = Quaternion.LookRotation(ownerUnit.PlanarVelocity);
                ownerUnit.animator.SetFloat("Speed", ownerUnit.PlanarVelocity.magnitude/ownerUnit.walkSpeed*0.5f);
            }
        }
    }
} 