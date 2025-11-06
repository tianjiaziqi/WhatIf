using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhatIf;

public class AttackState : StateBaseNoParam
{
    public override string StateName => "Attack";
    
    private PlayerUnit _unit;

    protected override void OnEnterNoParam(StateBase oldState)
    {
        if (ownerUnit is PlayerUnit unit)
        {
            _unit = unit;
        }
        _unit.animator.SetTrigger("Attack");
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }
}
