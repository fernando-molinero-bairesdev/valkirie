from game.math import Vector

class Object:
    def __init__(self, archetype, position, speed, color):
        self.archetype = archetype
        self.children = []
        self.__position = None
        self.set_position(position)
        self.set_speed(speed)
        self.color = color

    def from_dict(object):
        return Object(object.get("archetype"), Vector(object.get("x"), object.get("y")),Vector(object.get("speed").get("x"),object.get("speed").get("y")),(255,255,255))

    def __repr__(self):
        return f"archetype: {self.archetype} position: {self.position} speed: {self.speed}"

    def add_child(self, child):
        self.children.append(child)

    def get_all_objects(self):
        return self.children + [self]

    def set_speed(self, speed):
        self.__speed = speed
        if self.children:
            for child in self.children:
                child.speed = speed

    def get_speed(self):
        return self.__speed
    
    def set_position(self, position):
        if self.__position:
            change = position - self.__position
        else:
            change = position
        self.__position = position
        for child in self.children:
            child.position = child.position + change

    def get_position(self):
        if self.__position:
            return self.__position
        else:
            return None
        
    speed = property(get_speed, set_speed)
    position = property(get_position, set_position)

            
