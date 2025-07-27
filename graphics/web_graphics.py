class WebGraphics:
    def __init__(self, width, height):
        self.width = width
        self.height = height
        self.active = True
        self.events = {}

    def load_archetypes(self):
        # Load archetypes as needed for web
        pass

    def set_event(self, event, function):
        self.events[event] = function

    def start_scene(self):
        self.active = True
        self.load_archetypes()
        # Initialize web scene (stub)

    def end_scene(self):
        self.active = False
        if "end" in self.events:
            self.events["end"]()

    def event_handler(self):
        # Stub: handle web events
        pass

    def draw_object(self, obj):
        # Stub: draw object in web
        pass

    def draw_scene(self):
        # Stub: draw scene in web
        pass