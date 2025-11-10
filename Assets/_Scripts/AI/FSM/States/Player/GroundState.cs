using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhatIf;

public abstract class GroundState : StateBaseNoParam
{
    public sealed override void OnUpdate()
    {
        
        if (InputManager.Instance.JumpPressed && ownerUnit.ShouldRespondToInput())
        {
            ownerFsm.ChangeState<AirState, AirborneStateParam>(AirborneStateParam.FromJump());
            return;
        }
        if (!ownerUnit.isGrounded)
        {
            ownerFsm.ChangeState<AirState, AirborneStateParam>(AirborneStateParam.FromFall());
            return;
        }

        

        if (InputManager.Instance.AttackPressed && ownerUnit.ShouldRespondToInput())
        {
            ownerFsm.ChangeState<AttackState>();
            return;
        }
        OnGroundUpdate();
    }

    protected abstract void OnGroundUpdate();
}
