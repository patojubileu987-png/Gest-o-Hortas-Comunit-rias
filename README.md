# Gestão de Hortas Comunitárias

Este projeto contém uma solução .NET para gerenciar canteiros e cultivos em memória.

## O que há no projeto

- `MeuProjeto.csproj`: projeto original da solução.
- `src/GestaoHortas.Library/`: biblioteca de domínio e infraestrutura.
  - `Application/Interfaces/ICanteiroRepository.cs`: interface de repositório para canteiros.
  - `Application/Interfaces/ICultivoRepository.cs`: interface de repositório para cultivos.
  - `Domain/Entities/`: entidades principais como `Canteiro` e `Cultivo`.
  - `Domain/Services/CultivoCompatibilityService.cs`: serviço de compatibilidade de cultivos.
  - `Infra/Repositories/InMemoryCanteiroRepository.cs`: repositório em memória para canteiros.
  - `Infra/Repositories/InMemoryCultivoRepository.cs`: repositório em memória para cultivos.

- `src/GestaoHortas.Blazor/`: aplicativo Blazor com interface web.
  - Menu lateral de navegação.
  - Páginas para cadastrar, atualizar, listar e remover canteiros.
  - Páginas para cadastrar, pesquisar e remover cultivos.
  - Serviço de compatibilidade de cultivos em memória.
  - Configuração para execução em `http://localhost:5142`.

## Como executar

1. Abra o terminal na pasta do projeto:
   ```powershell
   cd c:\Users\utfpr\Documents\ProjetoMeu
   ```
2. Execute o aplicativo Blazor:
   ```powershell
   dotnet run --project .\src\GestaoHortas.Blazor\GestaoHortas.Blazor.csproj
   ```
3. Acesse no navegador:
   ```text
   http://localhost:5142
   ```

## Observações

- A aplicação usa persistência em memória, não há banco de dados.
- As alterações não são salvas entre execuções.
- O projeto Blazor foi criado para fornecer uma interface de registro visual com rotas e menus.
