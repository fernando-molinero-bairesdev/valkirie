import logging
from typing import Dict, List

from pygame.event import clear
from graphics.graphics import Graphics
import json
from time import time

class Game():
    def __init__(self, graphics: Graphics):
        logging.info("Game Starting")
        self.graphics = graphics
        self.debug_time = []
        self.actions = []
        self.times = []
        self.active_objects = []
        self.objects = []
        self.setup_events()
        self.load_objects()
        self.load_actions()
        self.start = time()


    def setup_events(self):
        logging.info("Setting up events")
        self.graphics.set_event("end", self.end_game)
        self.graphics.set_event("key_press", self.keys_pressed)
        self.graphics.set_event("key_up", self.key_up)
        self.graphics.set_event("objects", self.all_objects)

    def move_objects(self):
        raise NotImplementedError

    def collision_detection(self):
        raise NotImplementedError

    def on_collide(self):
        raise NotImplementedError

    def load_actions(self):
        self.actions.append(self.collision_detection)
        self.actions.append(self.move_objects)

    def load_objects(self):
        raise NotImplementedError

    def keys_pressed(self, key):
        raise NotImplementedError

    def key_up(self, key):
        raise NotImplementedError
    
    def set_protagonist(self, object):
        self.protagonist = object

    def all_objects(self) -> List[Dict]:
        return self.objects
    
    def execute_actions(self):
        for action in self.actions:
            action()

    def game_start(self):
        self.active = True
        self.scene_start()
        while self.active:
            self.execute_actions()
            self.graphics.event_handler()
            self.graphics.draw_scene()

    def scene_start(self):
        self.graphics.start_scene()

    def end_game(self):
        self.active = False
