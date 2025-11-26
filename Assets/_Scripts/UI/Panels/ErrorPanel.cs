using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WhatIf
{
    public class ErrorPanel : PanelBase
    {
        public override string PanelName => "Error";
        public Text errorText;
        public Button closeButton;

        private void Start()
        {
            closeButton.onClick.AddListener(() =>
            {
                Hide();
            });
        }

        public void SetErrorText(string text)
        {
            errorText.text = text;
        }
    }
}
