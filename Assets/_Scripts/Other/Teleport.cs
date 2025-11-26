using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WhatIf;

public class Teleport : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerUnit playerUnit))
        {
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
