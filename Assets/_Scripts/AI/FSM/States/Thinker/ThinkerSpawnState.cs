using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhatIf;

public class ThinkerSpawnState : StateBaseNoParam
{
    public override string StateName => "ThinkerSpawn";
    private float _spawnTimer;
    private ThinkerUnit _unit;
    protected override void OnEnterNoParam(StateBase oldState)
    {
        if (ownerUnit is ThinkerUnit unit)
        {
            _unit = unit;
        }
        _spawnTimer = 0f;
        ownerUnit.animator.SetBool("Dead", true);
    }
    public override void OnUpdate()
    {
        CheckSpawnTimer();
    }

    public override void OnExit()
    {
        ownerUnit.animator.SetBool("Dead", false);
    }

    public void CheckSpawnTimer()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer > _unit.respawnTime)
        {
            _unit.Respawn();
            ownerFsm.ChangeState<ThinkerChargingState>();
        }
    }

    
}
