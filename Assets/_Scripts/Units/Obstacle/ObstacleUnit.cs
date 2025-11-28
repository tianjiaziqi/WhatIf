using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace WhatIf
{
    public class ObstacleUnit : Unit
    {
        public BoxCollider selfCollider;
        public EngageArea engageArea;
        public bool playerInRange;
        public Transform targetTransform;
        public Vector3 guardPosition;
        public NavMeshAgent agent;
        public float followSopDistance = 1.5f;
        public float guardStopDistance = 0.5f;
        public float attackRange = 2f;
        public float attackCooldown = 2f;
        private float _lastAttackTime = -999f;


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (rb != null)
            {
                rb.isKinematic = true; 
            }

            if (!IsServer)
            {
                if (agent != null) 
                {
                    agent.enabled = false; 
                }
                
                currentHp.OnValueChanged += OnHpChanged;
                if (currentHp.Value <= 0) Ondeath();
                return;
            }
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                guardPosition = hit.position;
            }
            else
            {
                guardPosition = transform.position;
                Debug.LogWarning($"Can't sample position using current.");
            }

            
            agent.enabled = false; 
            transform.position = guardPosition;
            transform.rotation = Quaternion.identity;
            agent.enabled = true;
            agent.Warp(guardPosition);
            
    
            if (fsm == null)
            {
                Debug.LogError($"FSM is null for {name}");
                return;
            }
    
            var idleState = fsm.GetState<ObstacleIdle>();
            if (idleState == null)
            {
                Debug.LogError($"IdleState not found in FSM for {name}. ");
                return;
            }
            fsm.ChangeState<ObstacleIdle>();

            
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
            
            currentHp.OnValueChanged += OnHpChanged;
            if (currentHp.Value <= 0)
            {
                Ondeath();
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            currentHp.OnValueChanged -= OnHpChanged;
        }
        
        private void OnHpChanged(double oldHp, double newHp)
        {
            if (newHp <= 0 && oldHp > 0)
            {
                // 无论是 Server 还是 Client，血量归零都执行死亡逻辑（播放动画、切换状态）
                Ondeath();
            }
        }

        protected override void Start()
        {
            
        }

        public bool CanAttack()
        {
            return Time.time >= _lastAttackTime + attackCooldown;
        }

        protected override void Ondeath()
        {
            base.Ondeath();
            fsm.ChangeState<ObstacleDie>();
        }

        

        public override bool IsDead()
        {
            return currentHp.Value <= 0;
        }

        public override void OnAttack()
        {
            if (IsDead()) return;
            if (attackArea == null)
            {
                Debug.LogError($"Can not find attack area component for {name}");
                return;
            }

            Collider[] hitColliders = new Collider[3];
            Vector3 center = attackArea.transform.TransformPoint(attackArea.center);
            Vector3 halfExtents = Vector3.Scale(attackArea.size, attackArea.transform.lossyScale) * 0.5f;
            Quaternion orientation = attackArea.transform.rotation;
            int hitCount = Physics.OverlapBoxNonAlloc(center, halfExtents, hitColliders, orientation, 1<<LayerMask.NameToLayer("Player"));

            for (int i = 0; i < hitCount; i++)
            {
                if (!hitColliders[i].CompareTag("Player")) continue;
                hitColliders[i].GetComponent<PlayerUnit>().TakeDamage(this, atk);
            }
            
            _lastAttackTime = Time.time;
            fsm.ChangeState<ObstacleIdle>();
        }

        public override void TakeDamage(Unit attacker, double damage)
        {
            if (!IsServer) return;
            if (IsDead()) return;
            currentHp.Value -= damage;
            currentHp.Value = System.Math.Max(currentHp.Value, 0); // Make sure hp never below 0
            Debug.Log($"{name} got {damage} damage from {attacker.name}, current hp: {currentHp}");
            
            // check if dead
            if (currentHp.Value <= 0)
            {
                Ondeath();
                return;
            }

            if (fsm.CurrentState.CanInterrupt())
            {
                fsm.ChangeState<ObstacleHit>();
            }
            
        }

        public void OnHitAnimationFinish()
        {
            if(IsDead()) return;
            if(fsm != null)
                fsm.ChangeState<ObstacleIdle>();
        }

        protected override void Update()
        {
            if (IsServer)
            {
                base.Update();
            }
            
        }
    }
}
