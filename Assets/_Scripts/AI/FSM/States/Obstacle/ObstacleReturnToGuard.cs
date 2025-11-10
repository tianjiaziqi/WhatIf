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
            if(_unit.playerInRange && !_unit.targetTransform.GetComponent<PlayerUnit>().IsDead())
            {
                ownerFsm.ChangeState<ObstacleMove>();
                return;
            }
            float sqrDistToGuard = (_unit.guardPosition - _unit.transform.position).sqrMagnitude;
            float stopSqr = _unit.agent.stoppingDistance * _unit.agent.stoppingDistance;

            if (sqrDistToGuard <= stopSqr)
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
