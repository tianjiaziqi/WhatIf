using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace WhatIf
{
    public class ThinkerUnit : Unit
    {
        public BoxCollider selfCollider;
        public SkinnedMeshRenderer meshRenderer;
        public EngageArea engageArea;
        public bool playerInRange;
        
        [SerializeField] private float basicChargeTime;
        public float chargeTime => basicChargeTime * chargeTimeScale;
        public Transform targetTransform;
        public float respawnTime;
        public float projectileSpeed;
        public float attackRange;
        public float chargeTimeScale;
        public float attackCDScale;
        public float projectileSpeedScale;
        public float fadeTime;
        public GameObject projectilePrefab;
        public bool wasShoot;
        public float fadeIntervalUpperBound;
        public float fadeIntervalLowerBound;
        public float nextFadeTime;


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(!IsServer) return;
            if (fsm == null)
            {
                Debug.LogError($"FSM is null for {name}");
                return;
            }
            
            var idleState = fsm.GetState<ThinkerIdleState>();
            if (idleState == null)
            {
                Debug.LogError($"IdleState not found in FSM for {name}. ");
                return;
            }
            
            
            fsm.ChangeState<ThinkerIdleState>();
            engageArea.OnEnter.AddListener((other) =>
            {
                if (other.CompareTag("Player"))
                {
                    targetTransform = other.transform;
                    playerInRange = true;
                }
            });
            engageArea.OnExit.AddListener((other) =>
            {
                if (other.CompareTag("Player"))
                {
                    targetTransform = null;
                    playerInRange = false;
                }
            });
        }

        protected override void Update()
        {
            if (IsServer)
            {
                base.Update();
            }
            
        }

        public void InvisibleAndInvincible()
        {
            selfCollider.enabled = false;
            meshRenderer.enabled = false;
        }
        public void VisibleAndVincible()
        {
            selfCollider.enabled = true;
            meshRenderer.enabled = true;
        }

        public void Respawn()
        {
            currentHp.Value = maxHp;
        }

        protected override void Ondeath()
        {
            base.Ondeath();
            fsm.ChangeState<ThinkerSpawnState>();
        }

        public override void OnAttack()
        {
            if(targetTransform != null){
                GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
                projectile.GetComponent<ProjectileMovement>().Shoot(this, atk, targetTransform);
            }
            wasShoot = true;
        }

        public override bool IsDead()
        {
            return fsm.CurrentState.StateName == "ThinkerSpawn";
        }

        public override void TakeDamage(Unit attacker, double damage)
        {
            if (!IsServer) return;
            if (IsDead() || fsm.CurrentState.StateName == "ThinkerIdle") return;
            currentHp.Value -= damage;
            currentHp.Value = System.Math.Max(currentHp.Value, 0); // Make sure hp never below 0
            Debug.Log($"{name} got {damage} damage from {attacker.name}, current hp: {currentHp}");
            
            // check if dead
            if (currentHp.Value <= 0)
            {
                Ondeath();
            }
        }
        
    }
    
    
}
