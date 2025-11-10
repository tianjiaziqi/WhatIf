using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using WhatIf;

public class DieState : StateBaseNoParam
{
    public override string StateName => "Die";
    
    protected override void OnEnterNoParam(StateBase oldState)
    {
        ownerUnit.rb.velocity = Vector3.zero;
        ownerUnit.animator.SetBool("IsDead", true);
        if (ownerUnit.GetComponent<Collider>() != null)
        {
            ownerUnit.GetComponent<Collider>().enabled = false;       
        }
        ownerUnit.rb.isKinematic = true;
    }
    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }

    
}
