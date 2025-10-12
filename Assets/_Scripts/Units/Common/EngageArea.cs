using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EngageArea : MonoBehaviour
{
    public UnityEvent<Collider> OnEnter;
    public UnityEvent<Collider> OnExit;

    private void OnTriggerEnter(Collider other)
    {
        OnEnter?.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        OnExit?.Invoke(other);
    }
}
