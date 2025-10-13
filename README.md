# What If..

## Overview

What if.. is a 3D co-op platform puzzle game. The game world is a massive cube, with each face representing the past, present, and future. Two players control a married couple, rotating the cube to switch between time planes and collaborating to navigate through changing mechanisms and enemy positions. Player actions influence each other across time: past failures increase present difficulty, while present failures distort the future. Through temporal interaction, the game explores themes of regret, inevitability, and acceptance.



## Table of Contents
- [Team Plan](#team-plan)
- [Core Gameplay](#core-gameplay)
- [AI Design](#ai-design)
- [Script Event](#script-event)
- [Environmental Design](#environmental-design)
- [Assets](#assets)
- [Physics Scope](#physics-scope)
- [FSM Scope](#fsm-scope)
- [Controls](#controls)
- [Project Configuration](#project-configuration)
- [Assignment 2](#assignment-2)



## Team Plan

Group Number: 22

Members: Jiaziqi Tian(20365005), Zicun Zhao(20358708)

Roles:

- Jiaziqi: Core Gameplay, Timeline systems, AI and FSM implementation, UI design and integration
- Zicun: Physics Interaction Management, Environment and level construction, Balance and parameter design, Documentation, and State Tracking System (records player states before timeline switches, death counts, and dynamic difficulty values)



## Core Gameplay

Two players collaborate to complete platforming and puzzle-solving challenges across two separate zones.

The current face represents the player's present location, displayed as the top face of a square. If the player switches from the current face to a past face, the past face automatically becomes the current state, and the past face of that past face becomes the past state of the past.

Players can rotate the cube at any time to switch between Past / Present / Future.

- Past: By default loads the initial snapshot of the level (player and enemy positions). If a Fragmentation event has occurred earlier, switching to Past instead loads the fragmented snapshot from that event (Loop rule).
- Present: Always represents the most recent current state.
- Future: Only one player may enter at a time. When entered, the character is locked at the level’s endpoint. Entering the Future instantly increases the difficulty of the other player’s current timeline (following Burden Transfer rules).
- Burden Transfer: If any player fails in their timeline, the other player’s timeline instantly increases in difficulty by +1 (e.g., added Thinker, traps, faster enemy speed).

**Success Condition**: Both players reach the exit simultaneously, automatically rotating the cube to the future.

**Failure Condition**: Stepping on traps, falling into void, colliding with monsters.



## Game Type

Platformer Game



## Player Setup

Local co-op for two players. Each player controls one of the husband-and-wife characters. Players may exist in different timelines and spaces, but their actions affect each other. Only one player can enter the future phase at a time.



## AI Design
The behavior of each enemy is governed by a Finite State Machine (FSM), which serves as the core of their decision-making process. The FSM determines the enemy's actions based on factors like player proximity and internal cooldowns.

### Enemy 1: Obstacle FSM

- **Idle**: Remains stationary at guard position, switch to alert state when the player enters its attack range

- **Thinking**: Calculate the path from current position to player's position, once path calculation is complete, it transitions to the Move state
- **Move**: Move to destination, upon reaching the target position, if the target position is player's position switch to attack state

- **Hit**: This state is triggered upon receiving damage from the player, will interrupts all current actions(except died)
- **Knockback**: Enter when player's attack has knockback effect, will apply an upward, backward force on the unit, and also interrupt all current action(except died). If its position out its move range, will immediately enter move state and the destination will be the nearest point in the range 
- **Died**: Plays death animation, and then destroys the GameObject
- **Reset**: Enter this state when players move outside the attack range, set destination to initial position and turn to move state



### Enemy 2: Thinker FSM

- **Idle**: Invisible and invincible
- **Charging / Attack / Fading**: Becomes visible and can take damage. Damage does not interrupt actions; when HP reaches 0, the Thinker immediately enters **Spawn**.
  - **Charging**:  Charges for next attack, then enters Attack state when cooldown completes
  - **Attack**: Shoots a bullet toward the player's position, then returns to Charging.
  - **Fading**: When the player leaves the attack area, fade out and return to Idle (invisible and invincible).

- **Spawn**: Respawn after being destroyed, invincible during spawning.

**Note**: Damage does not interrupt the Thinker's actions; therefore, it lacks a 'Hit' state




### Enemy 3: Sentinel FSM

- **Idle**: Remains stationary at guard position. When the player enters its attack range, sets a random destination in its move range and switches to move state 
- **Move**: Flies to the destination; upon arrival, picks a new random point within range. Switch to attack state when the attack cooldown completes

- **Attack**: Shoots a straight bullet toward player, transitions to move state when attack complete
- **Hit**: Enter when got damage from player, will interrupt all current action(except died)
- **Knockback**: Enter when player's attack has knockback effect, will apply a backward force on the unit, and also interrupt all current action(except died). If its position out his move range, will immediately enter move state and the destination will be the nearest point in the range
- **Died**: Play die animation, and then destroy the game object
- **Reset**: Enter this state when player move outside the attack range, set destination to initial position and turn to move state



## Script Event

- **Fragmentation**: When one person activates a future critical mechanism (Press the button or attack the designated monster) while the other remains in the present, the environment collapses. Both parties become trapped in their current states, facing increased enemy encounters.
- **Loop**: When rotating back to the past, the past retains the state it was in when “Fragmentation” occurred.
- **Farewell**: If one party fails in the final stage, both must restart from the beginning
- **Burden Transfer**: Triggered when any player fails in their current spacetime. The difficulty level instantly increases by 1 in another player's spacetime: Enemy speed increases, death count + 1, Thinker +1, more trap. If the failure happens in the past, present immediately gains Thinker +1; if in the present, future immediately gains Thinker +1. 



## Environmental Design

The world is a massive 3D cube, with gameplay occurring on different 3D planes within the cube.

Two main scenes: Home and Wedding Venue, Home has living room, kitchen, and bedroom. Wedding Venue has lawn and Center

NavMesh baked for AI pathing

Interactive props with colliders/Triggers such as button, door, trap



## Assets

### Scene

Home:

| **Area**    | **Present (Warm)**                                           | **Past (New)**                                            | **Future (Living Alone)**                           |
| ----------- | ------------------------------------------------------------ | --------------------------------------------------------- | --------------------------------------------------- |
| Living Room | Cozy sofa, old coffee table, photo frame with couple, slightly yellowed wedding photo | Brand-new furniture, fresh flowers on table, bright walls | Single cushion, toppled photo frame, dim lighting   |
| Kitchen     | Used utensils, fridge covered with sticky notes              | Utensils neatly arranged, wedding checklist on fridge     | Single set of dishes, empty fridge                  |
| Bedroom     | Double bed, clothes mixed in wardrobe                        | Newly made bed, half-empty wardrobe                       | Single bed, wardrobe with only one person’s clothes |



Wedding Venue:

| **Scene** | **Present (Normal Garden)**        | **Past (Wedding Scene)**                          | **Future (Grave Garden)**       |
| --------- | ---------------------------------- | ------------------------------------------------- | ------------------------------- |
| Lawn      | Trimmed grass, some wilted flowers | Red carpet, white arch with flowers, guest chairs | Withered lawn, overgrown plants |
| Center    | Wooden bench                       | Wedding altar, space for officiant                | Wife’s gravestone, old bench    |



### Combat

Characters:

- Obstacle 
- Thinker
- Sentinel
- Player

Interactive Objects:

- Trap
- Button
- Door

### Sound
- Cube rotation
- Footsteps
- Jump
- Attack/hit
- Failure
- Enemy death
- Interaction sounds(button, door, trap)

### Source of Assets

Models and sounds will come from a mix of free online asset packs (itch.io/Unity Assets Store/kenney.nl/Opengameart.org) and original assets created by our team



## Physics Scope

Rigidbody on players, Sentinel and Obstacle

Colliders on Thinker, doors, platforms and the world cube

Triggers on bullets, traps, and buttons

Implement the enemy knockback effect by applying force to its rigidbody

Enemy knockback occurs only when hit by the player's knockback attack, applying an upward, backward impulse to the enemy's Rigidbody.

Damage detection is based on `Boxcast`/`Spherecast` from the Unity Physics library, triggered on specific frames of the attack animation.

All physics-based operations, such as applying forces for knockback or directly manipulating a Rigidbody's velocity, will be executed within `FixedUpdate` to ensure stable and frame-rate-independent simulation, as per standard physics engine practices



## FSM Scope

Per-object FSM component managing `currentState`, atomic `Transition(to)` with `OnExit/OnEnter`

State classes derive from `StateBase` (Enter/Update/Exit/Can Transition/Interruptible flags)

Event driven transitions using Unity events and C# events

Priority and conflict resolution when multiple transitions fire in the same frame (e.g., Interrupt > Hurt > Move).

Animation hooks: animation events drive damage detection and combo window

Interrupt policy: states tagged as interruptible by specific events (Hurt, Knockback, Stun); others ignored/queued.

Debug overlay: current/previous state, last transition cause, cool down timers

Data-driven config: ScriptableObject assets for state params (durations, thresholds, interrupt rules)

subscribe events in `OnEnter`, unsubscribe in `OnExit`



## System and Mechanics

Both players must reach the exit simultaneously to clear the level; stepping on traps, falling, or colliding with enemies results in failure and increases the difficulty in the other player’s timeline.

Players can rotate the cube at any time to switch between Past / Present / Future; the Past replays recorded actions, while the Future becomes more difficult as failures occur.

- Past: Stores the player’s initial state at the start of the level. When the cube rotates to the Past, that state is treated as the “Present,” and players can use it to alter the current timeline.

- Present: Always represents the player’s current state.

- Future: Locks the player at the level’s endpoint. Switching to the Future increases the difficulty of the other player’s current timeline.

Attacking enemies grants a temporary double jump; enemies follow FSM-driven behaviors and can be knocked back.

Special events (Fragmentation, Loop, Burden Transfer) dynamically alter the environment and timeline states.

Audio and visual effects provide feedback for key events such as environment collapse, enemy death, and cube rotation.



## Controls

WASD or Arrow keys Move

Space Jump

JIKL Rotate cube

E Interact (mechanisms, items)

Left mouse button Attack/Knockback enemies

Shift Sprint

Esc Pause, open system settings menu



## Project Configuration

Unity (2022.3.62f1), C# Script for

- `Unit`: Base class for enemies and players  
- `FSM`: Manage state transitions, held by each unit  
- `StateBase`: Base class for all states  
- `CubeRotator`: Handle cube rotation and timeline switching  
- `TimelineManager`: Record timeline events and adjust difficulty  
- `UIManager`: Manage UI panels and HUD

NavMesh for AI Pathing

Enemy movement will be driven by Unity's NavMeshAgent component, which will handle pathfinding and physical navigation

Animator controllers for players and enemies with parameters speed(blend tree), attackTrigger, sprintTrigger, isGrounded, jumpTrigger, interactionTrigger, KnockbackTrigger

Animator controllers for cube with parameters upTrigger, downTrigger, leftTrigger and rightTrigger

Collision layers configured in Project Settings

GitHub repository with commits organized by functional module and clear descriptions

README and in-game debug overlay displaying FPS, current/previous state, timeline difficulty, and Failure count for assessment and debugging

## Assignment 2

### FSM Description

- **Idle**: Invisible and invincible
- **Charging / Attack / Fading**: Becomes visible and can take damage. Damage does not interrupt actions; when HP reaches 0, the Thinker immediately enters **Spawn**.
  - **Charging**:  Charges for next attack, then enters Attack state when cooldown completes
  - **Attack**: Shoots a bullet toward the player's position, then returns to Charging.
  - **Fading**: When the player leaves the attack area, fade out and return to Idle (invisible and invincible).

- **Spawn**: Respawn after being destroyed, invincible during spawning.

**Note**: Damage does not interrupt the Thinker's actions; therefore, it lacks a 'Hit' state

### FSM Diagram
``` mermaid
stateDiagram-v2
    Spawn

    state "Visible & Vulnerable" as Active {
        Charging: Charging
        Attack: Attack
        Fading: Fading

        Charging --> Attack: Charge timer complete
        Attack --> Charging: Attack finished
        Charging --> Fading: Player leaves range
    }

    Spawn --> Charging: Respawn complete
    Idle --> Charging: Player enters range
    Fading --> Idle: Fade out complete
    Active --> Spawn: HP <= 0
```
### Gameplay Video
https://youtu.be/6sQPsoHl8Xk

### Temporary Control:
WASD - Move
J - Deal 20 damage to Thinker

