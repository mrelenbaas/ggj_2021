import sys

import pygame


BOX_OFFSET = 50
COLOR_BACKGROUND = pygame.Color('#38491a')
COLOR_CLOUD = (0, 0, 0)
COLOR_TEXT = (255, 255, 255)
FONT = 'calibri'
FONT_SIZE = 100
Z_FILL = 4


class Main:

    def __init__(self):
        pygame.init()
        pygame.font.init()
        #self.__window = pygame.display.set_mode((500, 500))
        self.__window = pygame.display.set_mode((0, 0), pygame.FULLSCREEN)

        self.__position = (0, 0)
        self.__x = 0
        self.__y = 0
        self.__cloud_top = pygame.Rect(0, 0, 0, 0)
        self.__cloud_bottom = pygame.Rect(0, 0, 0, 0)
        self.__cloud_left = pygame.Rect(0, 0, 0, 0)
        self.__cloud_right = pygame.Rect(0, 0, 0, 0)

        self.__font = pygame.font.SysFont(FONT, FONT_SIZE)
        self.__text = self.__font.render('---', True, COLOR_TEXT)

        self.__loop()

    def __input(self):
        self.__position = pygame.mouse.get_pos()
        self.__x = self.__position[0]
        self.__y = self.__position[1]
        self.__cloud_top = pygame.Rect(0,
                                       0,
                                       self.__window.get_width(),
                                       self.__y - BOX_OFFSET)
        self.__cloud_bottom = pygame.Rect(0,
                                          self.__y + BOX_OFFSET,
                                          self.__window.get_width(),
                                          self.__window.get_height() - (self.__y + BOX_OFFSET))
        self.__cloud_left = pygame.Rect(0,
                                        self.__y - BOX_OFFSET,
                                        self.__x - BOX_OFFSET,
                                        (self.__y + BOX_OFFSET) - (self.__y - BOX_OFFSET))
        self.__cloud_right = pygame.Rect(self.__x + BOX_OFFSET,
                                         self.__y - BOX_OFFSET,
                                         self.__window.get_width() - self.__x + BOX_OFFSET,
                                         (self.__y + BOX_OFFSET) - (self.__y - BOX_OFFSET))
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

    def __output(self):
        self.__window.fill(COLOR_BACKGROUND)
        pygame.draw.rect(self.__window, COLOR_CLOUD, self.__cloud_top)
        pygame.draw.rect(self.__window, COLOR_CLOUD, self.__cloud_bottom)
        pygame.draw.rect(self.__window, COLOR_CLOUD, self.__cloud_left)
        pygame.draw.rect(self.__window, COLOR_CLOUD, self.__cloud_right)
        self.__text = self.__font.render('{}, {}'.format(str(self.__x).zfill(Z_FILL), str(self.__y).zfill(Z_FILL)),
                                                         True,
                                                         COLOR_TEXT)
        self.__window.blit(self.__text, dest=(0, 0))


    def __quit(self):
        pygame.quit()
        sys.exit()


if __name__ == '__main__':
    main = Main()
