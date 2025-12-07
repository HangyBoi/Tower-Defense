# 🛡️ Modular Tower Defense: A Software Architecture Study

> **A Unity project focused on the practical application of Design Patterns and SOLID principles.**

<img alt="SoftwareArchitecture drawio" src="https://github.com/user-attachments/assets/80b1dcee-18fc-46ea-91cd-8fe9e4c0af41" />

**Focus:** Software Architecture, Clean Code, Scalability  
**Language:** C#  
**Course:** Software Architecture (Saxion University)

---

## 🏛️ Architectural Goals

This project was built to satisfy a strict set of technical requirements, focusing on modularity and the ability to extend the game without modifying the core codebase (Open/Closed Principle).

### 🧩 Design Patterns Implemented

Based on the technical requirements, the following patterns were engineered into the system:

#### 1. Strategy Pattern (Towers & Enemies)
* **Problem:** Towers need different attack styles (Single Target, AOE, Slow Effect) and Enemies need different movement behaviors.
* **Solution:** Implemented `IAttackBehavior` and `IMovementBehavior` interfaces.
* **Result:** New tower types can be created by simply creating a new class that implements the interface, without touching the base `Tower` class.

#### 2. Observer Pattern (UI & Game State)
* **Problem:** The UI needs to update when money changes, waves start, or enemies die, but the `GameManager` shouldn't know about the UI.
* **Solution:** `Subject` (Events) and `Observer` architecture.
* **Result:** The UI subscribes to the `EconomyManager` and `WaveManager`. When data changes, the managers invoke an event, and the UI updates automatically. Complete decoupling.

#### 3. Factory Method (Enemy Spawning)
* **Problem:** The game needs to spawn different combinations of enemies based on Wave Configurations.
* **Solution:** An `EnemyFactory` handles the instantiation logic.
* **Result:** The Wave Manager simply requests an enemy type, and the Factory handles the complexity of object creation and initialization.

#### 4. Singleton Pattern (Service Locators)
* **Usage:** Applied to `GameManager`, `SoundManager`, and `EconomyManager` to provide a global access point to persistent systems that must exist only once.

---

## 💾 Data-Driven Design

To meet the requirement of "Configurability without Code Changes," the project utilizes Unity **ScriptableObjects**.

* **Wave Configuration:** Designers can create `WaveData` assets to define enemy counts, types, and spawn intervals.
* **Enemy Stats:** Health, Speed, and Reward values are injected via assets, allowing for balancing directly in the Inspector.

---

## ✅ Requirements Checklist

The project successfully passed the academic criteria, including:
* [x] **Wave System:** 5+ waves with configurable difficulty.
* [x] **Pathfinding:** Enemies follow non-linear paths to a target.
* [x] **Tower Diversity:** 3 distinct tower types (Single, AOE, Debuff).
* [x] **Economy:** Functional Buy/Sell/Upgrade loop.
* [x] **Zero Warnings:** The project compiles with 0 warnings or errors.

---

*Developed by Nichita Cebotari.*
