using System;

namespace GestaoHortas.Library.Domain.Entities
{
    public enum TipoSolo { Argiloso, Arenoso, Humoso, Misto }
    public enum StatusCanteiro { Ativo, EmManutencao, Inativo }

    public class Canteiro
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public ValueObjects.Localizacao Localizacao { get; set; } = new();
        public decimal Area { get; set; }
        public TipoSolo TipoSolo { get; set; }
        public StatusCanteiro Status { get; set; } = StatusCanteiro.Ativo;
    }
}
