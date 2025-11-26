using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhatIf
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        private Dictionary<string, PanelBase> Panels = new();
        public PanelLists _panelLists;
        [SerializeField] private Transform canvasTransform;

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

        private void Start()
        {
            canvasTransform = GameObject.Find("Canvas").transform;
            InitializePanels();
        }

        private void InitializePanels()
        {
            Panels.Clear();
            foreach (var prefabs in _panelLists.Panels)
            {
                var panelObj = Instantiate(prefabs, canvasTransform);
                var panel = panelObj.GetComponent<PanelBase>();
                panel.Hide();
                Panels.Add(panel.PanelName, panel);
            }
        }

        public PanelBase GetPanel(string panelName)
        {
            if (Panels.TryGetValue(panelName, out var panel))
            {
                return panel;
            }
            else
            {
                Debug.LogError($"Panel {panelName} not found");
                return null;
            }
        }

        public void ShowPanel(string panelName)
        {
            if (Panels.TryGetValue(panelName, out var panel))
            {
                panel.Show();
            }
            else
            {
                Debug.LogError($"Panel {panelName} not found");
            }
        }

        public void HidePanel(string panelName)
        {
            if (Panels.TryGetValue(panelName, out var panel))
            {
                panel.Hide();
            }
            else
            {
                Debug.LogError($"Panel {panelName} not found");
            }
        }

        public void ShowErrorMessage(string message)
        {
            if (!Panels.ContainsKey("Error"))
            {
                Debug.LogWarning("Error panel not found");
            }
            if (GetPanel("Error") is ErrorPanel errorPanel)
            {
                errorPanel.SetErrorText(message);
                errorPanel.Show();
            }
        }
    }
}
