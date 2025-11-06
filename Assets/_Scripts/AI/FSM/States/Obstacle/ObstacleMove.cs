using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace WhatIf
{
    public class ObstacleMove : StateBaseNoParam
    {
        public override string StateName => "ObstacleMove";
        private ObstacleUnit _unit;
        private float _moveTimer;
        
        
        protected override void OnEnterNoParam(StateBase oldState)
        {
            if (ownerUnit is ObstacleUnit unit)
            {
                _unit = unit;
            }
            _unit.agent.stoppingDistance = _unit.followSopDistance;
            _unit.agent.isStopped = false;
            SetPlayerDestination();
            _unit.animator.SetBool("IsMoving", true);
            _moveTimer = 0.5f;
        }
        public override void OnUpdate()
        {
            if (!_unit.playerInRange)
            {
                ownerFsm.ChangeState<ObstacleReturnToGuard>();
                return;
            }
            Vector3 offset = _unit.targetTransform.position - _unit.transform.position;
            
            offset.y = 0;
            
            float sqrDistXZ = offset.sqrMagnitude; 
            
            float attackRangeSqr = _unit.attackRange * _unit.attackRange;

            if (sqrDistXZ <= attackRangeSqr)
            {
                _unit.agent.isStopped = true;
                _unit.agent.ResetPath();
                _unit.animator.SetBool("IsMoving", false);
                
                if (_unit.CanAttack())
                {
                    ownerFsm.ChangeState<ObstacleAttack>();
                    return; 
                }
            }
            else
            {
                _unit.agent.isStopped = false;
                _unit.animator.SetBool("IsMoving", true);
                
                _moveTimer -= Time.deltaTime;
                if (_moveTimer <= 0f)
                {
                    SetPlayerDestination();
                    _moveTimer = 1f;    
                }
            }
            _moveTimer -= Time.deltaTime;
            if (_moveTimer <= 0f)
            {
                SetPlayerDestination();
                _moveTimer = 0.5f;    
            }
        }

        public override void OnExit()
        {
            _moveTimer = 0f;
            if (_unit != null && _unit.agent.isOnNavMesh)
            {
                _unit.agent.isStopped = true;
                _unit.agent.ResetPath();
            }
            _unit.animator.SetBool("IsMoving", false);
            _unit = null;
        }
        
        private void SetPlayerDestination()
        {
            if (_unit == null || _unit.targetTransform == null) return;
            
            if (NavMesh.SamplePosition(_unit.targetTransform.position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                
                _unit.agent.SetDestination(hit.position);
            }
            else
            {
                
                _unit.agent.SetDestination(_unit.targetTransform.position);
            }
        }

        
    }
}