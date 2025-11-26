using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhatIf
{
    public abstract class PanelBase : MonoBehaviour
    {
        public abstract string PanelName { get; }
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }

}