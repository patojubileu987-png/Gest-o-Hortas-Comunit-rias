using System;

namespace GestaoHortas.Library.Domain.Entities
{
    public enum StatusCultivo { Ativo, Concluido, Cancelado }

    public class Cultivo
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid CanteiroId { get; set; }
        public string Especie { get; set; } = string.Empty;
        public DateTime DataPlantio { get; set; }
        public DateTime? PrevisaoColheita { get; set; }
        public StatusCultivo Status { get; set; } = StatusCultivo.Ativo;
    }
}
