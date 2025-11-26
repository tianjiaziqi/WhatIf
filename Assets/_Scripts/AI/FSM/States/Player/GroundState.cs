using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhatIf;

public abstract class GroundState : StateBaseNoParam
{
    protected PlayerUnit _unit;

    protected override void OnEnterNoParam(StateBase oldState)
    {
        if (ownerUnit is PlayerUnit unit)
        {
            _unit = unit;
        }
    }
    public sealed override void OnUpdate()
    {
        
        if (_unit.networkJump && ownerUnit.ShouldRespondToInput())
        {
            _unit.networkJump = false;
            ownerFsm.ChangeState<AirState, AirborneStateParam>(AirborneStateParam.FromJump());
            return;
        }
        
        if (!_unit.isGrounded)
        {
            ownerFsm.ChangeState<AirState, AirborneStateParam>(AirborneStateParam.FromFall());
            return;
        }
        
        if (_unit.networkAttack && ownerUnit.ShouldRespondToInput())
        {
            _unit.networkAttack = false;
            ownerFsm.ChangeState<AttackState>();
            return;
        }
        OnGroundUpdate();
    }
    
    

    protected abstract void OnGroundUpdate();
}
