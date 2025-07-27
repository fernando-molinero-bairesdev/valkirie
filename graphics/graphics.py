import pygame
import json

class Graphics():
    def __init__(self, width, height):
        pygame.init()
        self.active = True
        self.width = width
        self.height = height
        self.events = {}

    def load_archetypes(self):
        with open('./tetris/objects/archetypes.json') as f:
            self.archetypes = json.load(f)
    
    def set_event(self, event, function):
        self.events[event] = function

    def start_scene(self):
        self.active = True
        size = self.width, self.height
        self.load_archetypes()
        self.screen = pygame.display.set_mode(size)

    def end_scene(self):
        self.active = False
        self.events["end"]()

    def event_handler(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                self.end_scene()
            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_LEFT:
                    self.events["key_press"]("LEFT")
                elif event.key == pygame.K_RIGHT:
                    self.events["key_press"]("RIGHT")
                elif event.key == pygame.K_UP:
                    self.events["key_press"]("UP")
                elif event.key == pygame.K_DOWN:
                    self.events["key_press"]("DOWN")
                elif event.key == pygame.K_SPACE:
                    self.events["key_press"]("SPACE")
                elif event.key == pygame.K_ESCAPE:
                    self.end_scene()
            if event.type == pygame.KEYUP:
                if event.key == pygame.K_LEFT:
                    self.events["key_up"]("LEFT")
                elif event.key == pygame.K_RIGHT:
                    self.events["key_up"]("RIGHT")
                elif event.key == pygame.K_UP:
                    self.events["key_up"]("UP")
                elif event.key == pygame.K_DOWN:
                    self.events["key_up"]("DOWN")
                elif event.key == pygame.K_ESCAPE:
                    self.end_scene()

    def draw_object(self, obj):
        if obj:
            arq = self.archetypes[obj.archetype]
            
            if arq.get("type") == "rect":
                white = (0,255,255)
                position = (obj.position.x, obj.position.y, arq.get("width"),arq.get("height"))
                pygame.draw.rect(self.screen,obj.color,position,0)

    def draw_scene(self):
        self.screen.fill((0,0,0))
        for object in self.events["objects"]():
            self.draw_object(object)
        pygame.display.update()  