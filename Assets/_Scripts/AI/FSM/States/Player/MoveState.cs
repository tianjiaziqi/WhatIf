using System;
using UnityEngine;

namespace WhatIf
{
    [Serializable]
    public class MoveState : GroundState
    {
        public override string StateName => "Move";


        

        protected override void OnGroundUpdate()
        {
            if(_unit.networkInput.magnitude <= 0.1f && ownerUnit.ShouldRespondToInput())
            {
                ownerFsm.ChangeState<IdleState>();
                return;
            }
            HandleMovement();
        }

        public override void OnExit() { }

        void HandleMovement()
        {
            var input = _unit.networkInput;
            if (ownerUnit.isGrounded)
            {
                var targetVelocity = new Vector3(input.x, 0f, input.y) * ownerUnit.walkSpeed;
                ownerUnit.PlanarVelocity = targetVelocity;
                if (targetVelocity.magnitude > 0.1f)
                {
                    ownerUnit.transform.rotation = Quaternion.LookRotation(targetVelocity.normalized);
                }
                ownerUnit.animator.SetFloat("Speed", ownerUnit.PlanarVelocity.magnitude/ownerUnit.walkSpeed*0.5f);
            }
            
        }
    }
} 