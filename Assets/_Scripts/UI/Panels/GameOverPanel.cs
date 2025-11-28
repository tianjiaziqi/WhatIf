using Unity.Netcode;
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
        restartButton.onClick.AddListener(OnRestartClicked);
    }
    
    public void OnRestartClicked()
    {
        
        if (GameCycleManager.Instance != null)
        {
            GameCycleManager.Instance.OnRestartClicked();
        }
        else
        {
            Debug.LogError("GameCycleManager instance not found!");
        }
    }
    
    
}
