using System;

namespace AgendaDeCompromisso.Modelos;

public class Compromisso
{
    private DateTime _data { get; set; }
    private TimeSpan _hora { get; set; }

    public string? Descricao { get; set; }

    public Usuario? Usuario { get; set; }

    public Local? Local { get; set; }

    public List<Participante> participantes { get; set; } = new();

    public List<Anotacao> anotacoes { get; set; } = new();
    public DateTime Data
    {
        get => _data;
        set
        {
            if (value < DateTime.Now)
            {
                throw new ArgumentException("A data não pode ser no passado.");
            }
            _data = value;
        }
    }

    public TimeSpan Hora
    {
        get => _hora;
        set
        {
            if (value < TimeSpan.Zero || value >= TimeSpan.FromDays(1))
                throw new ArgumentException("A hora deve estar entre 00:00 e 23:59.");
            _hora = value;
        }
    }

    public void AdicionarParticipante(Participante p)
    {
        if (participantes == null)
        {
            participantes = new List<Participante>();
        }
        participantes.Add(p);
    }

    public void AdicionarAnotacao(Anotacao anotacao)
    {
        if (anotacoes == null)
        {
            anotacoes = new List<Anotacao>();
        }
        anotacoes.Add(anotacao);
    }

    public override string ToString()
    {
        var nomesParticipantes = participantes != null && participantes.Count > 0
            ? string.Join(", ", participantes.Select(p => p.Nome))
            : "Nenhum";
        var textosAnotacoes = anotacoes != null && anotacoes.Count > 0
            ? string.Join(" | ", anotacoes.Select(a => a.Texto))
            : "Nenhuma";

        return $"Usuário: {Usuario?.Nome}, Compromisso: {Descricao}, Data: {Data:dd/MM/yyyy}, Hora: {Hora:hh\\:mm}, Local: {Local?.NomeLocal}, Capacidade: {Local?.CapacidadeMax}, Participantes: {nomesParticipantes}, Anotações: {textosAnotacoes}, Data de criação: {anotacoes?.FirstOrDefault()?.DataCriacao:dd/MM/yyyy}";
    }

}
