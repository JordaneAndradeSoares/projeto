import pygame
import sys

pygame.init()

vida_maxima = 2000
velocidade = 100
ataque_especial = 100
ataque_fisico = 10000
defesa_especial = 100
defesa_fisica = 100
armazenamento = 100
modificador = 10
r = True
x1, y1 = 200, 500
x2, y2 = 100, 500
fps = 20
variavel = True

valor_aliado = vida_maxima
valor_inimigo = vida_maxima

tela = pygame.display.set_mode((900, 600))
pygame.display.set_caption("Projeto")

font = pygame.font.Font(None, 36)
fonte = pygame.font.Font(None, 100)

def dano(ataque_fisico, defesa_fisica, modificador):
    a = (ataque_fisico * (modificador / 100)) - defesa_fisica
    return max(0, a)

clock = pygame.time.Clock()

while r:
    keys = pygame.key.get_pressed()

    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            r = False

    tela.fill((0, 0, 0))
    clock.tick(fps)

    inimigo = pygame.draw.rect(tela, (255, 0, 0), (x1, y1, 40, 40))
    aliado = pygame.draw.rect(tela, (0, 0, 255), (x2, y2, 40, 40))

    if keys[pygame.K_LEFT]:
        x1 -= 20
    if keys[pygame.K_RIGHT]:
        x1 += 20

    if aliado.colliderect(inimigo):
        keys = pygame.key.get_pressed()
        variavel = False

    if not variavel:
        if valor_inimigo < 0 and valor_aliado > 0:
            c = "Você venceu!"
            text_surface = font.render(c, True, (255, 255, 255))
            tela.blit(text_surface, (300, 300))
        else:
            x2, y2 = 350, 200
            info_text = f"Vida aliado: {valor_aliado}"
            text_surface = font.render(info_text, True, (255, 255, 255))
            tela.blit(text_surface, (10, 10))

            x1, y1 = 10, 200
            info_text = f"Vida inimigo: {valor_inimigo}"
            text_surface = font.render(info_text, True, (255, 255, 255))
            tela.blit(text_surface, (350, 10))

        if keys[pygame.K_s]:
            valor_inimigo -= dano(ataque_fisico, defesa_fisica, modificador)

    pygame.display.flip()

pygame.quit()
sys.exit()