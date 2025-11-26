
using System.Xml.Schema;
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
        private Transform target;

        public Transform Target
        {
            get => target;
            set
            {
                target = value;
                if (target != null)
                {
                    transform.position = target.position + positionOffset;
                    transform.rotation = Quaternion.LookRotation((target.position + lookAtOffset) - transform.position) * rotationOffset;
                }
            }
        }
        [Space(10f)]
        
        [Header("Offsets")]
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private Vector3 lookAtOffset;
        [SerializeField] private Quaternion rotationOffset;
        [Space(10f)]
    
        [Header("Camera Speed")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;

        
    
        

        private void LateUpdate()
        {
            if (target == null) return;
            // Lerp to target position and rotation
            transform.position = Vector3.Lerp(transform.position, target.position + positionOffset, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((target.position + lookAtOffset) - transform.position) * rotationOffset, rotationSpeed * Time.deltaTime);
        }
    }

}