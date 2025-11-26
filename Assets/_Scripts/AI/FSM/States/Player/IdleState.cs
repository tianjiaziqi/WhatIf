using UnityEngine;

namespace WhatIf
{
    public class IdleState : GroundState
    {
        public override string StateName => "Idle";

        protected override void OnEnterNoParam(StateBase oldState)
        {
            base.OnEnterNoParam(oldState);
            _unit.rb.velocity = new Vector3(0f, ownerUnit.rb.velocity.y, 0f);
            _unit.animator.SetFloat("Speed", 0f);

            if (_unit.isGrounded)
            {
                _unit.PlanarVelocity = Vector3.zero;
            }
        }

        protected override void OnGroundUpdate()
        {
            if (_unit.networkInput.magnitude >= 0.1f && ownerUnit.ShouldRespondToInput())
            {
                ownerFsm.ChangeState<MoveState>();
                return;
            }
        }

        public override void OnExit() { }

        
    }
}
