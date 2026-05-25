using System;
using GestaoHortas.Library.Application.Interfaces;
using GestaoHortas.Library.Infra.Repositories;
using GestaoHortas.Library.Domain.Entities;
using GestaoHortas.Library.Domain.Services;
using GestaoHortas.Library.Domain.ValueObjects;

// Configuração da camada de aplicação
var canteiroRepository = new InMemoryCanteiroRepository();
var cultivoCompatibilityService = new CultivoCompatibilityService();

var menuService = new ConsoleMenuService(canteiroRepository, cultivoCompatibilityService);
menuService.Execute();

/// <summary>
/// Serviço de menu interativo do console
/// </summary>
public class ConsoleMenuService
{
    private readonly ICanteiroRepository _canteiroRepository;
    private readonly CultivoCompatibilityService _cultivoCompatibilityService;
    private readonly Dictionary<Guid, List<Cultivo>> _cultivosPorCanteiro = new();

    public ConsoleMenuService(ICanteiroRepository canteiroRepository, CultivoCompatibilityService cultivoCompatibilityService)
    {
        _canteiroRepository = canteiroRepository;
        _cultivoCompatibilityService = cultivoCompatibilityService;
    }

    public void Execute()
    {
        bool executando = true;

        while (executando)
        {
            ExibirMenuPrincipal();
            var opcao = Console.ReadLine() ?? "0";

            switch (opcao)
            {
                case "1":
                    MenuCanteiros();
                    break;
                case "2":
                    MenuCultivos();
                    break;
                case "0":
                    executando = false;
                    Console.WriteLine("\nAté logo!");
                    break;
                default:
                    Console.WriteLine("\nOpção inválida. Tente novamente.");
                    break;
            }

            if (executando)
            {
                Console.WriteLine("\nPressione ENTER para continuar...");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }

    private void ExibirMenuPrincipal()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║  SISTEMA DE GESTÃO DE HORTAS           ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.WriteLine("\n1 - Gerenciar Canteiros");
        Console.WriteLine("2 - Gerenciar Cultivos");
        Console.WriteLine("0 - Sair");
        Console.Write("\nEscolha uma opção: ");
    }

    private void MenuCanteiros()
    {
        bool voltarAoMenu = false;

        while (!voltarAoMenu)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║  GERENCIAR CANTEIROS                   ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine("\n1 - Listar Canteiros");
            Console.WriteLine("2 - Criar Novo Canteiro");
            Console.WriteLine("3 - Atualizar Canteiro");
            Console.WriteLine("4 - Remover Canteiro");
            Console.WriteLine("0 - Voltar ao Menu Principal");
            Console.Write("\nEscolha uma opção: ");

            var opcao = Console.ReadLine() ?? "0";

            switch (opcao)
            {
                case "1":
                    ListarCanteiros();
                    break;
                case "2":
                    CriarCanteiro();
                    break;
                case "3":
                    AtualizarCanteiro();
                    break;
                case "4":
                    RemoverCanteiro();
                    break;
                case "0":
                    voltarAoMenu = true;
                    break;
                default:
                    Console.WriteLine("\nOpção inválida.");
                    break;
            }

            if (!voltarAoMenu && opcao != "0")
            {
                Console.WriteLine("\nPressione ENTER para continuar...");
                Console.ReadLine();
            }
        }
    }

    private void ListarCanteiros()
    {
        Console.Clear();
        var canteiros = _canteiroRepository.GetAll().ToList();

        if (!canteiros.Any())
        {
            Console.WriteLine("Nenhum canteiro cadastrado.");
            return;
        }

        Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  CANTEIROS CADASTRADOS                                         ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        foreach (var canteiro in canteiros)
        {
            Console.WriteLine($"\nID: {canteiro.Id}");
            Console.WriteLine($"Nome: {canteiro.Nome}");
            Console.WriteLine($"Área: {canteiro.Area} m²");
            Console.WriteLine($"Tipo de Solo: {canteiro.TipoSolo}");
            Console.WriteLine($"Localização: {canteiro.Localizacao.Endereco} ({canteiro.Localizacao.Referencia})");
            Console.WriteLine($"Status: {canteiro.Status}");
            
            if (_cultivosPorCanteiro.TryGetValue(canteiro.Id, out var cultivos))
            {
                Console.WriteLine($"Cultivos: {cultivos.Count}");
            }
            Console.WriteLine("─────────────────────────────────────────────────────────────");
        }
    }

    private void CriarCanteiro()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║  CRIAR NOVO CANTEIRO                   ║");
        Console.WriteLine("╚════════════════════════════════════════╝");

        Console.Write("\nNome do Canteiro: ");
        var nome = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("Nome não pode estar vazio.");
            return;
        }

        Console.Write("Área (em m²): ");
        if (!decimal.TryParse(Console.ReadLine(), out var area) || area <= 0)
        {
            Console.WriteLine("Área inválida.");
            return;
        }

        Console.Write("Endereço: ");
        var endereco = Console.ReadLine() ?? "";

        Console.Write("Referência: ");
        var referencia = Console.ReadLine() ?? "";

        Console.WriteLine("\nTipo de Solo:");
        Console.WriteLine("1 - Argiloso");
        Console.WriteLine("2 - Arenoso");
        Console.WriteLine("3 - Humoso");
        Console.WriteLine("4 - Misto");
        Console.Write("Escolha: ");
        
        var tipoSoloInput = Console.ReadLine() ?? "1";
        var tipoSolo = tipoSoloInput switch
        {
            "1" => TipoSolo.Argiloso,
            "2" => TipoSolo.Arenoso,
            "3" => TipoSolo.Humoso,
            "4" => TipoSolo.Misto,
            _ => TipoSolo.Misto
        };

        var canteiro = new Canteiro
        {
            Nome = nome,
            Area = area,
            TipoSolo = tipoSolo,
            Localizacao = new Localizacao { Endereco = endereco, Referencia = referencia },
            Status = StatusCanteiro.Ativo
        };

        _canteiroRepository.Add(canteiro);
        _cultivosPorCanteiro[canteiro.Id] = new List<Cultivo>();

        Console.WriteLine($"\n✓ Canteiro '{nome}' criado com sucesso! (ID: {canteiro.Id})");
    }

    private void AtualizarCanteiro()
    {
        Console.Clear();
        var canteiros = _canteiroRepository.GetAll().ToList();

        if (!canteiros.Any())
        {
            Console.WriteLine("Nenhum canteiro para atualizar.");
            return;
        }

        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║  ATUALIZAR CANTEIRO                    ║");
        Console.WriteLine("╚════════════════════════════════════════╝");

        ListarCanteirosSimples(canteiros);

        Console.Write("\nEscolha o ID do canteiro: ");
        if (!Guid.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        var canteiro = _canteiroRepository.GetById(id);
        if (canteiro == null)
        {
            Console.WriteLine("Canteiro não encontrado.");
            return;
        }

        Console.Write($"\nNovo nome (atual: {canteiro.Nome}): ");
        var novoNome = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(novoNome))
            canteiro.Nome = novoNome;

        Console.Write($"Novo status (atual: {canteiro.Status}) - [Ativo/EmManutencao/Inativo]: ");
        var novoStatus = Console.ReadLine() ?? "";
        canteiro.Status = novoStatus switch
        {
            "EmManutencao" => StatusCanteiro.EmManutencao,
            "Inativo" => StatusCanteiro.Inativo,
            "Ativo" => StatusCanteiro.Ativo,
            _ => canteiro.Status
        };

        _canteiroRepository.Update(canteiro);
        Console.WriteLine("\n✓ Canteiro atualizado com sucesso!");
    }

    private void RemoverCanteiro()
    {
        Console.Clear();
        var canteiros = _canteiroRepository.GetAll().ToList();

        if (!canteiros.Any())
        {
            Console.WriteLine("Nenhum canteiro para remover.");
            return;
        }

        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║  REMOVER CANTEIRO                      ║");
        Console.WriteLine("╚════════════════════════════════════════╝");

        ListarCanteirosSimples(canteiros);

        Console.Write("\nEscolha o ID do canteiro a remover: ");
        if (!Guid.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        var canteiro = _canteiroRepository.GetById(id);
        if (canteiro == null)
        {
            Console.WriteLine("Canteiro não encontrado.");
            return;
        }

        Console.Write($"\nTem certeza que deseja remover '{canteiro.Nome}'? (s/n): ");
        if (Console.ReadLine()?.ToLower() == "s")
        {
            _canteiroRepository.Remove(id);
            _cultivosPorCanteiro.Remove(id);
            Console.WriteLine("\n✓ Canteiro removido com sucesso!");
        }
        else
        {
            Console.WriteLine("\nOperação cancelada.");
        }
    }

    private void MenuCultivos()
    {
        bool voltarAoMenu = false;

        while (!voltarAoMenu)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║  GERENCIAR CULTIVOS                    ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine("\n1 - Listar Cultivos");
            Console.WriteLine("2 - Adicionar Cultivo");
            Console.WriteLine("3 - Atualizar Cultivo");
            Console.WriteLine("4 - Remover Cultivo");
            Console.WriteLine("5 - Verificar Compatibilidade");
            Console.WriteLine("0 - Voltar ao Menu Principal");
            Console.Write("\nEscolha uma opção: ");

            var opcao = Console.ReadLine() ?? "0";

            switch (opcao)
            {
                case "1":
                    ListarCultivos();
                    break;
                case "2":
                    AdicionarCultivo();
                    break;
                case "3":
                    AtualizarCultivo();
                    break;
                case "4":
                    RemoverCultivo();
                    break;
                case "5":
                    VerificaCompatibilidade();
                    break;
                case "0":
                    voltarAoMenu = true;
                    break;
                default:
                    Console.WriteLine("\nOpção inválida.");
                    break;
            }

            if (!voltarAoMenu && opcao != "0")
            {
                Console.WriteLine("\nPressione ENTER para continuar...");
                Console.ReadLine();
            }
        }
    }

    private void ListarCultivos()
    {
        Console.Clear();

        if (!_cultivosPorCanteiro.Any(c => c.Value.Any()))
        {
            Console.WriteLine("Nenhum cultivo cadastrado.");
            return;
        }

        Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  CULTIVOS CADASTRADOS                                          ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");

        foreach (var canteiroKvp in _cultivosPorCanteiro)
        {
            var canteiro = _canteiroRepository.GetById(canteiroKvp.Key);
            if (canteiro == null || !canteiroKvp.Value.Any()) continue;

            Console.WriteLine($"\nCanteiro: {canteiro.Nome}");
            Console.WriteLine("─────────────────────────────────────────────────────────────");

            foreach (var cultivo in canteiroKvp.Value)
            {
                Console.WriteLine($"  ID: {cultivo.Id}");
                Console.WriteLine($"  Espécie: {cultivo.Especie}");
                Console.WriteLine($"  Data de Plantio: {cultivo.DataPlantio:dd/MM/yyyy}");
                if (cultivo.PrevisaoColheita.HasValue)
                    Console.WriteLine($"  Previsão de Colheita: {cultivo.PrevisaoColheita:dd/MM/yyyy}");
                Console.WriteLine($"  Status: {cultivo.Status}");
                Console.WriteLine();
            }
        }
    }

    private void AdicionarCultivo()
    {
        Console.Clear();
        var canteiros = _canteiroRepository.GetAll().ToList();

        if (!canteiros.Any())
        {
            Console.WriteLine("Nenhum canteiro disponível. Crie um canteiro primeiro.");
            return;
        }

        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║  ADICIONAR CULTIVO                     ║");
        Console.WriteLine("╚════════════════════════════════════════╝");

        ListarCanteirosSimples(canteiros);

        Console.Write("\nEscolha o ID do canteiro: ");
        if (!Guid.TryParse(Console.ReadLine(), out var canteiroId))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        var canteiro = _canteiroRepository.GetById(canteiroId);
        if (canteiro == null)
        {
            Console.WriteLine("Canteiro não encontrado.");
            return;
        }

        Console.Write("Espécie a cultivar: ");
        var especie = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(especie))
        {
            Console.WriteLine("Espécie não pode estar vazia.");
            return;
        }

        // Verificar compatibilidade com cultivos existentes
        if (_cultivosPorCanteiro.TryGetValue(canteiroId, out var cultivosExistentes))
        {
            var cultuvoExistente = cultivosExistentes.FirstOrDefault(c => c.Status == StatusCultivo.Ativo);
            if (cultuvoExistente != null)
            {
                if (!_cultivoCompatibilityService.AreCompatible(especie, cultuvoExistente.Especie))
                {
                    Console.WriteLine($"\n⚠ AVISO: {especie} é incompatível com {cultuvoExistente.Especie} neste canteiro!");
                    Console.Write("Deseja continuar mesmo assim? (s/n): ");
                    if (Console.ReadLine()?.ToLower() != "s")
                    {
                        Console.WriteLine("Operação cancelada.");
                        return;
                    }
                }
            }
        }

        Console.Write("Data de Plantio (dd/MM/yyyy): ");
        if (!DateTime.TryParse(Console.ReadLine(), out var dataPlantio))
        {
            Console.WriteLine("Data inválida.");
            return;
        }

        Console.Write("Previsão de Colheita (dd/MM/yyyy) [deixe em branco para não definir]: ");
        DateTime? previsaoColheita = null;
        var dataColheitaInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(dataColheitaInput))
        {
            if (DateTime.TryParse(dataColheitaInput, out var data))
                previsaoColheita = data;
        }

        var cultivo = new Cultivo
        {
            CanteiroId = canteiroId,
            Especie = especie,
            DataPlantio = dataPlantio,
            PrevisaoColheita = previsaoColheita,
            Status = StatusCultivo.Ativo
        };

        if (!_cultivosPorCanteiro.ContainsKey(canteiroId))
            _cultivosPorCanteiro[canteiroId] = new List<Cultivo>();

        _cultivosPorCanteiro[canteiroId].Add(cultivo);

        Console.WriteLine($"\n✓ Cultivo '{especie}' adicionado ao canteiro '{canteiro.Nome}' com sucesso!");
    }

    private void AtualizarCultivo()
    {
        Console.Clear();
        var todosCultivos = _cultivosPorCanteiro.SelectMany(c => c.Value).ToList();

        if (!todosCultivos.Any())
        {
            Console.WriteLine("Nenhum cultivo para atualizar.");
            return;
        }

        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║  ATUALIZAR CULTIVO                     ║");
        Console.WriteLine("╚════════════════════════════════════════╝");

        var contador = 1;
        var cultivosList = new List<Cultivo>();

        foreach (var kvp in _cultivosPorCanteiro)
        {
            var canteiro = _canteiroRepository.GetById(kvp.Key);
            foreach (var cultivo in kvp.Value)
            {
                cultivosList.Add(cultivo);
                Console.WriteLine($"{contador}. {cultivo.Especie} - Canteiro: {canteiro?.Nome} (ID: {cultivo.Id})");
                contador++;
            }
        }

        Console.Write("\nEscolha o ID do cultivo: ");
        if (!Guid.TryParse(Console.ReadLine(), out var cultivoId))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        var cultivo_update = todosCultivos.FirstOrDefault(c => c.Id == cultivoId);
        if (cultivo_update == null)
        {
            Console.WriteLine("Cultivo não encontrado.");
            return;
        }

        Console.WriteLine("\nAtualizar Status:");
        Console.WriteLine("1 - Ativo");
        Console.WriteLine("2 - Concluído");
        Console.WriteLine("3 - Cancelado");
        Console.Write("Escolha: ");
        
        var statusInput = Console.ReadLine() ?? "1";
        cultivo_update.Status = statusInput switch
        {
            "2" => StatusCultivo.Concluido,
            "3" => StatusCultivo.Cancelado,
            _ => StatusCultivo.Ativo
        };

        Console.WriteLine("\n✓ Cultivo atualizado com sucesso!");
    }

    private void RemoverCultivo()
    {
        Console.Clear();
        var todosCultivos = _cultivosPorCanteiro.SelectMany(c => c.Value).ToList();

        if (!todosCultivos.Any())
        {
            Console.WriteLine("Nenhum cultivo para remover.");
            return;
        }

        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║  REMOVER CULTIVO                       ║");
        Console.WriteLine("╚════════════════════════════════════════╝");

        var contador = 1;
        foreach (var kvp in _cultivosPorCanteiro)
        {
            var canteiro = _canteiroRepository.GetById(kvp.Key);
            foreach (var cultivo in kvp.Value)
            {
                Console.WriteLine($"{contador}. {cultivo.Especie} - Canteiro: {canteiro?.Nome} (ID: {cultivo.Id})");
                contador++;
            }
        }

        Console.Write("\nEscolha o ID do cultivo a remover: ");
        if (!Guid.TryParse(Console.ReadLine(), out var cultivoId))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        var cultivo_remove = todosCultivos.FirstOrDefault(c => c.Id == cultivoId);
        if (cultivo_remove == null)
        {
            Console.WriteLine("Cultivo não encontrado.");
            return;
        }

        Console.Write($"\nTem certeza que deseja remover o cultivo de '{cultivo_remove.Especie}'? (s/n): ");
        if (Console.ReadLine()?.ToLower() == "s")
        {
            var canteiroId = _cultivosPorCanteiro.First(c => c.Value.Contains(cultivo_remove)).Key;
            _cultivosPorCanteiro[canteiroId].Remove(cultivo_remove);
            Console.WriteLine("\n✓ Cultivo removido com sucesso!");
        }
        else
        {
            Console.WriteLine("\nOperação cancelada.");
        }
    }

    private void VerificaCompatibilidade()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║  VERIFICAR COMPATIBILIDADE             ║");
        Console.WriteLine("╚════════════════════════════════════════╝");

        Console.Write("\nPrimeira espécie: ");
        var especie1 = Console.ReadLine() ?? "";

        Console.Write("Segunda espécie: ");
        var especie2 = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(especie1) || string.IsNullOrWhiteSpace(especie2))
        {
            Console.WriteLine("Espécies não podem estar vazias.");
            return;
        }

        var saoCompativeis = _cultivoCompatibilityService.AreCompatible(especie1, especie2);
        
        Console.WriteLine();
        if (saoCompativeis)
        {
            Console.WriteLine($"✓ {especie1} e {especie2} são COMPATÍVEIS.");
        }
        else
        {
            Console.WriteLine($"✗ {especie1} e {especie2} são INCOMPATÍVEIS.");
        }
    }

    private void ListarCanteirosSimples(List<Canteiro> canteiros)
    {
        foreach (var canteiro in canteiros)
        {
            Console.WriteLine($"  {canteiro.Id} - {canteiro.Nome} ({canteiro.Area} m²)");
        }
    }
}
