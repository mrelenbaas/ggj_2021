import sys

import pygame

import server


BOX_OFFSET = 200
COLOR_BACKGROUND = pygame.Color('#38491a')
COLOR_CENTER = (255, 0, 0)
COLOR_CLOUD = (0, 0, 0)
COLOR_TEXT = (255, 255, 255)
COLOR_RED = (255, 0, 0)
COLOR_GREEN = (0, 255, 0)
COLOR_BLUE = (0, 0, 255)
FONT = 'calibri'
FONT_SIZE = 100
Z_FILL = 4


class Main:

    def __init__(self):
        self.server = server.Server()
        pygame.init()
        pygame.font.init()
        #self.__window = pygame.display.set_mode((500, 500))
        self.__window = pygame.display.set_mode((0, 0), pygame.FULLSCREEN)
        self.__width = self.__window.get_width()
        self.__height = self.__window.get_height()

        self.__position = (0, 0)
        self.__x = 0
        self.__y = 0
        self.__cloud_top = pygame.Rect(0, 0, 0, 0)
        self.__cloud_bottom = pygame.Rect(0, 0, 0, 0)
        self.__cloud_left = pygame.Rect(0, 0, 0, 0)
        self.__cloud_right = pygame.Rect(0, 0, 0, 0)

        self.__font = pygame.font.SysFont(FONT, FONT_SIZE)
        self.__text = self.__font.render('---', True, COLOR_TEXT)
        self.__message = ''
        self.__message_previous = ''
        self.__message_surface = None
        self.__width_percentage = 0.0
        self.__height_percentage = 0.0
        self.__cloud2_top = pygame.Rect(0, 0, 0, 0)
        self.__cloud2_bottom = pygame.Rect(0, 0, 0, 0)
        self.__cloud2_left = pygame.Rect(0, 0, 0, 0)
        self.__cloud2_right = pygame.Rect(0, 0, 0, 0)

        self.__background = pygame.image.load('coding_monster.png')
        self.__background = pygame.transform.scale(self.__background, (self.__width, self.__height))

        self.__loop()

    def __update_clouds(self, x, y):
        cloud_top = pygame.Rect(0,
                                0,
                                self.__window.get_width(),
                                y - BOX_OFFSET)
        cloud_bottom = pygame.Rect(0,
                                   y + BOX_OFFSET,
                                   self.__window.get_width(),
                                   self.__window.get_height() - (y + BOX_OFFSET))
        cloud_left = pygame.Rect(0,
                                 y - BOX_OFFSET,
                                 x - BOX_OFFSET,
                                 (y + BOX_OFFSET) - (y - BOX_OFFSET))
        cloud_right = pygame.Rect(x + BOX_OFFSET,
                                  y - BOX_OFFSET,
                                  self.__window.get_width() - x + BOX_OFFSET,
                                  (y + BOX_OFFSET) - (y - BOX_OFFSET))
        return (cloud_top, cloud_bottom, cloud_left, cloud_right)

    def __input(self):
        self.__position = pygame.mouse.get_pos()
        self.__x = self.__position[0]
        self.__y = self.__position[1]
        self.server.setPercentages((self.__x / self.__width), (self.__y / self.__height))
        self.__cloud_top, self.__cloud_bottom, self.__cloud_left, self.__cloud_right = self.__update_clouds(self.__x,
                                                                                                            self.__y)
        self.__cloud2_top, self.__cloud2_bottom, self.__cloud2_left, self.__cloud2_right = self.__update_clouds((self.__width / 2.0) + ((self.__width / 2.0) * self.__width_percentage),
                                                                                                                (self.__height / 2.0) + -((self.__height / 2.0) * self.__height_percentage))
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                self.__quit()
            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_ESCAPE:
                    self.__quit()

    def __loop(self):
        while True:
            self.__input()
            self.__output()
            pygame.display.update()
            self.__message = self.server.server_function()

    def __outline(self, r, c):
        pygame.draw.line(self.__window, c, pygame.Vector2(r.x, r.y),
                         pygame.Vector2(r.topright, r.top));
        pygame.draw.line(self.__window, c, pygame.Vector2(r.topright, r.top),
                         pygame.Vector2(r.bottomright, r.bottom));
        pygame.draw.line(self.__window, c, pygame.Vector2(r.bottomright, r.bottom),
                         pygame.Vector2(r.bottomleft, r.bottom));
        pygame.draw.line(self.__window, c, pygame.Vector2(r.bottomleft, r.bottom),
                         pygame.Vector2(r.x, r.y));

    def __output_clouds(self, cloud_top, cloud_bottom, cloud_left, cloud_right, c):

        pygame.draw.rect(self.__window, c, cloud_top)
        pygame.draw.rect(self.__window, c, cloud_bottom)
        pygame.draw.rect(self.__window, c, cloud_left)
        pygame.draw.rect(self.__window, c, cloud_right)
        #self.__outline(cloud_top, c)
        #self.__outline(cloud_bottom, c)
        #self.__outline(cloud_left, c)
        #self.__outline(cloud_right, c)

    def __output(self):
        #self.__window.fill(COLOR_BACKGROUND)
        self.__window.blit(self.__background, (0, 0))
        self.__output_clouds(self.__cloud2_top, self.__cloud2_bottom, self.__cloud2_left, self.__cloud2_right, COLOR_GREEN)
        self.__output_clouds(self.__cloud_top, self.__cloud_bottom, self.__cloud_left, self.__cloud_right, COLOR_RED)
        pygame.draw.rect(self.__window, COLOR_CENTER, pygame.Rect(((self.__width / 2.0) - 25.0), ((self.__height / 2.0) - 25.0), 50, 50))
        self.__text = self.__font.render('{}, {}'.format(str(self.__x).zfill(Z_FILL), str(self.__y).zfill(Z_FILL)),
                                                         True,
                                                         COLOR_TEXT)
        self.__window.blit(self.__text, dest=(0, 0))
        if self.__message != 'feather':
            try:
                messages = self.__message.split(',')
                width_percentage = float(messages[0])
                height_percentage = float(messages[1])
                self.__width_percentage = width_percentage
                self.__height_percentage = height_percentage
                message = '{},{}'.format(self.__width_percentage, self.__height_percentage)

                #sself.__message = self.__message.split(',')
                #if len(self.__message) < 4:
                #    return
                #self.__message = '{},{}'.format(self.__message[0], self.__message[1])
                self.__message_surface = self.__font.render(message,
                                                            True,
                                                            COLOR_TEXT)
                self.__window.blit(self.__message_surface, dest=(0, 100))
                self.__message_previous = message
            except:
                pass
        else:
            self.__message_surface = self.__font.render(self.__message_previous,
                                                        True,
                                                        COLOR_TEXT)
            self.__window.blit(self.__message_surface, dest=(0, 100))


    def __quit(self):
        pygame.quit()
        sys.exit()


if __name__ == '__main__':
    main = Main()
