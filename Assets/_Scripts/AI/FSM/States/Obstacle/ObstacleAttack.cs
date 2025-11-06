using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhatIf;

public class ObstacleAttack : StateBaseNoParam
{
    public override string StateName => "ObstacleAttack";
    private ObstacleUnit _unit;
    
    protected override void OnEnterNoParam(StateBase oldState)
    {
        if (ownerUnit is ObstacleUnit unit)
        {
            _unit = unit;
        }

        _unit.agent.isStopped = true;
        _unit.transform.rotation = Quaternion.LookRotation(_unit.targetTransform.position - _unit.transform.position);
        _unit.animator.SetTrigger("Attack");
    }
    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }

    
}
