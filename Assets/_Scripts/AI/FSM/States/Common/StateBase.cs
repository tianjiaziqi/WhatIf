using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace WhatIf
{
    /// <summary>
    /// Base class for all states, every unit always in a state.
    /// All states are a Scriptable object
    /// </summary>
    public abstract class StateBase : ScriptableObject
    {
        // Name of the state
        public abstract string StateName { get; }
        // Call when enter this state
        public abstract void OnEnter(StateBase oldState);
        // Call each frame when this state is active
        public abstract void OnUpdate();
        // Call when exit this state
        public abstract void OnExit();
            
        // Determine if hit can interrupt the state, default value is true
        public virtual bool CanInterrupt() => true;

        /// <summary>
        /// Which FSM this state belongs to.
        /// </summary>
        [ReadOnly] public FSM ownerFsm;
            
        /// <summary>
        /// Which unit this state belongs to.
        /// </summary>
        [ReadOnly] public Unit ownerUnit;
    }

    /// <summary>
    /// Base class for states with parameters.
    /// </summary>
    /// <typeparam name="TParam">Parameter type</typeparam>
    public abstract class StateBase<TParam> : StateBase where TParam : StateEnterParamBase
    {
        // Enter parameter
        protected TParam EnterParam{get; private set;}
        
        // Override OnEnter to set enter parameter
        public sealed override void OnEnter(StateBase oldState)
        {
            OnEnter(EnterParam, oldState);
        }
        
        protected abstract void OnEnter(TParam param, StateBase oldState);
        
        // Call by FSM to set the parameter
        internal void SetEnterParam(TParam param)
        {
            EnterParam = param;
        }
    }

    public abstract class StateBaseNoParam : StateBase<NoParam>
    {
        protected sealed override void OnEnter(NoParam param, StateBase oldState)
        {
            OnEnterNoParam(oldState);
        }
        protected abstract void OnEnterNoParam(StateBase oldState);
    }
}
