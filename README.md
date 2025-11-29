# What If.. (Multiplayer Build)

## Assignment 4: Networking Implementation

**Group 22:**

* Jiaziqi Tian (20365005)

* Zicun Zhao (20358708)

## Table of Contents

* [Demo Video](#demo-video)

* [Setup Instructions](#setup-instructions )

* [Controls](#controls)

* [Bonus Function Implemented](#bonus-function-implemented)

* [Technical Stack](#technical-stack)

## Demo Video
**[Click here to watch]( https://youtu.be/8LZ5apoFIw8 )**

## Setup Instructions
To test the multiplayer features locally:
1.  **Clone** this repository and switch to the networking branch.
2.  **Open** the project in Unity (2022.3.62f2).
3.  **Build the Project:** Go to `File > Build and Run` (Mac/Windows) to build a game application.
4.  **Launch:** Choose one of the following options:
    1.  Run 2 instances of the built application
    2.  Run 1 instance of the built application and play the other instance in the Unity Editor
    3.  Use ParrelSync(included in the repo, and can be accessed from the Unity tool bar under "ParrelSync") to play 2 instances in Unity Editor

5.  **Connect:**
    - **Host**: Click "Start as Host"
    - **Client**: Click "Start as Client" and enter the Join Code displayed on the top-right corner of the Host's screen.



## Controls

**WASD**: Move

**Space**: Jump

**J**: Attack

**Enter**: Chat

**Esc**: Hide chat panel / Unfocus input



## Bonus Function Implemented

### Metaverse Features

- **Shared Lobby**: Both Host and Client enter a shared lobby immediately upon joining the session. They can move, chat, and interact before the game starts.
- **Avatars**: All players can customize their avatar color in the Lobby Scene by clicking the color buttons at the top of the screen. This choice persists into the game.
- **Social Tools**: Players can chat in real-time in both the Lobby Scene and Game Scene by pressing the Enter key.



## Technical Stack

* **Framework**: Unity Netcode for GameObjects (NGO)
* **Connectivity**: Unity Relay Service (Handles Join Codes & NAT traversal)
