using System;
using UnityEngine;
using Unity.Netcode;

namespace WhatIf
{
    public class ChatNetworkManager : NetworkBehaviour
    {
        public static ChatNetworkManager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return)) UIManager.Instance.ShowPanel("Chat");
        }


        public void SendMessageToAll(string message)
        {
            SendChatMessageServerRpc(message);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SendChatMessageServerRpc(string message, ServerRpcParams rpcParams = default)
        {
            ulong senderId = rpcParams.Receive.SenderClientId;
            string formattedMsg = $"Player {senderId}: {message}";
            
            ReceiveMessageClientRpc(formattedMsg);
        }

        [ClientRpc]
        private void ReceiveMessageClientRpc(string message)
        {
            var panel = UIManager.Instance.GetPanel("Chat");
            if (panel is ChatPanel chatPanel)
            {
                chatPanel.AddMessage(message);
            }
        }
    }
}