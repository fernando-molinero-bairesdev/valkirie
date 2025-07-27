from tetris.pieces import Piece
from game.math import Vector

def test_piece_creation():
    piece = Piece(Vector(50,50), Vector(0,10))
    for obj in piece.get_all_objects():
        print(obj)
