class Vector:
    def __init__(self,x,y):
        self.x = x
        self.y = y
    def __add__(self, other):
        return Vector(self.x + other.x, self.y + other.y)

    def __sub__(self, other):
        return Vector(self.x - other.x, self.y - other.y)

    def __mul__(self, other):
        return Vector(self.x * other.x, self.y * other.y)
    def __repr__(self):
        return f"x:{self.x} y:{self.y}"

class Rectangle:
    def __init__(self,position:Vector, size:Vector):
        self.position = position
        self.size = size

    def x1(self):
        return self.position.x - (self.size.x / 2.0)

    def x2(self):
        return self.position.x + (self.size.x / 2.0)

    def y1(self):
        return self.position.y - (self.size.y / 2.0)

    def y2(self):
        return self.position.y + (self.size.y / 2.0)

    def insersect(a, b):
        return (a.x2() > b.x1() and a.x1() < b.x2() and a.y2() > b.y1() and a.y1() < b.y2())