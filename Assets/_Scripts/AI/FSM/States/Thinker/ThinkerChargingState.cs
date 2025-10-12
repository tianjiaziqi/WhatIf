using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhatIf;

public class ThinkerChargingState : StateBaseNoParam
{
    public override string StateName => "ThinkerCharging";
    private float _chargeTime;
    private ThinkerUnit _unit;
    protected override void OnEnterNoParam(StateBase oldState)
    {
        if (ownerUnit is ThinkerUnit unit)
        {
            _unit = unit;
        }
        _chargeTime = 0f;
        ownerUnit.animator.SetBool("Charging", true);
    }
    public override void OnUpdate()
    {
        CheckPlayer();
        CountChargeTime();
    }

    public override void OnExit()
    {
        ownerUnit.animator.SetBool("Charging", false);
    }

    private void CheckPlayer()
    {
        if (!_unit.playerInRange)
        {
            ownerFsm.ChangeState<ThinkerFadingState>();
        }
    }
    
    private void CountChargeTime()
    {
        _chargeTime += Time.deltaTime;
        if (_chargeTime > _unit.chargeTime)
        {
            ownerFsm.ChangeState<ThinkerAttackState>();       
        }
    }

    
}
