from game.objects import Object 
from game.math import Vector
import random

TOP_LEFT = Vector(-1,-1)
TOP_CENTER = Vector(0,-1)
TOP_RIGHT = Vector(1,-1)
MIDDLE_LEFT = Vector(-1,0)
CENTER = Vector(0,0)
MIDDLE_RIGHT = Vector(1,0)
BOTTOM_LEFT = Vector(-1,1)
BOTTOM_RIGHT = Vector(1,1)
BOTTOM_CENTER = Vector(0,1)
FAR_TOP = Vector(0,-2)
FAR_RIGHT = Vector(2,0)
FAR_BOTTOM = Vector(0, 2)
FAR_LEFT = Vector(-2,0)

PIECES = {
    "o": ((TOP_LEFT,TOP_CENTER,MIDDLE_LEFT),(TOP_LEFT,TOP_CENTER,MIDDLE_LEFT),
    (TOP_LEFT,TOP_CENTER,MIDDLE_LEFT),(TOP_LEFT,TOP_CENTER,MIDDLE_LEFT)),
    "j": ((TOP_CENTER,BOTTOM_CENTER,BOTTOM_LEFT),(MIDDLE_RIGHT,MIDDLE_LEFT,TOP_LEFT),
    (BOTTOM_CENTER,TOP_CENTER,TOP_RIGHT),(MIDDLE_LEFT,MIDDLE_RIGHT,BOTTOM_RIGHT)),
    "l": ((TOP_CENTER,BOTTOM_CENTER,BOTTOM_RIGHT),(MIDDLE_RIGHT,MIDDLE_LEFT, BOTTOM_LEFT),
    (BOTTOM_CENTER, TOP_CENTER, TOP_LEFT),(MIDDLE_LEFT, MIDDLE_RIGHT, TOP_RIGHT)),
    "i": ((BOTTOM_CENTER,TOP_CENTER,FAR_BOTTOM),(MIDDLE_RIGHT,MIDDLE_LEFT, FAR_LEFT),
    (BOTTOM_CENTER, TOP_CENTER, FAR_TOP),(MIDDLE_LEFT, MIDDLE_RIGHT, FAR_RIGHT)),
    "t": ((MIDDLE_LEFT,TOP_CENTER,MIDDLE_RIGHT),(TOP_CENTER,MIDDLE_RIGHT, BOTTOM_CENTER),
    (MIDDLE_LEFT, BOTTOM_CENTER, MIDDLE_RIGHT),(BOTTOM_CENTER, TOP_CENTER, MIDDLE_LEFT)),
    "z": ((BOTTOM_CENTER,MIDDLE_RIGHT,TOP_RIGHT),(BOTTOM_CENTER,BOTTOM_RIGHT, MIDDLE_LEFT),
    (TOP_CENTER, MIDDLE_LEFT, BOTTOM_LEFT),(MIDDLE_RIGHT, TOP_CENTER, TOP_LEFT)),
    "s": ((MIDDLE_LEFT,TOP_CENTER,TOP_RIGHT),(TOP_CENTER,MIDDLE_RIGHT, BOTTOM_RIGHT),
    (MIDDLE_RIGHT, BOTTOM_CENTER, BOTTOM_LEFT),(BOTTOM_CENTER, MIDDLE_LEFT, TOP_LEFT))
    }
    #TODO create test for piece colors
PIECE_DEFAULT_COLOR = {"o":(0,255,0),"j":(255,0,0),"l":(0,0,255),'i':(255,255,0),
"t":(0,255,255),"z":(255,255,255),"s":(255,0,255)}



class Piece(Object):
    def __init__(self,position:Vector, speed: Vector, color=(1,0,0), piece_type = ""):
        self.size = Vector(20,20)

        if piece_type == "":
            piece_types = list(PIECES.keys())
            piece_type = random.choice(piece_types)
        
        super().__init__("block",position,speed, PIECE_DEFAULT_COLOR[piece_type])

        self.piece_type = piece_type
        self.current_position = 0
        self.add_children()

    def add_children(self):
        for block in PIECES[self.piece_type][0]:
            new_object = Object("block",self.position + (block * self.size), self.speed, PIECE_DEFAULT_COLOR[self.piece_type])
            self.add_child(new_object)
                
    def rotate_piece(self):
        if self.current_position == 3:
            self.current_position = 0
        else:
            self.current_position = self.current_position + 1
        count = 0
        for child in self.children:
            child.position = self.position + (PIECES[self.piece_type][self.current_position][count]*self.size)
            count = count + 1



