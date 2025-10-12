using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace WhatIf
{
    /// <summary>
    /// Finite State Machine - responsible for managing the state of units.
    /// </summary>
    
    [Serializable]
    public class FSM
    {
        /// <summary>
        /// Dictionary of all registered states.
        /// </summary>
        private Dictionary<Type, StateBase> _states = new Dictionary<Type, StateBase>();

        /// <summary>
        /// Current state this FSM holding
        /// </summary>
        public StateBase CurrentState { get; private set; }
        
        /// <summary>
        /// Update method (not unity) for FSM, call the current state's update every frame.
        /// </summary>
        public void Update()
        {
            if (CurrentState != null)
            {
                try
                {
                    CurrentState.OnUpdate();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        /// <summary>
        /// Register a state to this FSM.
        /// </summary>
        /// <param name="state"> the state wants to register</param>
        /// <param name="ownerUnit">which unit this state belongs to</param>
        /// <typeparam name="TState">type of the state</typeparam>
        public void RegisterState<TState>(TState state, Unit ownerUnit = null) where TState : StateBase
        {
            state.ownerFsm = this;
            state.ownerUnit = ownerUnit;
            _states[state.GetType()] = state;
        }

        /// <summary>
        /// Change to a state with a parameter.
        /// </summary>
        /// <param name="param">Parameter</param>
        /// <typeparam name="TState">Which State</typeparam>
        /// <typeparam name="TParam">What kind of Parameter</typeparam>
        /// <exception cref="ArgumentException"></exception>
        public void ChangeState<TState, TParam>(TParam param) where TState : StateBase<TParam>
            where TParam : StateEnterParamBase
        {
            if (!_states.TryGetValue(typeof(TState), out var state))
            {
                throw new ArgumentException($"State {typeof(TState)} not registered");
            }

            var typeState = (StateBase<TParam>)state;
            ChangeStateInternal(typeState, param);
            Debug.Log($"Change state to {typeof(TState)}");
        }
        
        /// <summary>
        /// Change to a state without a parameter.
        /// </summary>
        /// <typeparam name="TState"> Which State</typeparam>
        public void ChangeState<TState>() where TState : StateBaseNoParam
        {
            ChangeState<TState, NoParam>(NoParam.Instance);
        }

        public StateBase GetState<TState>() where TState : StateBaseNoParam
        {
            if (!_states.TryGetValue(typeof(TState), out var state))
            {
                throw new ArgumentException($"State {typeof(TState)} not registered");
            }
            else
            {
                return state as TState;
            }
        }

        /// <summary>
        /// Internal method to change state.
        /// </summary>
        /// <param name="newState">state change to</param>
        /// <param name="param">parameter with new state</param>
        /// <typeparam name="TParam">type of parameter</typeparam>
        private void ChangeStateInternal<TParam>(StateBase<TParam> newState, TParam param) where TParam : StateEnterParamBase
        {
            var oldState = CurrentState;
            
            // call OnExit of old state
            if (CurrentState != null)
            {
                CurrentState.OnExit();
            }

            //Update current state
            CurrentState = newState;
            newState.SetEnterParam(param);

            // call OnEnter of new state
            newState.OnEnter(oldState);
            
        }

        /// <summary>
        /// Get the number of registered states.
        /// </summary>
        /// <returns></returns>
        public int GetRegisteredStateCount()
        {
            return _states.Count;
        }
        
        
        
    }
}
