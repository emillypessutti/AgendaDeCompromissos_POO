using System;

namespace AgendaDeCompromisso.Modelos;

public class Participante
{
    public string? Nome { get; set; }

    private List<Compromisso> _compromissos = new();

    public void AdicionarCompromisso(Compromisso compromisso)
    {
        if (_compromissos == null)
        {
            _compromissos = new List<Compromisso>();
        }
        _compromissos.Add(compromisso);
    }
}
