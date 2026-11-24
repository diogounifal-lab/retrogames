-- RetroGames · Aula 1 · Etapa 5: testes de integridade
-- EXECUTE UM TESTE POR VEZ no Workbench (o script para no primeiro erro).
-- Os resultados abaixo são os ESPERADOS; confira no seu banco e troque por "obtido: ..." se quiser.
USE retrogames;

-- Teste 1: jogo com plataforma inexistente. Esperado: erro 1452 (chave estrangeira)
INSERT INTO tb_jogo (no_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES ('Teste FK', 2000, 10.00, 1, TRUE, UTC_TIMESTAMP(), 999, 1);

-- Teste 2: plataforma com nome repetido. Esperado: erro 1062 (UNIQUE)
INSERT INTO tb_plataforma (no_plataforma, no_fabricante, nu_ano_lancamento) VALUES ('Super Nintendo', 'Nintendo', 1990);

-- Teste 3: excluir plataforma que possui jogos. Esperado: erro 1451 (chave estrangeira)
DELETE FROM tb_plataforma WHERE co_seq_plataforma = 1;

-- Teste 4: jogo com no_jogo nulo. Esperado: erro 1048 (NOT NULL)
INSERT INTO tb_jogo (no_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES (NULL, 2000, 10.00, 1, TRUE, UTC_TIMESTAMP(), 1, 1);

-- Teste 5: jogo com preço 0. Esperado: ACEITO (a regra RN06 fica no Service, não no banco)
INSERT INTO tb_jogo (no_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES ('Teste preco zero', 2000, 0, 1, TRUE, UTC_TIMESTAMP(), 1, 1);

-- LIMPEZA: remove o que os testes inseriram
DELETE FROM tb_jogo WHERE no_jogo = 'Teste preco zero';
