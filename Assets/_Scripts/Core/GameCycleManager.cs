using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace WhatIf
{
    public class GameCycleManager : NetworkBehaviour
    {
        public static GameCycleManager Instance;
        
        private NetworkVariable<int> _deadPlayerCount = new NetworkVariable<int>(0);
        

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                _deadPlayerCount.Value = 0;
            }
            
            _deadPlayerCount.OnValueChanged += OnDeathCountChanged;
        }
        
        public override void OnNetworkDespawn()
        {
             _deadPlayerCount.OnValueChanged -= OnDeathCountChanged;
        }
        
        public void ReportPlayerDeath()
        {
            if (IsServer)
            {
                _deadPlayerCount.Value++;
                Debug.Log($"Player died. Total dead: {_deadPlayerCount.Value}");
            }
        }

        private void OnDeathCountChanged(int previous, int current)
        {
            if (current >= 2)
            {
                ShowGameOverUI();
            }
        }

        private void ShowGameOverUI()
        {
            Debug.Log("Game Over! Showing UI.");
            UIManager.Instance.ShowPanel("GameOver");
        }
        
        public void OnRestartClicked()
        {
            RestartGameServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void RestartGameServerRpc()
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }
}