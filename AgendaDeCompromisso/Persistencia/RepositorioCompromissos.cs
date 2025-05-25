using System;
using System.Text.Json;
using AgendaDeCompromisso.Modelos;

namespace AgendaDeCompromisso.Persistencia;

public class RepositorioCompromissos
{
    private const string caminhoArquivo = "compromissos.json";

    public static void SalvarCompromisso(List<Compromisso> lista)
    {
        var json = JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(caminhoArquivo, json);
    }

    public static List<Compromisso> CarregarCompromisso()
    {
        if (!File.Exists(caminhoArquivo)) return new List<Compromisso>();
        var json = File.ReadAllText(caminhoArquivo);
        return JsonSerializer.Deserialize<List<Compromisso>>(json) ?? new List<Compromisso>();
    }
}


