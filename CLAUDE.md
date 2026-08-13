# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Valkirie is a Python base game engine. The goal is a reusable engine (`game/`, `graphics/`) with individual games (currently `tetris/`) built on top of it.

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
  - `Graphics.load_archetypes()` hardcodes the path `./tetris/objects/archetypes.json`. This means the graphics layer is not actually game-agnostic yet — it directly references the `tetris` game's data file. Any new game (or the `web` backend) currently depends on that path existing, or on this method being generalized (e.g. taking a path/game parameter) first.

- **`tetris/` — the one concrete game currently built on the engine.**
  - `Tetris(Game)` implements Tetris' rules: gravity/movement (`move_objects`), AABB collision against nearby non-active objects (`get_crashable_pieces`, `collision_detection`), line clearing (`check_line`), and spawns a new `Piece` after each collision (`on_collide` → `create_piece`).
  - `Piece(Object)` represents a tetromino: `pieces.py` defines each shape (`o`, `j`, `l`, `i`, `t`, `z`, `s`) as 4 rotation states, each a tuple of relative-offset constants (`TOP_LEFT`, `MIDDLE_RIGHT`, etc.) scaled by the piece's cell size; `rotate_piece()` repositions children to the next rotation state.
  - `tetris/objects/archetypes.json` maps an archetype name (`floor`, `wall`, `block`) to its shape/size for rendering; `tetris/objects/objects.json` is the level's static starting objects, loaded via `Object.from_dict`.

## Coordinate system

Positions are pixel-based `Vector(x, y)`. The Tetris board uses 20px cells (`Tetris.size`); collisions are simple AABB rectangle overlap, not pixel-perfect.
