using System;

namespace AgendaDeCompromisso.Modelos;

public class Local
{
    public string? NomeLocal { get; set; }
    public int CapacidadeMax { get; set; }

    public bool ValidarCapacidade(int quantidade)
    {
        if (quantidade <= 0)
        {
            throw new ArgumentException("A capacidade deve ser maior que zero.");
        }

        if (string.IsNullOrWhiteSpace(CapacidadeMax.ToString()))
        {
            throw new ArgumentException("A capacidade máxima não pode ser vazia.");
        }
        return quantidade <= CapacidadeMax;
    }

}