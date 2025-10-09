using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhatIf
{
    public class StateConfig : ScriptableObject
    {
        public List<StateBase> States = new();
    }
}
