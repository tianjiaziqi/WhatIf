using Unity.Netcode;
using UnityEngine;
using WhatIf;

public class Teleport : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerUnit playerUnit))
        {
            if (playerUnit.OwnerClientId == NetworkManager.ServerClientId)
            {
                UIManager.Instance.ShowPanel("Start");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerUnit playerUnit))
        {
            if (playerUnit.OwnerClientId == NetworkManager.ServerClientId)
            {
                UIManager.Instance.HidePanel("Start");
            }
        }
    }
}
