using System;
using System.Collections.Generic;
using System.Linq;
using GestaoHortas.Library.Application.Interfaces;
using GestaoHortas.Library.Domain.Entities;

namespace GestaoHortas.Library.Infra.Repositories
{
    public class InMemoryCultivoRepository : ICultivoRepository
    {
        private readonly List<Cultivo> _store = new();

        public void Add(Cultivo cultivo) => _store.Add(cultivo);

        public IEnumerable<Cultivo> GetAll() => _store.ToList();

        public IEnumerable<Cultivo> GetByCanteiro(Guid canteiroId) => _store.Where(x => x.CanteiroId == canteiroId).ToList();

        public Cultivo? GetById(Guid id) => _store.FirstOrDefault(x => x.Id == id);

        public void Remove(Guid id)
        {
            var item = GetById(id);
            if (item != null)
                _store.Remove(item);
        }

        public void Update(Cultivo cultivo)
        {
            var existing = GetById(cultivo.Id);
            if (existing == null)
                return;

            existing.CanteiroId = cultivo.CanteiroId;
            existing.Especie = cultivo.Especie;
            existing.DataPlantio = cultivo.DataPlantio;
            existing.PrevisaoColheita = cultivo.PrevisaoColheita;
            existing.Status = cultivo.Status;
        }
    }
}
