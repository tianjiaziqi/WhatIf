using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhatIf;

public class ThinkerAttackState : StateBaseNoParam
{

    
    public override string StateName => "ThinkerAttack";
    private ThinkerUnit _unit;
    protected override void OnEnterNoParam(StateBase oldState)
    {
        if (ownerUnit is ThinkerUnit unit)
        {
            _unit = unit;
        }
        ownerUnit.animator.SetBool("Shoot", true);
    }
    public override void OnUpdate()
    {
        if (_unit.wasShoot)
        {
            ownerFsm.ChangeState<ThinkerChargingState>();       
        }
    }

    public override void OnExit()
    {
        _unit.wasShoot = false;
    }

    
}
