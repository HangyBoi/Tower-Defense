# ⛉ Modular Tower Defense: A Software Architecture Showcase

> **A scalable, data-driven Tower Defense engine built to demonstrate SOLID principles and Design Patterns in Unity.**

![main-game_2_optimized](https://github.com/user-attachments/assets/ab71a7f1-9b21-4867-93c5-05fe68491904)

**Role:** Software Architect & Developer  
**Course:** Game Software Architecture  
**Tech:** Unity, C#, ScriptableObjects  
**Architecture:** Event-Driven, Component-Based  

---

## ✎ᝰ. Project Overview

This project was developed as a final assignment for a **Software Architecture** course. The primary goal was not just to create a functional game, but to engineer a **maintainable, extensible, and designer-friendly codebase** that adheres to strict requirements and SOLID principles.

The result is a "White Box" engine where almost every aspect of gameplay—Wave composition, Tower stats, Enemy types, and Shop prices—can be configured by a Game Designer in the Unity Inspector without writing a single line of code.

---

## ⏣ Architectural Pillars & Design Patterns

The codebase relies on **5 core design patterns** to ensure modularity and decoupling.

<img alt="SoftwareArchitecture drawio" src="https://github.com/user-attachments/assets/b96ebcb2-1d3c-4b6a-ab2f-6875b6e5e25b" />

*UML Diagram of the Whole Project's Architecture (open in a new tab to explore it more freely)*

### 1. The Observer Pattern (Decoupling Logic & UI)
**Goal:** Remove all hard dependencies between the core game logic (Health, Money, Waves) and the User Interface.
* **Implementation:** I utilized static **Event Buses** (`WaveEventsBus`, `CurrencyEventsBus`, `TowerEventsBus`).
* **Benefit:** The `LevelManager` does not know the HUD exists. It simply fires `OnWaveStarted`. The UI listens for this event and updates itself. This prevents "Spaghetti Code" where game logic is cluttered with `textComponent.text = ...`.

### 2. The Strategy Pattern (Hot-Swappable Behaviors)
**Goal:** Allow enemies and towers to have vastly different behaviors without creating a rigid inheritance tree.
* **Towers:** Each tower holds a `ProjectileEffectSO` (ScriptableObject). The `Tower` class simply calls `ApplyEffect()`.
    * *Single Target Strategy:* Deals direct damage.
    * *AOE Strategy:* Physics overlap sphere detection + damage loop.
    * *Debuff Strategy:* Applies speed modifiers.
* **Enemies:** Movement logic is abstracted into an `IMovementStrategy` interface, allowing specialized movement (e.g., Flying vs. Ground) to be swapped at runtime.

### 3. The Factory Pattern (Upgrade System)
**Goal:** Manage complex object creation and replacement during gameplay.
* **Implementation:** When a tower is upgraded, a Factory method handles the transaction: it validates funds, destroys the old instance, instantiates the new visual model from `TowerLevelData`, and injects the new stats.

### 4. Finite State Machine (Game Flow)
**Goal:** Strictly manage the valid transitions between gameplay phases.
* **States:** `BuildingPhase` ↔ `SpawningPhase` → `Win/Lose`.
* **Implementation:** `LevelManager.cs` acts as the central state machine, ensuring players cannot trigger wave logic while in a menu, or build towers during invalid states (configurable via bool `allowTowerBuildingDuringWave`).

### 5. Object Pooling (Optimization)
**Goal:** Eliminate garbage collection spikes during heavy combat.
* **Usage:** Floating "Damage Numbers" and "Money Gain" popups are pooled. Instead of `Instantiate/Destroy`, the UI system recycles existing text elements, maintaining smooth framerates even with 100+ active enemies.

---

## 🗁 Data-Driven Design (Scriptable Objects)

A key requirement was "Editor configurability without coding." I achieved this using **ScriptableObjects** as the database for the game.

![Editor Configuration Gif](docs/images/editor-config.gif)
| System | ScriptableObject Data |
| :--- | :--- |
| **Waves** | `WaveDefinition` - List of enemies, spawn intervals, and delays. |
| **Enemies** | `EnemyDataSO` - Health, Speed, Gold Reward, Prefab reference. |
| **Towers** | `TowerLevelData` - Range, Fire Rate, Projectile Visuals. |
| **Shop** | `ShopItemDataSO` - Button Icon, Cost, Description. |

---

## ≔ Features Checklist

### ꪜ Wave System
* [x] Minimum 5 waves with increasing difficulty.
* [x] Simultaneous vs. Sequential spawning support.
* [x] Fully configurable via Inspector.

### ꪜ Tower System
* [x] **Placement Validation:** Grid-based system with visual indicators (Green/Red ghost mesh).
* [x] **Types:** Single Target, Area of Effect (AOE), Slow/Debuff.
* [x] **Upgrades:** Visual model changes and stat boosts upon upgrading.

### ꪜ Enemy AI
* [x] **Navigation:** NavMesh-based pathfinding following non-linear paths.
* [x] **Feedback:** World-space health bars (Billboarding) and floating money text.

---

## ⌨ Controls & Installation

1. **Clone Repo:** `git clone [REPO LINK]`
2. **Open:** Unity 6.2 (or relevant version).
3. **Play:** Open `Scenes/MainScene` and hit Play.

| Input | Action |
| :--- | :--- |
| **Mouse Click** | Select Tower / Place Tower |
| **Left/Right Arrow Keys** | Adjust Game Time Scale |

---

*Developed at Saxion University of Applied Sciences.*
