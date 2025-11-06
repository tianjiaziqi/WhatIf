using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhatIf
{
    public class ObstacleReturnToGuard : StateBaseNoParam
    {
        public override string StateName => "ObstacleReturnToGuard";
        private ObstacleUnit _unit;
        
        protected override void OnEnterNoParam(StateBase oldState)
        {
            if (ownerUnit is ObstacleUnit unit)
            {
                _unit = unit;
            }
            _unit.agent.isStopped = false;
            _unit.animator.SetBool("IsMoving", true);
            _unit.agent.stoppingDistance = _unit.guardStopDistance;
            _unit.agent.SetDestination(_unit.guardPosition);
        }
        public override void OnUpdate()
        {
            if(_unit.playerInRange)
            {
                ownerFsm.ChangeState<ObstacleMove>();
                return;
            }

            if (_unit.agent.hasPath && _unit.agent.remainingDistance <= _unit.agent.stoppingDistance)
            {
                ownerFsm.ChangeState<ObstacleIdle>();
            }
        }

        public override void OnExit()
        {
            _unit.animator.SetBool("IsMoving", false);
        }

        
    }
}
