using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : NetworkBehaviour
{
    public NetworkVariable<int> playerCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField]private Text playerCountText;
    public static RoomManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        playerCount.OnValueChanged += OnPlayerCountChanged;
        
        UpdatePlayerCountUI(playerCount.Value);
        
        
        
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectionChanged;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientConnectionChanged;
            playerCount.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;
        }
    }

    public override void OnNetworkDespawn()
    {
        playerCount.OnValueChanged -= OnPlayerCountChanged;
        if (IsServer && NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectionChanged;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientConnectionChanged;
        }
    }
    
    private void OnClientConnectionChanged(ulong clientId)
    {
        playerCount.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;
    }
    
    private void OnPlayerCountChanged(int oldVal, int newVal)
    {
        UpdatePlayerCountUI(newVal);
    }

    private void UpdatePlayerCountUI(int count)
    {
        if (playerCountText != null)
        {
            playerCountText.text = $"Players: {count}";
        }
    }
}