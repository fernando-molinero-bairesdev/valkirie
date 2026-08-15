# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Valkirie started as a Python/pygame base game engine (`game/`, `graphics/`, `tetris/` — see below, still present and functional). The project is now pivoting to a Unity/C# engine at [`unity/`](unity/README.md), aimed at three planned games: Valkirie (2D platformer), a multiplayer co-op superhero-emergency game, and a physics-driven deformable-blob game. The Python code is not being actively extended for that goal; treat it as the original prototype, not the current target architecture.

### Unity architecture summary (see `unity/README.md` for the full breakdown)

- **Layering:** `Core` (engine-agnostic: motors, powers, objectives/incidents, events) → per-motor implementations (`PlatformerMotor2D`, `AerialMotor2D`, a not-yet-built `SoftBodyMotor` for the blob game) → per-game content (`Games/Valkirie`, `Games/Superhero`, `Games/Blob` — currently empty placeholders).
- **Composition over inheritance, applied consistently:** `PowerDefinition` (character abilities), `ObjectiveDefinition`/`IncidentDefinition` (level/emergency goals) all follow the same pattern — a ScriptableObject definition holding a list of small, reusable, composable effect/condition/action ScriptableObjects. New content is usually a new asset, not new code.
- **`IMotor`** is the interface all movement goes through (`Vector3`/`Quaternion`, not `Vector2`, specifically so a future 3D game can reuse the same ability/objective code). `MotorSwitcher` lets one character own several motors and hand off between them (e.g. a flying hero landing).
- **`IncidentDefinition : ObjectiveSet`** adds severity, a player-count difficulty curve, and `IncidentTrigger`s (condition → actions) that unify spawn tables and escalation into one mechanism, for the superhero game's concurrent, reactive emergencies. Spawn zones are referenced by string id (`[ZoneId]`-drawn dropdown backed by `ZoneIdRegistry`), not direct scene references, so incidents stay portable across maps.
- Real gaps, not yet designed/built: no object pooling, no actual Netcode for GameObjects wiring (`IncidentInstance` must run server-authoritative only once that's added), no `SoftBodyMotor`, no input-to-motor glue layer.

No Unity project files (`ProjectSettings/`, `Library/`) exist yet — `unity/README.md` has setup steps.

---

## Python prototype (legacy, not actively developed)

The goal was a reusable engine (`game/`, `graphics/`) with individual games (currently `tetris/`) built on top of it.

## Commands

```bash
# Install runtime deps (pygame)
pip install -r requirements.txt

# Install dev deps (pytest)
pip install -r dev_requirements.txt

# Run the game (backend chosen by config.json's "graphics_backend")
python main.py

# Run all tests
pytest

# Run a single test
pytest tetris/tests/test_pieces.py::test_piece_creation
```

A virtualenv already exists at `.env/` (activate with `.env/Scripts/activate` on Windows).

`config.json` controls which `Graphics` backend `main.py` wires up: `"pygame"` (implemented) or `"web"` (stub, see below).

## Architecture

Three layers, in order of dependency:

- **`game/` — engine core, meant to be game-agnostic.**
  - `Game` (`game.py`) is an abstract base implementing the template-method game loop: `game_start()` calls `scene_start()` once, then loops `execute_actions()` (runs `collision_detection` then `move_objects`) → `graphics.event_handler()` → `graphics.draw_scene()`. Concrete games must implement `load_objects`, `move_objects`, `collision_detection`, `on_collide`, `keys_pressed`, `key_up`.
  - `Object` (`objects.py`) holds position/speed/color and a parent-child tree: moving a parent's `position` shifts all children by the same delta (see `set_position`); `speed` propagates to children the same way.
  - `math.py` has `Vector` (2D, `+`/`-`/`*`) and `Rectangle` (AABB via `x1/x2/y1/y2`, overlap test `insersect` — note the typo, that's the actual method name in use).
  - `collision.py` (`HitBox`/`HitObject`) is not wired into any game — actual collision detection instead builds `Rectangle`s directly and calls `insersect` (see `tetris/tetris.py`). Treat `collision.py` as dead/incomplete code, not the live collision path.

- **`graphics/` — rendering + input abstraction.**
  - `Graphics` (`graphics.py`) is an ABC declaring `start_scene`, `event_handler`, `draw_object`, `draw_scene`, plus an event-callback registry (`set_event`/`events`) that games use to hook `end`, `key_press`, `key_up`, and `objects` (a callback the graphics layer calls to get the current object list to draw).
  - `PygameGraphics` is the working backend: maps arrow keys/space/escape to `key_press`/`key_up` events, draws rectangles per-object using per-archetype width/height.
  - `WebGraphics` is a stub — every method is a no-op placeholder.
  - `Graphics` never does file I/O — it just holds `self.archetypes` (a dict) and exposes `set_archetypes(archetypes)`. `Game.__init__` calls a `load_archetypes()` hook (each concrete game implements this the same way it implements `load_objects()` — see `Tetris.load_archetypes`, which reads `tetris/objects/archetypes.json`) and immediately does `self.graphics.set_archetypes(self.archetypes)`, so data is wired in once at construction time, before `start_scene()` (which now takes no arguments) is ever called. This keeps `Graphics` a pure rendering abstraction — a new game only needs to implement its own `load_archetypes()`.

- **`tetris/` — the one concrete game currently built on the engine.**
  - `Tetris(Game)` implements Tetris' rules: gravity/movement (`move_objects`), AABB collision against nearby non-active objects (`get_crashable_pieces`, `collision_detection`), line clearing (`check_line`), and spawns a new `Piece` after each collision (`on_collide` → `create_piece`).
  - `Piece(Object)` represents a tetromino: `pieces.py` defines each shape (`o`, `j`, `l`, `i`, `t`, `z`, `s`) as 4 rotation states, each a tuple of relative-offset constants (`TOP_LEFT`, `MIDDLE_RIGHT`, etc.) scaled by the piece's cell size; `rotate_piece()` repositions children to the next rotation state.
  - `tetris/objects/archetypes.json` maps an archetype name (`floor`, `wall`, `block`) to its shape/size for rendering; `tetris/objects/objects.json` is the level's static starting objects, loaded via `Object.from_dict`.

## Coordinate system

Positions are pixel-based `Vector(x, y)`. The Tetris board uses 20px cells (`Tetris.size`); collisions are simple AABB rectangle overlap, not pixel-perfect.
