using Unity.Netcode;
using UnityEngine;

namespace WhatIf
{
    /// <summary>
    /// Enum for unit types. Used by AI to determine relationships.
    /// </summary>
    public enum EUnitType
    {
        Player,
        Enemy
    }
    
    /// <summary>
    /// Base class for all units, has common behavior of all units.
    /// abstract to prevent instantiation.
    /// </summary>
    public abstract class Unit : NetworkBehaviour
    {
        /// <summary>
        /// State config: A scriptable object defines all state types for this unit.
        /// </summary>
        public StateConfig stateConfig;//
        
        /// <summary>
        /// Finite State Machine: Manage the current state for this unit.
        /// </summary>
        public FSM fsm;
        
        
        public Rigidbody rb;
        
        public Animator animator;

        /// <summary>
        /// Max Hp of this unit.
        /// </summary>
        public double maxHp;

        /// <summary>
        /// Current Hp of this unit. Will set to maxHp on initialization.
        /// </summary>
        public NetworkVariable<double> currentHp = new NetworkVariable<double>();
        
        /// <summary>
        /// Unit type, used by AI to determine relationships.
        /// Player: handle input
        /// </summary>
        public EUnitType unitType;
        
        
        public float atk;

        public GroundDetctor groundDetector;
        public bool isGrounded;

        public Vector3 PlanarVelocity
        {
            get => new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            set => rb.velocity = new Vector3(value.x, rb.velocity.y, value.z);
        }

        /// <summary>
        /// Handle damage
        /// </summary>
        /// <param name="attacker">Who attacked this unit</param>
        /// <param name="damage">Amount of damage to take</param>
        public virtual void TakeDamage(Unit attacker, double damage)
        {
            if (!IsServer) return;
            // Do nothing if the unit is dead or no damage
            if(damage <= 0) return;
            if(IsDead()) return;
            currentHp.Value -= damage;
            currentHp.Value = System.Math.Max(currentHp.Value, 0); // Make sure hp never below 0
            Debug.Log($"{name} got {damage} damage from {attacker.name}, current hp: {currentHp}");
            
            // check if dead
            if (currentHp.Value <= 0)
            {
                Ondeath();
            }
        }


        protected virtual void Ondeath()
        {
            Debug.Log($"{name} died");
        }
        
        /// <summary>
        /// BoxCollider that define attack area, this should be set to a trigger
        /// </summary>
        public BoxCollider attackArea;

        [Header("Movement")] 
        public float walkSpeed;
        public float runAcceleration;

        [Header("Jump")] public float jumpHeight;
        
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                currentHp.Value = maxHp;
            }
        }

        protected virtual void Awake()
        {
            Debug.Log($"{name} Awake Called");
            // Create an FSM for the unit
            fsm = new();
            // register all states in state config
            InitializeStatesFromConfig();
        }
        
        /// <summary>
        /// Register all states in state config to the FSM.
        /// </summary>
        private void InitializeStatesFromConfig()
        {
            // Do nothing if state config is null or the state list is empty
            if (stateConfig == null)
            {
                Debug.LogError($"State config is null for {name}");
                return;
            }

            if (stateConfig.States == null || stateConfig.States.Count == 0)
            {
                Debug.LogError($"State list in state config is empty for {name}");
                return;
            }
            
            foreach (var state in stateConfig.States)
            {
                // Continue if the state is null
                if (state == null)
                {
                    Debug.LogError($"Found null state in state config for {name}");
                    continue;
                }

                var stateInstance = Instantiate(state);
                
                // Register the state to the FSM
                fsm.RegisterState(stateInstance, this);
            }
            rb = GetComponent<Rigidbody>();
            Debug.Log($"{name} initialized with {fsm.GetRegisteredStateCount()} states");
        }

        protected virtual void Start()
        {
            if (fsm == null)
            {
                Debug.LogError($"FSM is null for {name}");
                return;
            }
            
            var idleState = fsm.GetState<IdleState>();
            if (idleState == null)
            {
                Debug.LogError($"IdleState not found in FSM for {name}. ");
                return;
            }
            
            groundDetector.OnEnter.AddListener((other) =>
            {
                if (other.CompareTag("Ground"))
                {
                    isGrounded = true;
                }
            });
            groundDetector.OnExit.AddListener((other) =>
            {
                if (other.CompareTag("Ground"))
                {
                    isGrounded = false;
                }
            });
            fsm.ChangeState<IdleState>();
        }

        protected virtual void Update()
        {
            // Call update of FSM
            fsm.Update();
        }
        
        
        /// <summary>
        /// Check if this unit should respond to input(only player respond to input).
        /// </summary>
        /// <returns></returns>
        public bool ShouldRespondToInput()
        {
            return unitType == EUnitType.Player && !IsDead();
        }

        /// <summary>
        /// Check if this unit died
        /// </summary>
        /// <returns></returns>
        public virtual bool IsDead()
        {
            return false;
        }
        
        /// <summary>
        /// Check if a target is an enemy
        /// </summary>
        /// <param name="targetUnit"></param>
        /// <returns></returns>
        public bool IsEnemy(Unit targetUnit)
        {
            if(targetUnit == null) return false;
            if(targetUnit == this) return false; // Can't attack self
            if (targetUnit.IsDead()) return false; // Can't attack a dead unit
            if (this.IsDead()) return false; // Can't attack if this unit died

            var myType = this.unitType;
            var targetType = targetUnit.unitType;

            switch (myType)
            {
                case EUnitType.Player when targetType == EUnitType.Enemy:
                case EUnitType.Enemy when targetType == EUnitType.Player:
                    return true;
                default:
                    return false;
            }
        }

        public abstract void OnAttack();


    }

}