using System;

namespace AgendaDeCompromisso.Modelos;

public class Participante
{
    private string? _nome { get; set; }

    private List<Compromisso> _compromissos = new();

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
        if (_compromissos == null)
        {
            _compromissos = new List<Compromisso>();
        }
        _compromissos.Add(compromisso);
    }
}
