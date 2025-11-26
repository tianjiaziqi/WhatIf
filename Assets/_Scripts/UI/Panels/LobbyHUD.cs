using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WhatIf;

public class LobbyHUD : MonoBehaviour
{
    public Text joinCodeText;
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
    }
}
