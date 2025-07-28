import pygame
from .graphics import Graphics


class PygameGraphics(Graphics):
    def __init__(self, width, height):
        super().__init__(width, height)
        pygame.init()
        self.screen = None

    def start_scene(self):
        self.active = True
        size = self.width, self.height
        self.load_archetypes()
        self.screen = pygame.display.set_mode(size)

    def event_handler(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                self.end_scene()
            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_LEFT:
                    if "key_press" in self.events:
                        self.events["key_press"]("LEFT")
                elif event.key == pygame.K_RIGHT:
                    if "key_press" in self.events:
                        self.events["key_press"]("RIGHT")
                elif event.key == pygame.K_UP:
                    if "key_press" in self.events:
                        self.events["key_press"]("UP")
                elif event.key == pygame.K_DOWN:
                    if "key_press" in self.events:
                        self.events["key_press"]("DOWN")
                elif event.key == pygame.K_SPACE:
                    if "key_press" in self.events:
                        self.events["key_press"]("SPACE")
                elif event.key == pygame.K_ESCAPE:
                    self.end_scene()
            if event.type == pygame.KEYUP:
                if event.key == pygame.K_LEFT:
                    if "key_up" in self.events:
                        self.events["key_up"]("LEFT")
                elif event.key == pygame.K_RIGHT:
                    if "key_up" in self.events:
                        self.events["key_up"]("RIGHT")
                elif event.key == pygame.K_UP:
                    if "key_up" in self.events:
                        self.events["key_up"]("UP")
                elif event.key == pygame.K_DOWN:
                    if "key_up" in self.events:
                        self.events["key_up"]("DOWN")
                elif event.key == pygame.K_ESCAPE:
                    self.end_scene()

    def draw_object(self, obj):
        if obj and self.screen:
            arq = self.archetypes[obj.archetype]
            
            if arq.get("type") == "rect":
                position = (
                    obj.position.x,
                    obj.position.y,
                    arq.get("width"),
                    arq.get("height")
                )
                pygame.draw.rect(self.screen, obj.color, position, 0)

    def draw_scene(self):
        if self.screen:
            self.screen.fill((0, 0, 0))
            if "objects" in self.events:
                for obj in self.events["objects"]():
                    self.draw_object(obj)
            pygame.display.update()
