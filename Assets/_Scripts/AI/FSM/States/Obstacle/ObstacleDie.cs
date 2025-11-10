using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhatIf
{
    public class ObstacleDie : StateBaseNoParam
    {
        public override string StateName => "ObstacleDie";
        private ObstacleUnit _unit;
        protected override void OnEnterNoParam(StateBase oldState)
        {
            if (ownerUnit is ObstacleUnit unit)
            {
                _unit = unit;
            }
            _unit.agent.isStopped = true;
            _unit.animator.SetBool("IsDead", true);
            if (_unit.GetComponent<Collider>() != null)
            {
                _unit.GetComponent<Collider>().enabled = false;      
            }
            GameObject.Destroy(_unit.gameObject, 5f);
        }
        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }

        
    }
}
