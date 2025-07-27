from abc import ABC, abstractmethod
from turtle import width

class HitObject(ABC):
    @abstractmethod
    def detect_colission(self):
        pass


class HitBox(HitObject):
    def __init__(self, x, y, width, height):
        self.x = x
        self.y = y
        self.width = width
        self.height = height

    def x1(self):
        return self.x - (self.width / 2)

    def x2(self):
        return self.x + (self.width / 2)

    def y1(self):
        return self.y - (self.height / 2)
    
    def y2(self):
        return self.y + (self.height / 2)

    def detect_colission(self,other):
        return self.x1()

