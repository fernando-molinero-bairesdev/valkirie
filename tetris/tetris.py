from game.game import Game
from game.math import Vector, Rectangle
from game.objects import Object
from tetris.pieces import Piece
import json

class Tetris(Game):
    def __init__(self, graphics):
        self.size = 20
        self.score = 0
        super().__init__(graphics=graphics)
        self.stage = {"base-speed": 0.008}

    def load_objects(self):
        with open('./tetris/objects/objects.json') as f:
            self.objects = []
            objects = json.load(f)
            for obj in objects:
                self.objects.append(Object.from_dict(obj))

        self.create_piece()

    def get_crashable_pieces(self):
        return  [object for object in self.objects if object not in self.active_objects and
            object.position.x >= self.protagonist.position.x - (self.size * 2) and  
            object.position.x <= self.protagonist.position.x + (self.size * 2) and
            object.position.y >= self.protagonist.position.y - self.size]

    def get_crashable_horizontal_pieces(self, direction):
        if direction > 0:
            return [object for object in self.objects if object not in self.active_objects and 
                object.position.x >= self.protagonist.position.x - (self.size * 2) and
                object.position.y >= self.protagonist.position.y - (self.size * 2) and
                object.position.y <= self.protagonist.position.y + (self.size * 2)
            ]
        elif direction < 0:
            return [object for object in self.objects if object not in self.active_objects and 
                object.position.x <= self.protagonist.position.x + (self.size * 2) and
                object.position.y >= self.protagonist.position.y - (self.size * 2) and
                object.position.y <= self.protagonist.position.y + (self.size * 2)
            ]
        
    def create_piece(self):
        piece = Piece(Vector(150,50),Vector(0,10))
        self.active_objects.extend(piece.get_all_objects())
        self.objects.extend(piece.get_all_objects())
        
        self.protagonist = piece
        if self.collision_detection():
            self.end_game()

    def horizontal_collision_detection(self, direction):
        crashable_pieces = self.get_crashable_horizontal_pieces(direction)

        for active in self.active_objects:
            movable = Rectangle(active.position, Vector(self.size, self.size))
            for crash in crashable_pieces:
                crashable = Rectangle(crash.position, Vector(self.size, self.size))
                if crashable.insersect(movable):
                    return True
            if movable.position.x >= 230 and direction > 0:
                return True
            if movable.position.x <= 50 and direction < 0:
                return True
        return False

    def horizontal_move(self, direction):
        if not self.horizontal_collision_detection(direction):
            self.protagonist.position = self.protagonist.position + Vector(direction * self.size,0)

    def keys_pressed(self, key):
        if key == "ESC":
            self.end_game()
            return
        if key == "UP":
            self.protagonist.speed = Vector(0,8)
            return
        if key == "DOWN":
            self.protagonist.speed = Vector(0,30)
            return
        if key == "LEFT":
            self.horizontal_move(-1)
            return
        if key == "RIGHT":
            self.horizontal_move(1)
            return
        if key == "SPACE":
            self.protagonist.rotate_piece()
            return
    
    def key_up(self, key):
        if key == "ESC":
            self.end_game()
            return
        if key == "UP":
            self.protagonist.speed = Vector(0,10)
            return
        if key == "DOWN":
            self.protagonist.speed = Vector(0,10)
            return

    def deactivate_objects(self):
        self.active_objects = []

    def move_objects(self):
        for active in self.active_objects:
            if isinstance(active, Object):
                if active.speed.x == 0:
                    active.position.y = active.position.y + (active.speed.y * self.stage["base-speed"])
                active.position.x = active.position.x + (active.speed.x * 0.01)

    def collision_detection(self):
        crashable_pieces = self.get_crashable_pieces()

        for active in self.active_objects:
            movable = Rectangle(active.position, Vector(self.size, self.size))
            for crash in crashable_pieces:
                crashable = Rectangle(crash.position, Vector(self.size, self.size))
                if crashable.insersect(movable):
                    self.on_collide()
                    return True
            if movable.position.y >= 480:
                self.on_collide()
                return True
        return False

    def check_line(self, y):
        tolerance = 10
        lines = list(filter(lambda objs: objs.position.y >= y - tolerance 
        and objs.position.y <= y + tolerance and objs.archetype == 'block', self.objects
        ))
        if len(lines) >= 10:
            deleted_line = y + tolerance
            for line in lines:
                self.objects.remove(line)
            moved_pieces = filter(lambda blocks: blocks.archetype == 'block' and blocks.position.y < deleted_line, self.objects)
            for moved in moved_pieces:
                moved.position.y = moved.position.y + 20
        pass

    def on_collide(self):
        self.protagonist.speed.y = 0
        for active in self.active_objects:
            self.check_line(active.position.y)
        self.deactivate_objects()
        self.create_piece()
