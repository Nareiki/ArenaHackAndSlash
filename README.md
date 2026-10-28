# Arena Hack & Slash – Architecture & Systems Showcase

> **Portfolio Technical Prototype**  
> Built with **Unity 2022.3 (LTS)** | **Universal Render Pipeline (URP)** | **C#**  
> *A technical demonstration of game programming design patterns, extensible AI systems, editor tooling, and performance optimization via Unity's Job System & Burst Compiler.*

---

## 🎯 Overview

This project is an **Arena Hack-and-Slash technical showcase** developed to demonstrate clean software architecture, modern gameplay programming patterns, and performant systems in Unity. Rather than focusing on finished art assets, the repository highlights **reusable systems, decoupled architectures, and scalable game engineering**.

---

## 🏗️ Core Architectural Highlights & Patterns

### 1. Object Pooling Pattern
* **Location:** [`Assets/Project/Scripts/ObjectPooling/ObjectPooler.cs`](Assets/Project/Scripts/ObjectPooling/ObjectPooler.cs)
* **Design Rationale:** Frequent instantiation and destruction of enemies, projectiles, and effects causes garbage collection spikes and CPU frame-time hitches.
* **Implementation:**
  * Tag-based pool manager (`ObjectPooler`) with configurable pre-allocation pools (`ObjectPoolItem`).
  * Dynamic pool expansion (`ShouldExpand`) to handle peak loads gracefully without crashing or dropping spawn requests.
  * Tightly integrated into the **Enemy Factory** (`EnemyFactory.cs`), abstracting entity reuse away from gameplay managers.

---

### 2. Reflection-Driven Editor Tooling & Code Generation
* **Location:** [`Assets/Project/Scripts/AIBehaviour/DataGeneration/`](Assets/Project/Scripts/AIBehaviour/DataGeneration/)  
  *(Key Scripts: `DataGenerationEditor.cs`, `ScriptGenerator.cs`, `NonReflectionValue.cs`, `DataBehaviourBase.cs`)*
* **Design Rationale:** Balancing designer-friendly data authoring with modular AI trees. Extracting parameters from deeply nested runtime nodes manually is error-prone and tedious.
* **Implementation:**
  * **Custom Editor Window (`NodeDataExtractor`):** Uses C# Reflection (`System.Reflection`) to inspect a given `BasicTree` hierarchy at edit time, traversing composite and leaf nodes.
  * **Custom Attribute Filtering:** Employs a custom `[NonReflectionValue]` attribute to automatically ignore private runtime fields (e.g., timers, agent references), isolating only design-time parameters.
  * **Automated C# Code Generation:** `ScriptGenerator` dynamically formats and writes new C# `ScriptableObject` definitions (e.g., `EnemyBehaviourData.cs`) directly to disk and refreshes the AssetDatabase.
  * **Runtime Blackboard Mapping:** Generated `DataBehaviourBase` assets dynamically map their serialized fields into the AI's `Blackboard` dictionary at runtime.

---

### 3. Behaviour Tree AI & Blackboard Architecture
* **Location:** [`Assets/Project/Scripts/AIBehaviour/`](Assets/Project/Scripts/AIBehaviour/)
* **Design Rationale:** Replace rigid finite state machines for complex NPC combat logic with a composable, modular decision hierarchy.
* **Implementation:**
  * **Composite Pattern:** Built from scratch with an `INode` contract, `BasicNode` base class, and composite controllers (`Sequence`, `Selector`).
  * **Decoupled Blackboard Pattern (`Blackboard.cs`):** A centralized, type-safe data context (`GetData<T>()`, `SetData()`) that removes direct dependencies between condition checks (`CheckEnemyInAttackRange`, `CheckEnemyInFOVRange`) and action tasks (`TaskAttack`, `TaskKeepDistance`, `TaskSurroundTarget`, `TaskPatrol`).
  * **Reactive Element Control (`TaskElementControl.cs`):** AI nodes dynamically adapt behaviors and speed based on real-time environmental elements (Fire causes panicking speed bursts; Water/Toxic slows and applies damage-over-time).

---

## ⚡ Additional Technical Highlights

### 4. High-Performance Multithreading: C# Job System & Burst Compiler
* **Location:** [`Assets/Project/Scripts/ThreadOptimization/`](Assets/Project/Scripts/ThreadOptimization/) & [`Assets/Project/Scripts/WorldGeneration/`](Assets/Project/Scripts/WorldGeneration/)
* **Implementation:**
  * Procedural circular arena mesh creation (`CalculateCircularArenaJob.cs`) and perimeter wall placement (`GenerateWallPositionsAndRotationsJob.cs`) running in parallel via `IJobParallelFor`.
  * Fully compiled using Unity's **Burst Compiler** (`[BurstCompile]`) with `NativeArray<T>` allocations to eliminate managed garbage collection during mesh generation.
  * Live procedural updates in-editor via `[ExecuteInEditMode]` and `OnValidate()`.

### 5. Modular Character Controller (SOLID Principles & Strategy Pattern)
* **Location:** [`Assets/Project/Scripts/CharacterScripts/NewHirarchy/`](Assets/Project/Scripts/CharacterScripts/NewHirarchy/)
* **Implementation:**
  * **Single Responsibility & Open/Closed:** Movement logic is decoupled into discrete, composable components (`GroundMovement`, `AirborneMovement`, `CameraMovement`, `WallClimbing`, `WallJump`).
  * **Platforming Mechanics:** Refactored from a monolithic controller into isolated systems: `BasicJump` (base physics), `Multijump` (derived specialization), and `CoyoteTime` (grace-period check) bound through interface contracts (`IJumpable`, `ICoyoteCheckable`).
  * **Input Abstraction:** Uses Unity's New Input System (`PlayerInput.cs`), binding actions via C# events (`OnJumpPerformed`).

### 6. Generic Type-Safe Event Bus (Observer Pattern)
* **Location:** [`Assets/Project/Scripts/MessageSystems/MSEnemySpawn.cs`](Assets/Project/Scripts/MessageSystems/MSEnemySpawn.cs)
* **Implementation:**
  * Lightweight static messaging system supporting generic subscriptions (`Subscribe<T>`, `Unsubscribe<T>`, `Publish<T>`).
  * Allows enemies (`BasicEnemy`) to broadcast death/deactivation events to the `SpawnManager` without maintaining direct references between systems.

### 7. Reactive Environmental Interactions (Simulation & Physics)
* **Location:** [`Assets/Project/Scripts/SimulationScripts/`](Assets/Project/Scripts/SimulationScripts/)
* **Implementation:**
  * Physics particle collisions (`FireSimulation`, `PoisonSimulation`, `RainSimulation`) interface with entities through contract segregation (`IFire`, `IPoison`, `IWeatherAffectable`, `IDamageable`).
  * Seamlessly passes elemental afflictions into the AI's `Blackboard`, triggering behavior tree state changes on the fly.

### 8. GPU Compute Shader & Procedural Instancing
* **Location:** [`Assets/Project/Scripts/Shader/Compute/CubeGraph.cs`](Assets/Project/Scripts/Shader/Compute/CubeGraph.cs)
* **Implementation:**
  * Demonstrates GPU-driven point cloud evaluation using a custom `ComputeShader`, `ComputeBuffer`, and `Graphics.DrawMeshInstancedProcedural` capable of rendering up to 1,000,000 dynamic points smoothly.

---

## 📊 Design Patterns Summary

| Design Pattern | Key Classes / Scripts | Purpose & Benefit |
| :--- | :--- | :--- |
| **Object Pool** | `ObjectPooler`, `EnemyFactory` | Eliminates GC spikes by recycling enemy game objects. |
| **Composite** | `BasicNode`, `Sequence`, `Selector` | Enables modular and extensible Behaviour Tree AI hierarchies. |
| **Blackboard** | `Blackboard`, `EnemyBT`, `GuardBT` | Decouples AI state storage from specific task execution nodes. |
| **Reflection / Meta-Programming** | `DataGenerationEditor`, `ScriptGenerator` | Extracts node configuration and generates typed `ScriptableObject` code. |
| **Factory** | `IFactory`, `EnemyFactory`, `IProduct` | Encapsulates entity instantiation and pooling initialization. |
| **Observer (Event Bus)** | `MSEnemySpawn` | Type-safe, decoupled event messaging between combatants and spawners. |
| **State (FSM)** | `StateMachine`, `PlayerStateMachine`, `PlayerBaseState` | Manages character animation and state transitions. |
| **Strategy / Composition** | `PlayerManager`, `GroundMovement`, `AirborneMovement` | Modular movement behaviours swappable without inheritance bloat. |

---

## 📁 Repository Structure

```text
Assets/
├── Project/
│   ├── Scenes/                  # Test & Demonstration Scenes (AI, Generation, Simulation)
│   └── Scripts/
│       ├── AIBehaviour/         # Behaviour Tree nodes, tasks, blackboard, and reflection tools
│       │   ├── BehaviourTree/   # Base nodes, composites (Sequence, Selector)
│       │   ├── BehaviourTasks/  # Concrete leaf execution tasks (Attack, Patrol, Move, etc.)
│       │   ├── BehaviourChecks/ # Leaf condition nodes (Range, FOV, Cooldown)
│       │   ├── DataGeneration/  # Custom Editor reflection window & script generator
│       │   └── EnemyAI/         # Concrete enemy tree configurations
│       ├── CharacterScripts/    # Modular player controller, jump strategies, and FSM
│       ├── Factory/             # ScriptableObject-based Factory implementation
│       ├── MessageSystems/      # Type-safe generic Event Bus
│       ├── ObjectPooling/       # Dynamic Object Pool manager
│       ├── Shader/Compute/      # Compute Shader and procedural GPU instancing
│       ├── SimulationScripts/   # Elemental particle simulation & interface triggers
│       ├── SpawnSystem/         # Enemy spawner coordinating with Factory & Pool
│       ├── ThreadOptimization/  # Burst-compiled C# Jobs for procedural geometry
│       └── WorldGeneration/     # Procedural arena mesh and wall generation
```

---

## 🛠️ Tech Stack & Requirements

* **Engine:** Unity 2022.3.15f1 (LTS)
* **Render Pipeline:** Universal Render Pipeline (URP 14.0.9)
* **Packages & APIs:**
  * Unity C# Job System & Burst Compiler (`Unity.Jobs`, `Unity.Burst`, `Unity.Collections`)
  * Unity AI Navigation (`com.unity.ai.navigation`)
  * Unity New Input System (`com.unity.inputsystem`)
  * Cinemachine (`com.unity.cinemachine`)
