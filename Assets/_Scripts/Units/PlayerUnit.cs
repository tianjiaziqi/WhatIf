using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhatIf
{
    public class PlayerUnit : Unit
    {
        public override void OnAttack()
        {
            if (attackArea == null)
            {
                Debug.LogError("BoxCollider attack area is null, cannot perform target detection");
                return;
            }
            
            BoxCollider attackCollider = attackArea;
            
            // Get world space data for detecting area
            Vector3 center = attackCollider.transform.TransformPoint(attackCollider.center);
            Vector3 halfExtents = Vector3.Scale(attackCollider.size, attackCollider.transform.lossyScale) * 0.5f;
            Quaternion orientation = attackCollider.transform.rotation;
            
            // Use OverlapBoxNonAlloc to detect
            const int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            int hitCount = Physics.OverlapBoxNonAlloc(center, halfExtents, hitColliders, orientation, 1<< LayerMask.NameToLayer("Enemy"));
            
            Debug.Log($"Attack area detected {hitCount} colliders");
            
            for (int i = 0; i < hitCount; i++)
            {
                Collider hitCollider = hitColliders[i];
                
                // Only detect colliders that are CapsuleCollider
                if (!(hitCollider.CompareTag("Enemy")))
                {
                    continue;
                }

                // get the target unit from collider
                Unit targetUnit = hitCollider.GetComponent<Unit>();

                if (targetUnit == null)
                {
                    continue;
                }
                targetUnit.TakeDamage(this, atk);
            }
        }

        public void OnAttackAnimationComplete()
        {
            fsm.ChangeState<IdleState>();
        }
    }
}
