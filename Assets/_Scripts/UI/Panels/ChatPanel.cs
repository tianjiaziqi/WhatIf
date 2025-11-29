using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WhatIf
{
    public class ChatPanel : PanelBase
    {
        public override string PanelName => "Chat";
        
        [SerializeField] private InputField inputField;
        [SerializeField] private Text chatText;

        
        [SerializeField] private int maxMessages = 10;
        //[SerializeField] private float autoHideDuration = 5f;
        
        private List<string> _messages = new List<string>();

        private void Start()
        {
            inputField.onEndEdit.AddListener(OnSubmit);
            UpdateUIText();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!inputField.isFocused)
                {
                    inputField.ActivateInputField();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                inputField.DeactivateInputField();
                if (EventSystem.current != null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
                Hide();
            }
        }

        public void AddMessage(string newMessage)
        {
            _messages.Add(newMessage);
            
            if (_messages.Count > maxMessages)
            {
                _messages.RemoveAt(0);
            }
            
            UpdateUIText();
        }

        private void UpdateUIText()
        {
            chatText.text = string.Join("\n", _messages);
        }

        private void OnSubmit(string text)
        {
            if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter)) 
            {
                return; 
            }
            if (string.IsNullOrWhiteSpace(text)) return;
            
            if (ChatNetworkManager.Instance != null)
            {
                ChatNetworkManager.Instance.SendMessageToAll(text);
            }
            else
            {
                Debug.LogError("ChatNetworkManager not found!");
            }
            
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }
}