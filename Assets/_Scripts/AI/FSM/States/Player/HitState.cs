using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhatIf;

public class HitState : StateBaseNoParam
{
    public override string StateName => "Hit";
    
    protected override void OnEnterNoParam(StateBase oldState)
    {
        ownerUnit.rb.velocity = new Vector3(0f, ownerUnit.rb.velocity.y, 0f);
        ownerUnit.PlanarVelocity = Vector3.zero;
        ownerUnit.animator.SetTrigger("Hit");
    }
    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
    }

    
}
