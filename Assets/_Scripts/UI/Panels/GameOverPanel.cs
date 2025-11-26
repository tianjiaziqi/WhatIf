using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WhatIf;

public class GameOverPanel : PanelBase
{
    [SerializeField] private Button restartButton;
    public override string PanelName => "GameOver";

    private void Start()
    {
        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }
}
