using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhatIf
{
    public class ProjectileMovement : MonoBehaviour
    {
        public float speed;
        private bool _shoot;
        private Transform _target;
        private float _dmg;
        private Unit _attacker;

        private void Start()
        {
            Destroy(gameObject, 5f);
        }

        private void FixedUpdate()
        {
            if (_shoot)
            {
                transform.Translate(Vector3.up * (speed * Time.fixedDeltaTime));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerUnit>().TakeDamage(_attacker, _dmg);
                Destroy(gameObject);
            }
        }

        public void Shoot(Unit attacker, float dmg, Transform target)
        {
            this._attacker = attacker;
            this._dmg = dmg;
            this._target = target;
            transform.rotation = Quaternion.LookRotation(target.position - transform.position)*Quaternion.Euler(90,0,0);
            _shoot = true;
        }
        
    }

}