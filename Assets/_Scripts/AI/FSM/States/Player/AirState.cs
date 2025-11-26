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
        private PlayerUnit _unit;

        protected override void OnEnter(AirborneStateParam param, StateBase oldState)
        {
            if(ownerUnit is PlayerUnit unit) _unit = unit;
            _initialY = ownerUnit.transform.position.y;

            if (param.EntryType == EAirborneEnterType.FromJump)
            {
                _unit.PerformJump(); 
                
                ownerUnit.animator.SetTrigger("Jump");
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

        

        private void CheckLanding()
        {
            if (ownerUnit.isGrounded && !_hasLanded && _hasStartedJump)
            {
                _hasLanded = true;
                
                if (ownerUnit.unitType == EUnitType.Player)
                {
                    bool hasInput = _unit.networkInput.magnitude >= 0.1f;

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

