using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WhatIf
{
    public class PlayerUnit : Unit
    {
        [HideInInspector] public Vector2 networkInput;
        [HideInInspector] public bool networkJump;
        [HideInInspector] public bool networkAttack;


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                string currentScene = SceneManager.GetActiveScene().name;
                if (currentScene == "GameScene" || currentScene == "LobbyScene")
                {
                    BindCamera();
                }
            }

            if (IsServer) NetworkManager.Singleton.SceneManager.OnLoadComplete += OnSceneLoaded;

        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnSceneLoaded;
            }
        }



        public override void OnAttack()
        {
            if (IsDead()) return;
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
            int hitCount = Physics.OverlapBoxNonAlloc(center, halfExtents, hitColliders, orientation,
                1 << LayerMask.NameToLayer("Enemy"));

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
            if (IsDead()) return;
            fsm.ChangeState<IdleState>();
        }

        protected override void Ondeath()
        {
            base.Ondeath();
            fsm.ChangeState<DieState>();
            UIManager.Instance.ShowPanel("GameOver");
        }

        public void OnHitAnimationComplete()
        {
            if (IsDead()) return;
            fsm.ChangeState<IdleState>();
        }

        public override void TakeDamage(Unit attacker, double damage)
        {
            base.TakeDamage(attacker, damage);
            if (!IsDead())
            {
                fsm.ChangeState<HitState>();
            }
        }

        public override bool IsDead()
        {
            return currentHp.Value <= 0;
        }

        protected override void Update()
        {
            if (IsOwner)
            {
                if (InputManager.Instance.JumpPressed)
                {
                    animator.SetTrigger("Jump");
                }

                UpdateInputServerRpc(InputManager.Instance.MovementInput, InputManager.Instance.JumpPressed,
                    InputManager.Instance.AttackPressed);
            }

            if (IsServer)
            {
                base.Update();
            }
        }

        [ServerRpc]
        private void UpdateInputServerRpc(Vector2 input, bool jump, bool attack)
        {
            networkInput = input;
            if (jump) networkJump = true;
            if (attack) networkAttack = true;
        }

        public void PerformJump()
        {
            float g = -Physics.gravity.y;
            float v0 = Mathf.Sqrt(2f * g * jumpHeight);
            Vector3 v = rb.velocity;
            v.y = v0;
            rb.velocity = v;
        }

        private void OnSceneLoaded(ulong clientId, string sceneName, LoadSceneMode mode)
        {

            if (IsServer && (sceneName == "GameScene" || sceneName == "LobbyScene"))
            {
                if (clientId == OwnerClientId)
                {
                    UpdatePlayerPosServer(clientId);
                }

            }

        }


        private void BindCamera()
        {
            if (Camera.main != null)
            {
                var camFollow = Camera.main.GetComponent<CameraFollow>();
                if (camFollow != null)
                {
                    camFollow.Target = transform;
                }
            }
            else
            {
                Debug.LogWarning("Main camera not found");
            }
        }
        
        private void UpdatePlayerPosServer(ulong clientId)
        {
            int index = (int)clientId % DataHolder.Instance.playerPositions.Count;
            var spawnPoint = DataHolder.Instance.playerPositions[index].transform;
            var targetPos = spawnPoint.position;
            var targetRot = spawnPoint.rotation;

            if (TryGetComponent<NetworkTransform>(out var netTransform))
            {
                netTransform.Teleport(targetPos, targetRot, transform.localScale);
            }
            else
            {
                transform.position = targetPos;
                transform.rotation = targetRot;
            }

            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                PlanarVelocity = Vector3.zero;
            }

            ClientRpcParams rpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams { TargetClientIds = new[] { clientId } }
            };
            ForceCameraSnapClientRpc(rpcParams);
        }




        [ClientRpc]
        private void ForceCameraSnapClientRpc(ClientRpcParams clientRpcParams = default)
        {
            if (IsOwner)
            {
                BindCamera();

                if (Camera.main != null)
                {
                    var cam = Camera.main.GetComponent<CameraFollow>();
                    if (cam != null)
                    {
                        cam.Target = transform;
                    }
                }

                if (DataHolder.Instance.loadingPanel != null)
                    DataHolder.Instance.loadingPanel.SetActive(false);
            }
        }


    }

}
    

