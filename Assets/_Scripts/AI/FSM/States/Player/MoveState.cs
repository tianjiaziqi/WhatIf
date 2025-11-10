using System;
using UnityEngine;

namespace WhatIf
{
    [Serializable]
    public class MoveState : GroundState
    {
        public override string StateName => "Move";
        

        protected override void OnEnterNoParam(StateBase oldState)
        {
            
        }

        protected override void OnGroundUpdate()
        {
            if(!InputManager.Instance.HasMovementInput && ownerUnit.ShouldRespondToInput())
            {
                ownerFsm.ChangeState<IdleState>();
                return;
            }
            HandleMovement();
        }

        public override void OnExit() { }

        void HandleMovement()
        {
            if (ownerUnit.isGrounded)
            {
                var targetVelocity = new Vector3(InputManager.Instance.MovementInput.x, 0f, InputManager.Instance.MovementInput.y) * ownerUnit.walkSpeed;
                ownerUnit.PlanarVelocity = Vector3.MoveTowards(ownerUnit.PlanarVelocity, targetVelocity, ownerUnit.runAcceleration * Time.fixedDeltaTime);
                if (targetVelocity.magnitude > 0.1f)
                {
                    ownerUnit.transform.rotation = Quaternion.LookRotation(targetVelocity.normalized);
                }
                ownerUnit.animator.SetFloat("Speed", ownerUnit.PlanarVelocity.magnitude/ownerUnit.walkSpeed*0.5f);
            }
            
        }
    }
} 