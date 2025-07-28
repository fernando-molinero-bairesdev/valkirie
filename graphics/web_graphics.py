from .graphics import Graphics


class WebGraphics(Graphics):
    def __init__(self, width, height):
        super().__init__(width, height)

    def start_scene(self):
        self.active = True
        self.load_archetypes()
        # Initialize web scene (stub)

    def event_handler(self):
        # Stub: handle web events
        pass

    def draw_object(self, obj):
        # Stub: draw object in web
        pass

    def draw_scene(self):
        # Stub: draw scene in web
        pass