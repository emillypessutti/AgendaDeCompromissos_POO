using System;
using System.Text.Json;
using AgendaDeCompromisso.Modelos;

namespace AgendaDeCompromisso.Persistencia;

public class RepositorioCompromissos
{
    const string caminhoArquivo = "compromissos.json";
    static void SalvarCompromissos(List<Compromisso> lista)
    {
        var json = JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(caminhoArquivo, json);
    }

}
