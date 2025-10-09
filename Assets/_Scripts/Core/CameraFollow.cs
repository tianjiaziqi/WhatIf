using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace WhatIf
{
    /// <summary>
    /// CameraFollow is responsible for following a target transform with a smooth camera movement and rotation.
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        // All variables are serialized and private
        [Header("Target")]
        [SerializeField] private Transform target;
        [Space(10f)]
        
        [Header("Offsets")]
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private Vector3 lookAtOffset;
        [SerializeField] private Quaternion rotationOffset;
        [Space(10f)]
    
        [Header("Camera Speed")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;

        private void Awake()
        {
            if (target == null)
            {
                // If no target is set, set to player
                Debug.LogWarning("No target set for camera follow, set to player");
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
    
        private void Start()
        {
            // Set initial position and rotation
            transform.position = target.position + positionOffset;
            transform.rotation = Quaternion.LookRotation((target.position + lookAtOffset) - transform.position) * rotationOffset;
        }

        private void LateUpdate()
        {
            // Lerp to target position and rotation
            transform.position = Vector3.Lerp(transform.position, target.position + positionOffset, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((target.position + lookAtOffset) - transform.position) * rotationOffset, rotationSpeed * Time.deltaTime);
        }
    }

}