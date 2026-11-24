-- RetroGames · Aula 1 · Etapa 3: tabelas
-- Só estrutura: PK, FK (comportamento padrão), tipos, NOT NULL e UNIQUE.
-- Sem trigger, function, CHECK, ON DELETE CASCADE ou ON DELETE SET NULL.

CREATE DATABASE IF NOT EXISTS retrogames CHARACTER SET utf8mb4;
USE retrogames;

CREATE TABLE tb_condicao_jogo (
    co_condicao_jogo INT         NOT NULL,            -- chave fixa, sem AUTO_INCREMENT
    ds_condicao_jogo VARCHAR(30) NOT NULL,
    PRIMARY KEY (co_condicao_jogo),
    UNIQUE KEY uq_condicao_jogo_ds (ds_condicao_jogo)
);

CREATE TABLE tb_perfil (
    co_perfil INT         NOT NULL,                   -- chave fixa, sem AUTO_INCREMENT
    ds_perfil VARCHAR(30) NOT NULL,
    PRIMARY KEY (co_perfil),
    UNIQUE KEY uq_perfil_ds (ds_perfil)
);

CREATE TABLE tb_plataforma (
    co_seq_plataforma INT         NOT NULL AUTO_INCREMENT,
    no_plataforma     VARCHAR(80) NOT NULL,
    no_fabricante     VARCHAR(80) NOT NULL,
    nu_ano_lancamento INT         NOT NULL,
    PRIMARY KEY (co_seq_plataforma),
    UNIQUE KEY uq_plataforma_no (no_plataforma)
);

CREATE TABLE tb_genero (
    co_seq_genero INT         NOT NULL AUTO_INCREMENT,
    no_genero     VARCHAR(50) NOT NULL,
    PRIMARY KEY (co_seq_genero),
    UNIQUE KEY uq_genero_no (no_genero)
);

CREATE TABLE tb_jogo (
    co_seq_jogo       INT           NOT NULL AUTO_INCREMENT,
    no_jogo           VARCHAR(120)  NOT NULL,
    ds_jogo           TEXT          NULL,             -- única coluna opcional
    nu_ano_lancamento INT           NOT NULL,
    vl_preco          DECIMAL(10,2) NOT NULL,
    qt_estoque        INT           NOT NULL,
    st_ativo          BOOLEAN       NOT NULL DEFAULT TRUE,
    dt_cadastro       DATETIME      NOT NULL,         -- UTC
    co_plataforma     INT           NOT NULL,
    co_condicao_jogo  INT           NOT NULL,
    PRIMARY KEY (co_seq_jogo),
    KEY idx_jogo_no (no_jogo),
    CONSTRAINT fk_jogo_plataforma FOREIGN KEY (co_plataforma)    REFERENCES tb_plataforma (co_seq_plataforma),
    CONSTRAINT fk_jogo_condicao   FOREIGN KEY (co_condicao_jogo) REFERENCES tb_condicao_jogo (co_condicao_jogo)
);

CREATE TABLE tb_jogo_genero (
    co_jogo   INT NOT NULL,
    co_genero INT NOT NULL,
    PRIMARY KEY (co_jogo, co_genero),
    CONSTRAINT fk_jogo_genero_jogo   FOREIGN KEY (co_jogo)   REFERENCES tb_jogo (co_seq_jogo),
    CONSTRAINT fk_jogo_genero_genero FOREIGN KEY (co_genero) REFERENCES tb_genero (co_seq_genero)
);

CREATE TABLE tb_usuario (
    co_seq_usuario INT          NOT NULL AUTO_INCREMENT,
    no_usuario     VARCHAR(100) NOT NULL,
    ds_email       VARCHAR(150) NOT NULL,
    ds_senha_hash  VARCHAR(255) NOT NULL,
    st_ativo       BOOLEAN      NOT NULL DEFAULT TRUE,
    dt_cadastro    DATETIME     NOT NULL,             -- UTC
    co_perfil      INT          NOT NULL,
    PRIMARY KEY (co_seq_usuario),
    UNIQUE KEY uq_usuario_email (ds_email),
    CONSTRAINT fk_usuario_perfil FOREIGN KEY (co_perfil) REFERENCES tb_perfil (co_perfil)
);
