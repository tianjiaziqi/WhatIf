using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WhatIf
{
    public class StartPanelLobby : PanelBase
    {
        public override string PanelName => "Start";
        public Button closeButton;
        public Button startButton;
        public Text contentText;

        private void Start()
        {
            closeButton.onClick.AddListener(() =>
            {
                Hide();
            });
            startButton.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }

        public override void Show()
        {
            CheckCanStart();
            UpdateTextInfo();
            base.Show();
        }

        private void UpdateTextInfo()
        {
            int count = RoomManager.Instance.playerCount.Value;
            if (count < 2)
            {
                contentText.text = "Waiting for more players";
                return;
            }
            contentText.text = $"You're going to start a game with {count} Players";
        }

        private void CheckCanStart()
        {
            if (RoomManager.Instance.playerCount.Value < 2)
            {
                startButton.interactable = false;
                startButton.gameObject.SetActive(false);
            }
            else
            {
                startButton.interactable = true;
                startButton.gameObject.SetActive(true);
            }
        }
    }
}
