-- RetroGames · Aula 1 · Etapa 4: dados de exemplo (rode depois do 01_tabelas.sql)
-- tb_usuario fica vazia: o administrador é criado pela API (aula 6).
USE retrogames;

INSERT INTO tb_condicao_jogo (co_condicao_jogo, ds_condicao_jogo) VALUES
 (1, 'Novo'), (2, 'Usado'), (3, 'Recondicionado');

INSERT INTO tb_perfil (co_perfil, ds_perfil) VALUES
 (1, 'Administrador'), (2, 'Cliente');

INSERT INTO tb_plataforma (co_seq_plataforma, no_plataforma, no_fabricante, nu_ano_lancamento) VALUES
 (1, 'NES',            'Nintendo', 1983),
 (2, 'Super Nintendo', 'Nintendo', 1990),
 (3, 'Mega Drive',     'Sega',     1988),
 (4, 'PlayStation',    'Sony',     1994),
 (5, 'Nintendo 64',    'Nintendo', 1996),
 (6, 'Master System',  'Sega',     1985),
 (7, 'Atari 2600',     'Atari',    1977);   -- sem jogos de propósito (teste da contagem com 0)

INSERT INTO tb_genero (co_seq_genero, no_genero) VALUES
 (1, 'Ação'), (2, 'Aventura'), (3, 'Plataforma'), (4, 'RPG'),
 (5, 'Luta'), (6, 'Corrida'), (7, 'Esporte'), (8, 'Quebra-cabeça');

-- Jogos (20). #14 tem estoque zero; #20 é inativo.
INSERT INTO tb_jogo (co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo) VALUES
 ( 1, 'Super Mario Bros.',                 'O clássico de plataforma que definiu uma geração.',          1985,  89.90, 5, TRUE,  '2025-01-10 12:00:00', 1, 2),
 ( 2, 'The Legend of Zelda',               'Explore Hyrule e resgate a princesa.',                       1986, 129.90, 3, TRUE,  '2025-01-10 12:00:00', 1, 2),
 ( 3, 'Metroid',                           NULL,                                                          1986,  99.90, 2, TRUE,  '2025-01-10 12:00:00', 1, 2),
 ( 4, 'Super Mario World',                 'Mario e Yoshi em Dinosaur Land.',                             1990, 149.90, 4, TRUE,  '2025-01-10 12:00:00', 2, 2),
 ( 5, 'Super Metroid',                     'Samus volta a Zebes.',                                        1994, 199.90, 2, TRUE,  '2025-01-10 12:00:00', 2, 3),
 ( 6, 'Chrono Trigger',                    'RPG com viagem no tempo.',                                    1995, 399.90, 1, TRUE,  '2025-01-10 12:00:00', 2, 2),
 ( 7, 'Street Fighter II',                 NULL,                                                          1991, 119.90, 6, TRUE,  '2025-01-10 12:00:00', 2, 2),
 ( 8, 'Sonic the Hedgehog',                'O ouriço mais rápido dos games.',                             1991,  79.90, 7, TRUE,  '2025-01-10 12:00:00', 3, 2),
 ( 9, 'Streets of Rage 2',                 'Briga de rua com trilha lendária.',                           1992, 109.90, 3, TRUE,  '2025-01-10 12:00:00', 3, 3),
 (10, 'Phantasy Star IV',                  NULL,                                                          1993, 249.90, 1, TRUE,  '2025-01-10 12:00:00', 3, 2),
 (11, 'Final Fantasy VII',                 'Cloud contra a Shinra.',                                      1997, 179.90, 3, TRUE,  '2025-01-10 12:00:00', 4, 2),
 (12, 'Crash Bandicoot',                   NULL,                                                          1996,  89.90, 5, TRUE,  '2025-01-10 12:00:00', 4, 2),
 (13, 'Gran Turismo',                      'Simulador de corrida.',                                       1997,  69.90, 4, TRUE,  '2025-01-10 12:00:00', 4, 3),
 (14, 'Tekken 3',                          'Luta em 3D.',                                                 1997,  99.90, 0, TRUE,  '2025-01-10 12:00:00', 4, 2),
 (15, 'Super Mario 64',                    'Mario entra no 3D.',                                          1996, 199.90, 3, TRUE,  '2025-01-10 12:00:00', 5, 2),
 (16, 'The Legend of Zelda: Ocarina of Time', 'Link atravessa o tempo.',                                  1998, 299.90, 2, TRUE,  '2025-01-10 12:00:00', 5, 2),
 (17, 'Mario Kart 64',                     NULL,                                                          1996, 159.90, 4, TRUE,  '2025-01-10 12:00:00', 5, 3),
 (18, 'Alex Kidd in Miracle World',        NULL,                                                          1986,  59.90, 2, TRUE,  '2025-01-10 12:00:00', 6, 2),
 (19, 'Wonder Boy III: The Dragon''s Trap', 'Aventura com transformações.',                               1989,  69.90, 3, TRUE,  '2025-01-10 12:00:00', 6, 2),
 (20, 'Dr. Robotnik''s Mean Bean Machine', 'Puzzle com os inimigos do Sonic.',                            1993,  59.90, 1, FALSE, '2025-01-10 12:00:00', 3, 2);

-- Gêneros de cada jogo (1 a 3 por jogo)
INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES
 (1,3), (2,2), (2,1), (3,1), (3,2),
 (4,3), (5,1), (5,2), (6,4), (6,2), (7,5),
 (8,3), (8,1), (9,1), (10,4), (10,2),
 (11,4), (11,2), (12,3), (13,6), (14,5),
 (15,3), (15,2), (16,1), (16,2), (16,4), (17,6),
 (18,3), (18,2), (19,3), (19,1), (20,8);
