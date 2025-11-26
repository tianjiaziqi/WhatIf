using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.UI;

namespace WhatIf
{
    public class JoinPanel : PanelBase
    {
        public override string PanelName => "Join";
        public Button closeButton;
        public InputField codeInput;
        public Button joinButton;
        private string _joinCode;

        private void Start()
        {
            closeButton.onClick.AddListener(()=>
            {
                Hide();
            });
            
            codeInput.onValueChanged.AddListener((code) =>
            {
                _joinCode = code;
            });
            joinButton.onClick.AddListener(() =>
            {
                Join(_joinCode);
            });

            
        }
        private async void Join(string joinCode)
        {
            UIManager.Instance.ShowPanel("Loading");
            try
            {
                var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.joinCode = joinCode;
                }
                RelayServerData serverData = new RelayServerData(allocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);
                NetworkManager.Singleton.StartClient();
            }
            catch (RelayServiceException e)
            {
                UIManager.Instance.ShowErrorMessage(e.Message);
            }
            UIManager.Instance.HidePanel("Loading");
        }
    
    
    }
    
    
}
