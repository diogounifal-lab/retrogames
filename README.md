# RetroGames

Projeto do curso (aulas 1 a 5): banco, modelos, Repository, Service e API (leitura e gravação).

## Como rodar
1. MySQL 8 no ar. No Workbench, rode em ordem: `database/01_tabelas.sql` e `database/02_dados_exemplo.sql`.
2. Copie `.env.example` para `.env` e troque `SUA_SENHA` pela senha do seu MySQL.
3. `dotnet build` na raiz.
4. `dotnet run --project RetroGames.WebApi` e abra http://localhost:5080/swagger
5. Front (só a página inicial por enquanto): `cd retrogames.web && npm install && npm run dev`

## Falta colocar
- `AGENTS.md` na raiz (o arquivo do professor).
- Repositório Git, commits por etapa e quadro de tarefas (veja a lista do roteiro de cada aula).
