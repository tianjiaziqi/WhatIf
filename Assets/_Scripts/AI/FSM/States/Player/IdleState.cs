using UnityEngine;

namespace WhatIf
{
    public class IdleState : StateBaseNoParam
    {
        public override string StateName => "Idle";

        private bool _wasGrounded;

        protected override void OnEnterNoParam(StateBase oldState)
        {
            // set horizontal velocity to 0
            var v = ownerUnit.rb.velocity;
            ownerUnit.rb.velocity = new Vector3(0f, v.y, 0f);

            ownerUnit.animator.SetFloat("Speed", 0f);
            _wasGrounded = ownerUnit.isGrounded;
        }

        public override void OnUpdate()
        {
            bool grounded = ownerUnit.isGrounded; 

            
            if (_wasGrounded && !grounded)
            {
                //OwnerFSM.ChangeState<JumpState, AirborneStateParam>(AirborneStateParam.FromFall());
                _wasGrounded = grounded;
                return;
            }
            _wasGrounded = grounded;

            if (ownerUnit.unitType == EUnitType.Player)
            {
                HandleInput(grounded);
            }

            
            if (grounded)
            {
                ownerUnit.PlanarVelocity = Vector3.zero;
            }
        }

        public override void OnExit() { }

        private void HandleInput(bool grounded)
        {
            if (!ownerUnit.ShouldRespondToInput())
                return;
            
            if (InputManager.Instance.AttackPressed)
            {
                ownerFsm.ChangeState<AttackState>();
                return;
            }
            
            if (InputManager.Instance.HasMovementInput)
            {
                if (InputManager.Instance.RunHeld){}
                //TODO: run state
                else
                {
                    ownerFsm.ChangeState<MoveState>();
                    return;
                }
            }
            
            
            if (InputManager.Instance.JumpPressed && _wasGrounded)
            {
                ownerFsm.ChangeState<AirState, AirborneStateParam>(AirborneStateParam.FromJump());
            }
            
        }
    }
}
