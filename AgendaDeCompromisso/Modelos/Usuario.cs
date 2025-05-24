using System;

namespace AgendaDeCompromisso.Modelos;

public class Usuario
{
    private string? _nome { get; set; }

    private List<Compromisso> compromissos = new();

    public IReadOnlyCollection<Compromisso> Compromissos => compromissos.AsReadOnly();

    public string? Nome
    {
        get => _nome;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("O nome não pode ser vazio.");
            }
            _nome = value;
        }
    }
    public void AdicionarCompromisso(Compromisso compromisso)
    {
        if (compromissos == null)
        {
            compromissos = new List<Compromisso>();
        }
        compromissos.Add(compromisso);
    }
}