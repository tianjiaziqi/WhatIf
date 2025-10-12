using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhatIf;

public class ThinkerIdleState : StateBaseNoParam
{
    public override string StateName => "ThinkerIdle";
    private ThinkerUnit _unit;
    protected override void OnEnterNoParam(StateBase oldState)
    {
        if (ownerUnit is ThinkerUnit unit)
        {
            _unit = unit;
        }
        _unit.InvisibleAndInvincible();
    }
    public override void OnUpdate()
    {
        DetectPlayer();
    }

    public override void OnExit()
    {
        _unit.VisibleAndVincible();
    }

    private void DetectPlayer()
    {
        if (_unit.playerInRange)
        {
            ownerFsm.ChangeState<ThinkerChargingState>();
        }
    }

    
}
