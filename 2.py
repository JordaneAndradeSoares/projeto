import pygame
import sys

# Inicialização do Pygame
pygame.init()

# Definindo cores
cor_do_fundo= (0, 0, 0)
cor_das_letras = (255, 0, 0)

# Configurações da tela
x, y = 800, 600
FPS = 60

# Configurações do jogador
player_health = 100
player_damage = 10

# Configurações do inimigo
enemy_health = 50
enemy_damage = 5

# Inicialização da tela
screen = pygame.display.set_mode((x, y))
pygame.display.set_caption("Sistema de Luta por Turnos")

tamanho_das_imagens_x, tamanho_das_imagens_y = 200, 250

# Carregando imagens
player_image = pygame.image.load("C:\\Users\\Jordane A. Soares\\Pictures\\projetos\\projeto jogo 15\\tudo 2\\imagens\\1. verme.jpeg");
enemy_image = pygame.image.load("C:\\Users\\Jordane A. Soares\\Pictures\\projetos\\projeto jogo 15\\tudo 2\\imagens\\1. verme.jpeg");
player_image = pygame.transform.scale(player_image, (tamanho_das_imagens_x, tamanho_das_imagens_y))
enemy_image = pygame.transform.scale(player_image, (tamanho_das_imagens_x, tamanho_das_imagens_y))

player_rect = player_image.get_rect()
player_rect.topleft = (50, x // 2 - player_rect.height // 2)

enemy_rect = enemy_image.get_rect()
enemy_rect.topleft = (y - 50 - enemy_rect.width, x // 2 - enemy_rect.height // 2)


# Relógio para controlar o FPS
clock = pygame.time.Clock()

def draw_text(surf, text, size, x, y, color):
    font = pygame.font.Font(None, size)
    text_surface = font.render(text, True, color)
    text_rect = text_surface.get_rect(center=(x, y))
    surf.blit(text_surface, text_rect)

def main():
    global player_health, enemy_health

    player_turn = True


    while True:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
                sys.exit()

            if event.type == pygame.KEYDOWN and event.key == pygame.K_SPACE:
                if player_turn:
                    enemy_health -= player_damage
                    player_turn = False
                else:
                    player_health -= enemy_damage
                    player_turn = True

        screen.fill(cor_do_fundo)

        # Desenhando imagens
        screen.blit(player_image, player_rect)
        screen.blit(enemy_image, enemy_rect)

        draw_text(screen, f"Jogador: {player_health} HP", 30, x // 9, y// 8, cor_das_letras)
        draw_text(screen, f"Inimigo: {enemy_health} HP", 30, 5 * (x // 9), y // 8, cor_das_letras)

        if player_health <= 0:
            draw_text(screen, "Você perdeu!", 50, x // 2, y // 2 - 50, cor_das_letras)
        elif enemy_health <= 0:
            draw_text(screen, "Você venceu!", 50, x // 2, y // 2 - 50, cor_das_letras)

        pygame.display.flip()
        clock.tick(FPS)

if __name__ == "__main__":
    main()
