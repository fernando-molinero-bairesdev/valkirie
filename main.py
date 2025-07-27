import json

with open("config.json") as f:
    config = json.load(f)

if config["graphics_backend"] == "pygame":
    from graphics.graphics import Graphics
    graphics = Graphics(800, 600)
elif config["graphics_backend"] == "web":
    from graphics.web_graphics import WebGraphics
    graphics = WebGraphics(800, 600)
else:
    raise ValueError("Unknown graphics backend")

from tetris.tetris import Tetris
tetris = Tetris(graphics=graphics)
tetris.game_start()