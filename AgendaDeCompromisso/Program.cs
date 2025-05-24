using System.Text.Json;
using AgendaDeCompromisso.Modelos;
class Program
{
    const string caminhoArquivo = "compromissos.json";
    static void Main(string[] args)
    {

        List<Compromisso> compromissos = CarregarCompromissos();

        if (args.Length == 0)
        {
            Console.WriteLine(" \n===Agenda de Compromissos===\n");
            Console.WriteLine(" Listar");
            Console.WriteLine(" Adicionar <usuario> <data> <hora> <descricao> <local> <capacidade> <participantes> <anotacoes>");
            Console.WriteLine(" Editar <usuario> <data> <hora> <descricao> <local> <capacidade> <participantes> <anotacoes>");
            Console.WriteLine(" Excluir <indice>");
            Console.WriteLine(" Sair");
            return;
        }

        string opcao = args[0].ToLower();

        switch (opcao)
        {
            case "listar":
                ListarCompromissos(compromissos);
                break;
            case "adicionar":
                AdicionarCompromissos(args.Skip(1).ToArray(), compromissos);
                break;
            case "editar":
                EditarCompromissos(args.Skip(1).ToArray(), compromissos);
                break;
            case "excluir":
                ExcluirCompromissos(args.Skip(1).ToArray(), compromissos);
                break;
            case "sair":
                Console.WriteLine("Saindo...");
                break;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }

        SalvarCompromissos(compromissos);
    }
    static void SalvarCompromissos(List<Compromisso> lista)
    {
        var json = JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(caminhoArquivo, json);
    }

    static List<Compromisso> CarregarCompromissos()
    {
        if (!File.Exists(caminhoArquivo)) return new List<Compromisso>();
        var json = File.ReadAllText(caminhoArquivo);
        return JsonSerializer.Deserialize<List<Compromisso>>(json) ?? new List<Compromisso>();
    }

    static void ListarCompromissos(IList<Compromisso> lista)
    {
        if (lista.Count == 0)
        {
            Console.WriteLine("Nenhum compromisso encontrado.");
            return;
        }

        for (int i = 0; i < lista.Count; i++)
        {
            var c = lista[i];
            var nomesParticipantes = c.participantes != null && c.participantes.Count > 0
                ? string.Join(", ", c.participantes.Select(p => p.Nome))
                : "Nenhum";
            var textosAnotacoes = c.anotacoes != null && c.anotacoes.Count > 0
                ? string.Join(" | ", c.anotacoes.Select(a => a.Texto))
                : "Nenhuma";

            Console.WriteLine($"{i + 1}. Criado por: {c.Usuario?.Nome} - Dia {c.Data:dd/MM/yyyy} às {c.Hora} - Local: {c.Local?.NomeLocal} - Descrição: {c.Descricao} - Capacidade: {c.Local?.CapacidadeMax} - Participantes: {nomesParticipantes} - Anotações: {textosAnotacoes}");
        }
    }

    static void AdicionarCompromissos(string[] args, List<Compromisso> lista)
    {
        if (args.Length < 7)
        {
            Console.WriteLine("Uso: adicionar <usuario> <data> <hora> <descricao> <local> <capacidade> <participantes> <anotacoes>");
            return;
        }

        for (int i = 0; i < args.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(args[i]))
            {
                Console.WriteLine($"O argumento {i + 1} não pode ser vazio.");
                return;
            }
        }

        var local = new Local { NomeLocal = args[4], CapacidadeMax = int.Parse(args[5]) };

        var listaParticipantes = args[6]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => new Participante { Nome = p.Trim() })
            .ToList();

        try
        {
            if (!local.ValidarCapacidade(listaParticipantes.Count))
            {
                Console.WriteLine("Quantidade de participantes excede a capacidade máxima do local.");
                return;
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
            return;
        }

        var compromisso = new Compromisso
        {
            Usuario = new Usuario { Nome = args[0] },
            Data = DateTime.Parse(args[1]),
            Hora = TimeSpan.Parse(args[2]),
            Descricao = args[3],
            Local = local,
            participantes = listaParticipantes,
            anotacoes = args.Length > 7
    ? args[7]
        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(a => new Anotacao { Texto = a.Trim() })
        .ToList()
    : new List<Anotacao>()
        };

        lista.Add(compromisso);
        Console.WriteLine("Compromisso adicionado via CLI.");
    }

    static void EditarCompromissos(string[] args, List<Compromisso> lista)
    {
        Console.WriteLine("Argumentos recebidos: " + args.Length);
        for (int i = 0; i < args.Length; i++)
            Console.WriteLine($"args[{i}]: '{args[i]}'");

        if (args.Length < 9)
        {
            Console.WriteLine("Uso: editar <numero_exibido> <novo_usuario> <nova_data> <nova_hora> <novo_local> <nova_descricao> <nova_capacidade> <novo_participante> <nova_anotacao>");
            return;
        }

        if (!int.TryParse(args[0], out int indiceVisual) || indiceVisual < 1 || indiceVisual > lista.Count)
        {
            Console.WriteLine("Número inválido.");
            return;
        }
        int indice = indiceVisual - 1;

        if (!string.IsNullOrWhiteSpace(args[1]))
            lista[indice].Usuario = new Usuario { Nome = args[1] };
        if (!string.IsNullOrWhiteSpace(args[2]))
            lista[indice].Data = DateTime.Parse(args[2]);
        if (!string.IsNullOrWhiteSpace(args[3]))
            lista[indice].Hora = TimeSpan.Parse(args[3]);
        if (!string.IsNullOrWhiteSpace(args[4]))
            lista[indice].Descricao = args[4];
        if (!string.IsNullOrWhiteSpace(args[5]))
            lista[indice].Local = new Local { NomeLocal = args[5], CapacidadeMax = int.Parse(args[6]) };
        if (!string.IsNullOrWhiteSpace(args[7]))
            lista[indice].participantes = args[7]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => new Participante { Nome = p.Trim() })
                .ToList();
        if (!string.IsNullOrWhiteSpace(args[8]))
            lista[indice].anotacoes = args[8]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(a => new Anotacao { Texto = a.Trim() })
                .ToList();

        Console.WriteLine("Compromisso editado.");
    }

    static void ExcluirCompromissos(string[] args, List<Compromisso> lista)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Uso: excluir <indice>");
            return;
        }

        if (!int.TryParse(args[0], out int indiceVisual) || indiceVisual < 1 || indiceVisual > lista.Count)
        {
            Console.WriteLine("Índice inválido.");
            return;
        }

        int indice = indiceVisual - 1;
        lista.RemoveAt(indice);
        Console.WriteLine("Compromisso removido.");
    }
}

