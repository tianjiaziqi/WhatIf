using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhatIf
{
    public class ObstacleIdle : StateBaseNoParam
    {
        public override string StateName => "ObstacleIdle";
        private ObstacleUnit _unit;
        
        protected override void OnEnterNoParam(StateBase oldState)
        {
            if (ownerUnit is ObstacleUnit unit)
            {
                _unit = unit;
            }
            _unit.agent.isStopped = true;
            _unit.agent.ResetPath();
            _unit.animator.SetBool("IsMoving", false);
        }
        public override void OnUpdate()
        {
            if (_unit.playerInRange)
            {
                ownerFsm.ChangeState<ObstacleMove>();
                return;
            }
            float sqrDistToGuard = (_unit.guardPosition - _unit.transform.position).sqrMagnitude;
            if (sqrDistToGuard > (_unit.guardStopDistance * _unit.guardStopDistance))
            {
                ownerFsm.ChangeState<ObstacleReturnToGuard>();
                return;
            }
        }

        public override void OnExit()
        {
            
        }
    }
}
