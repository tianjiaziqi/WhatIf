using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using WhatIf;

public class GameInitializationAsync : MonoBehaviour
{
    public GameObject cover;
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        cover.SetActive(false);
    }
}
