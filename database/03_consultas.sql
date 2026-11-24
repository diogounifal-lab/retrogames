-- RetroGames · Aula 1 · Etapa 5: consultas do catálogo
USE retrogames;

-- 1. Catálogo: jogos ativos com plataforma e condição, ordenados por nome
SELECT j.co_seq_jogo, j.no_jogo, p.no_plataforma, c.ds_condicao_jogo, j.vl_preco, j.qt_estoque
FROM tb_jogo j
JOIN tb_plataforma p    ON p.co_seq_plataforma = j.co_plataforma
JOIN tb_condicao_jogo c ON c.co_condicao_jogo  = j.co_condicao_jogo
WHERE j.st_ativo = TRUE
ORDER BY j.no_jogo;

-- 2. Gêneros de um jogo informado (jogo 16)
SELECT g.no_genero
FROM tb_jogo_genero jg
JOIN tb_genero g ON g.co_seq_genero = jg.co_genero
WHERE jg.co_jogo = 16
ORDER BY g.no_genero;

-- 3. Jogos de um gênero informado (gênero 3 = Plataforma)
SELECT j.co_seq_jogo, j.no_jogo
FROM tb_jogo_genero jg
JOIN tb_jogo j ON j.co_seq_jogo = jg.co_jogo
WHERE jg.co_genero = 3
ORDER BY j.no_jogo;

-- 4. Quantidade de jogos por plataforma, incluindo plataformas sem jogos (Atari 2600 = 0)
SELECT p.no_plataforma, COUNT(j.co_seq_jogo) AS qt_jogos
FROM tb_plataforma p
LEFT JOIN tb_jogo j ON j.co_plataforma = p.co_seq_plataforma
GROUP BY p.co_seq_plataforma, p.no_plataforma
ORDER BY p.no_plataforma;

-- 5. Busca por parte do nome, 5 itens por página (página 1; para a página 2, OFFSET 5)
SELECT co_seq_jogo, no_jogo
FROM tb_jogo
WHERE no_jogo LIKE '%mario%'
ORDER BY no_jogo
LIMIT 5 OFFSET 0;

-- Conferências do checklist da etapa 4
-- SELECT COUNT(*) FROM tb_plataforma; SELECT COUNT(*) FROM tb_genero; SELECT COUNT(*) FROM tb_jogo;
-- Jogos sem gênero (deve vir vazio):
SELECT j.co_seq_jogo, j.no_jogo FROM tb_jogo j
LEFT JOIN tb_jogo_genero jg ON jg.co_jogo = j.co_seq_jogo WHERE jg.co_jogo IS NULL;
