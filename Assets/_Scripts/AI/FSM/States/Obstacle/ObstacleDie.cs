using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
            _unit.rb.velocity = Vector3.zero;
    
           
            if (_unit.IsServer && _unit.TryGetComponent<Unity.Netcode.Components.NetworkTransform>(out var netTransform))
            {
                netTransform.Teleport(_unit.transform.position, _unit.transform.rotation, _unit.transform.localScale);
            }
            _unit.animator.SetBool("IsDead", true);
            if (_unit.GetComponent<Collider>() != null)
            {
                _unit.GetComponent<Collider>().enabled = false;      
            }
            if (_unit.IsServer)
            {
                _unit.StartCoroutine(WaitAndDespawn());
            }
        }
        public override void OnUpdate()
        {
            _unit.rb.velocity = Vector3.zero;
            _unit.PlanarVelocity = Vector3.zero;
        }

        public override void OnExit()
        {
            
        }
        
        private IEnumerator WaitAndDespawn()
        {
            yield return new WaitForSeconds(5f);
            
            if (_unit != null && 
                _unit.TryGetComponent<Unity.Netcode.NetworkObject>(out var netObj) && 
                netObj.IsSpawned) 
            {
                netObj.Despawn();
            }
        }

        
    }
}
