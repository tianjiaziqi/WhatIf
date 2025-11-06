using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhatIf
{
    public class ObstacleHit : StateBaseNoParam
    {
        public override string StateName => "ObstacleHit";
        private ObstacleUnit _unit;
        
        protected override void OnEnterNoParam(StateBase oldState)
        {
            if (ownerUnit is ObstacleUnit unit)
            {
                _unit = unit;
            }

            _unit.agent.isStopped = true;
            _unit.animator.SetBool("IsMoving", false);
            _unit.animator.SetTrigger("Hit");
        }
        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }

        
    }
}
