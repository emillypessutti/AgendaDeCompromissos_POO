using System.Text.Json;
using System.Text.Json.Serialization;
using AgendaDeCompromisso.Modelos;
using AgendaDeCompromisso.Persistencia;
class Program
{
    const string caminhoArquivo = "compromissos.json";
    static void Main(string[] args)
    {

        List<Compromisso> compromissos = RepositorioCompromissos.CarregarCompromisso();

        if (args.Length == 0)
        {
            Console.WriteLine(" \n===Agenda de Compromissos===\n");
            Console.WriteLine(" Listar");
            Console.WriteLine(" Adicionar <usuario> <data> <hora> <descricao> <local> <capacidade> <participantes> <anotacoes>");
            Console.WriteLine(" Editar <indice> <usuario> <data> <hora> <descricao> <local> <capacidade> <participantes> <anotacoes>");
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

        RepositorioCompromissos.SalvarCompromisso(compromissos);
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
            Console.WriteLine($"{i + 1}. {lista[i]}");
        }
    }

    static void AdicionarCompromissos(string[] args, List<Compromisso> lista)
    {
        if (args.Length < 7)
        {
            Console.WriteLine("Uso: adicionar <usuario> <data> <hora> <descricao> <local> <capacidade> <participantes> <anotacoes>");
            return;
        }

        for (int i = 0; i <= 5; i++)
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
        .Select(a => new Anotacao { Texto = a.Trim(), DataCriacao = DateTime.Now })
        .ToList()
    : new List<Anotacao>()
        };

        lista.Add(compromisso);
        Console.WriteLine("Compromisso adicionado via CLI.");
    }

    static void EditarCompromissos(string[] args, List<Compromisso> lista)
    {
        if (args.Length < 9)
        {
            Console.WriteLine("Uso: editar <indice> <novo_usuario> <nova_data> <nova_hora> <novo_local> <nova_descricao> <nova_capacidade> <novo_participante> <nova_anotacao>");
            return;
        }

        string[] argumentos = new string[9];
        for (int i = 0; i < 9; i++)
        {
            argumentos[i] = i < args.Length ? args[i] : "";
        }

        if (!int.TryParse(argumentos[0], out int indiceVisual) || indiceVisual < 1 || indiceVisual > lista.Count)
        {
            Console.WriteLine("Número inválido.");
            return;
        }
        int indice = indiceVisual - 1;

        var compromisso = lista[indice];
        var usuarioTemp = !string.IsNullOrWhiteSpace(argumentos[1]) ? new Usuario { Nome = argumentos[1] } : compromisso.Usuario;
        var dataTemp = !string.IsNullOrWhiteSpace(argumentos[2]) ? DateTime.Parse(argumentos[2]) : compromisso.Data;
        var horaTemp = !string.IsNullOrWhiteSpace(argumentos[3]) ? TimeSpan.Parse(argumentos[3]) : compromisso.Hora;
        var descricaoTemp = !string.IsNullOrWhiteSpace(argumentos[4]) ? argumentos[4] : compromisso.Descricao;
        var localTemp = compromisso.Local != null
            ? new Local { NomeLocal = compromisso.Local.NomeLocal, CapacidadeMax = compromisso.Local.CapacidadeMax }
            : new Local();

        if (!string.IsNullOrWhiteSpace(argumentos[5]))
            localTemp.NomeLocal = argumentos[5];
        if (!string.IsNullOrWhiteSpace(argumentos[6]))
            localTemp.CapacidadeMax = int.Parse(argumentos[6]);

        var participantesTemp = compromisso.participantes;
        if (!string.IsNullOrWhiteSpace(argumentos[7]))
            participantesTemp = args[7]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => new Participante { Nome = p.Trim() })
                .ToList();

        var anotacoesTemp = compromisso.anotacoes;
        if (!string.IsNullOrWhiteSpace(args[8]))
            anotacoesTemp = args[8]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(a => new Anotacao { Texto = a.Trim(), DataCriacao = DateTime.Now })
                .ToList();

        if (localTemp != null && participantesTemp != null)
        {
            if (!localTemp.ValidarCapacidade(participantesTemp.Count))
            {
                Console.WriteLine("Quantidade de participantes excede a capacidade máxima do local.");
                return;
            }
        }

        compromisso.Usuario = usuarioTemp;
        compromisso.Data = dataTemp;
        compromisso.Hora = horaTemp;
        compromisso.Descricao = descricaoTemp;
        compromisso.Local = localTemp;
        compromisso.participantes = participantesTemp ?? new List<Participante>();
        compromisso.anotacoes = anotacoesTemp;

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

