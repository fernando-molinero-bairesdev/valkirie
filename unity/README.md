# Valkirie engine scaffold (Unity)

Starter C# for the Core systems designed for a shared engine across three games
(Valkirie, a superhero co-op emergency game, a deformable blob game).

This folder is a real Unity project - `ProjectSettings/` and `Packages/manifest.json`
were carried over from a local Unity 6 (`6000.5.8f1`) install's default project (URP,
Input System, and the 2D/Physics2D modules were already configured there), then
stripped of tutorial-specific content and given a fresh project GUID. `Library/`,
`Logs/`, `Temp/`, `UserSettings/` are intentionally not included - Unity regenerates
those on first open, and they don't belong in version control anyway.

## Opening it

1. In Unity Hub, **Open** (or **Add**) this `unity/` folder directly.
2. First open will take a few minutes - Unity has to import everything and build the
   Library cache from scratch.
3. Via Package Manager, add:
   - **Netcode for GameObjects** (`com.unity.netcode.gameobjects`) - needed for the superhero game
   - **2D Pixel Perfect** (`com.unity.2d.pixel-perfect`) - for the NES/SNES-style rendering
   (left out of the manifest deliberately rather than hand-picking version numbers that
   might not resolve cleanly against this Editor build - Package Manager will pick a
   correct one).
4. Open Edit > Project Settings > Player and confirm active input handling is set to
   "Input System Package" (it should already be, since Input System was already installed).

## What's here

- **`Core/Abilities`** - `IMotor` (the interface every game's movement goes through),
  `MotorSwitcher` (lets a character own multiple motors and hand off between them, e.g.
  a flying hero landing), `PlatformerMotor2D`, `AerialMotor2D`.
- **`Core/Powers`** - `PowerEffect` (composable building blocks like `FlightEffect`,
  `MoveSpeedModifierEffect`), `PowerDefinition` (assembles effects into a named power),
  `CharacterDefinition`/`CharacterLoadout` (assembles powers into a character).
- **`Core/Objectives`** - `ObjectiveCondition` (composable building blocks like
  `CollectCountCondition`, `ReachZoneCondition`), `ObjectiveDefinition`, `ObjectiveSet`.
- **`Core/Incidents`** - `IncidentDefinition : ObjectiveSet`, adding severity, a
  difficulty-scaling curve, and `IncidentTrigger`s (condition -> actions, e.g.
  `SpawnEntitiesAction`) that cover both initial spawn tables and escalation with one
  mechanism. `SpawnZone` + `IncidentSpawnZoneRegistry` resolve triggers' string `zoneId`
  references to actual scene positions; `ZoneIdRegistry` + the `[ZoneId]` drawer turn
  those fields into dropdowns to cut down on typo'd ids.
- **`Core/Events`** - a ScriptableObject-asset event bus (`GameEvent`, `StringGameEvent`,
  `GameEventListener`) that everything above communicates through, so Core never has
  hardcoded references to game-specific concepts like "soul" or "civilian."
- **`Core/Editor`** - the `[ZoneId]` dropdown drawer, in its own assembly
  (`Valkirie.Core.Editor.asmdef`) so it never ships in a player build.
- **`Games/Valkirie`, `Games/Superhero`, `Games/Blob`** - empty except for notes on what
  goes there. No scenes or content have been authored yet.

## Known gaps, called out rather than silently built around

- **No object pooling.** `SpawnEntitiesAction` currently calls `Object.Instantiate`
  directly (marked with a `TODO`). Fine for a first pass; needed before repeatable
  ambient waves are viable.
- **No actual Netcode wiring.** `IncidentInstance` is plain C# with a comment that it must
  run server-authoritative only - it doesn't reference `NetworkBehaviour` yet because the
  package isn't installed in this scaffold and the replication approach (which fields
  become `NetworkVariable`/`NetworkList`, which events become `ClientRpc`) hasn't been
  designed in detail.
- **No `SoftBodyMotor`.** The blob game's motor wasn't designed in this pass - see
  `Games/Blob/README.md`.
- **No input-to-motor glue.** `IMotor.Move()` takes a plain direction vector on purpose -
  nothing here reads from the Input System yet. That's a thin per-game controller layer,
  not a Core concern.
