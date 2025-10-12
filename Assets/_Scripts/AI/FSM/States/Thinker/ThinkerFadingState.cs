using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhatIf;

public class ThinkerFadingState : StateBaseNoParam
{
    public override string StateName => "ThinkerFading";
    private ThinkerUnit _unit;
    private float _fadeTimer;
    protected override void OnEnterNoParam(StateBase oldState)
    {
        if (ownerUnit is ThinkerUnit unit)
        {
            _unit = unit;
        }
        _unit.animator.SetBool("Fading", true);
    }
    public override void OnUpdate()
    {
        CheckFadeTime();
    }

    public override void OnExit()
    {
        _fadeTimer = 0f;
        _unit.animator.SetBool("Fading", false);
    }

    private void CheckFadeTime()
    {
        _fadeTimer += Time.deltaTime;
        if (_fadeTimer > _unit.fadeTime)
        {
            ownerFsm.ChangeState<ThinkerIdleState>();
        }
    }

    
}
