using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WhatIf;

public class LobbyHUD : MonoBehaviour
{
    public Text joinCodeText;
    
    public Color[] availableColors = new Color[] 
    { 
        Color.red, 
        Color.blue, 
        Color.green, 
        Color.yellow, 
        Color.cyan 
    };
    
    public Button[] colorButtons;
    private void Start()
    {
        if (GameManager.Instance != null)
        {
            joinCodeText.text = "Join Code: " + GameManager.Instance.joinCode;
        }
        else
        {
            Debug.LogError("GameManager is null");
        }
        for (int i = 0; i < colorButtons.Length; i++)
        {
            int index = i;
            colorButtons[i].onClick.AddListener(() => OnColorButtonClicked(index));
            
            if (index < availableColors.Length)
            {
                colorButtons[i].GetComponent<Image>().color = availableColors[index];
            }
        }
    }
    
    private void OnColorButtonClicked(int colorIndex)
    {
        if (colorIndex >= availableColors.Length) return;
        Color selectedColor = availableColors[colorIndex];
        
        var localPlayer = NetworkManager.Singleton.LocalClient?.PlayerObject;
            
        if (localPlayer != null)
        {
            var playerColorScript = localPlayer.GetComponent<PlayerColor>();
            if (playerColorScript != null)
            {
                playerColorScript.RequestColorChangeServerRpc(selectedColor);
            }
        }
        else
        {
            Debug.LogWarning("Local player object not found! Are you connected?");
        }
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
    
}
