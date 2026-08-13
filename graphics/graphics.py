from abc import ABC, abstractmethod

class Graphics(ABC):
    def __init__(self, width, height):
        self.active = True
        self.width = width
        self.height = height
        self.events = {}
        self.archetypes = {}

    def set_archetypes(self, archetypes):
        self.archetypes = archetypes

    def set_event(self, event, function):
        self.events[event] = function

    @abstractmethod
    def start_scene(self):
        """Initialize the graphics scene"""
        pass

    def end_scene(self):
        self.active = False
        if "end" in self.events:
            self.events["end"]()

    @abstractmethod
    def event_handler(self):
        """Handle input events"""
        pass

    @abstractmethod
    def draw_object(self, obj):
        """Draw a single object"""
        pass

    @abstractmethod
    def draw_scene(self):
        """Draw the entire scene"""
        pass  