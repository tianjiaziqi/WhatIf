using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WhatIf;

public class StartPanel : MonoBehaviour
{
    public Button startButton;
    public Button joinButton;
    public Button creditsButton;
    public Button exitButton;

    private void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            Host();
        });
        joinButton.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel("Join");
        });
        creditsButton.onClick.AddListener(() =>
        {
            
        });
        exitButton.onClick.AddListener(Application.Quit);
    }

    private async void Host()
    {
        UIManager.Instance.ShowPanel("Loading");
        var alloc = await RelayService.Instance.CreateAllocationAsync(1);
        GameManager.Instance.joinCode = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);
        RelayServerData data = new RelayServerData(alloc, "dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(data);
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
    }
}
