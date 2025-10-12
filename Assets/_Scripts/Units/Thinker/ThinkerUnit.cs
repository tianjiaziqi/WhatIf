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
        protected override void Start()
        {
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
            
            currentHp = maxHp;
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
            base.Update();
            if(Input.GetKeyDown(KeyCode.J))
            {
                TakeDamage(this, 20);
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
            currentHp = maxHp;
        }

        protected override void Ondeath()
        {
            base.Ondeath();
            fsm.ChangeState<ThinkerSpawnState>();
        }

        public void Shooting()
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
            if (IsDead() || fsm.CurrentState.StateName == "ThinkerIdle") return;
            currentHp -= damage;
            currentHp = System.Math.Max(currentHp, 0); // Make sure hp never below 0
            Debug.Log($"{name} got {damage} damage from {attacker.name}, current hp: {currentHp}");
            
            // check if dead
            if (currentHp <= 0)
            {
                Ondeath();
            }
        }
        
    }
    
    
}
