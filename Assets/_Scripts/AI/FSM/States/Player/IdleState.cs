using UnityEngine;

namespace WhatIf
{
    public class IdleState : GroundState
    {
        public override string StateName => "Idle";

        protected override void OnEnterNoParam(StateBase oldState)
        {
            ownerUnit.rb.velocity = new Vector3(0f, ownerUnit.rb.velocity.y, 0f);
            ownerUnit.animator.SetFloat("Speed", 0f);

            if (ownerUnit.isGrounded)
            {
                ownerUnit.PlanarVelocity = Vector3.zero;
            }
        }

        protected override void OnGroundUpdate()
        {
            if (InputManager.Instance.HasMovementInput && ownerUnit.ShouldRespondToInput())
            {
                ownerFsm.ChangeState<MoveState>();
                return;
            }
        }

        public override void OnExit() { }

        
    }
}
