using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhatIf
{
    [Serializable]
    public class AirState : StateBase<AirborneStateParam>
    {

        public override string StateName => "Airborne";
        private bool _hasLanded;
        private float _initialY;
        private bool _hasStartedJump;

        protected override void OnEnter(AirborneStateParam param, StateBase oldState)
        {
            _initialY = ownerUnit.transform.position.y;
            if (param.EntryType == EAirborneEnterType.FromJump)
            {
                ownerUnit.animator.SetTrigger("Jump");
                PerformJump();
            }

            ownerUnit.animator.SetBool("Airborne", true);
            _hasLanded = false;
            _hasStartedJump = false;
        }

        public override void OnUpdate()
        {
            if (!ownerUnit.isGrounded) _hasStartedJump = true;
            CheckLanding();
        }

        public override void OnExit()
        {
            ownerUnit.animator.SetTrigger("Landing");
            ownerUnit.animator.SetBool("Airborne", false);
            _hasLanded = false;
            _hasStartedJump = false;
        }

        private void PerformJump()
        {
            float g = -Physics.gravity.y;
            float v0 = Mathf.Sqrt(2f * g * ownerUnit.jumpHeight);
            Vector3 v = ownerUnit.rb.velocity;
            v.y = v0;
            ownerUnit.rb.velocity = v;
        }

        private void CheckLanding()
        {
            if (ownerUnit.isGrounded && !_hasLanded && _hasStartedJump)
            {
                _hasLanded = true;
                
                if (ownerUnit.unitType == EUnitType.Player)
                {
                    bool hasInput = InputManager.Instance.HasMovementInput;

                    if (hasInput)
                    {
                        ownerFsm.ChangeState<MoveState>();
                    }
                    else
                    {
                        ownerFsm.ChangeState<IdleState>();
                    }
                }
                else
                {
                    ownerFsm.ChangeState<IdleState>();
                }
            }
        }
        
    }
}

