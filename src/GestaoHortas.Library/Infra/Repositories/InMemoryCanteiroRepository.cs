using System;
using System.Collections.Generic;
using System.Linq;
using GestaoHortas.Library.Application.Interfaces;
using GestaoHortas.Library.Domain.Entities;

namespace GestaoHortas.Library.Infra.Repositories
{
    public class InMemoryCanteiroRepository : ICanteiroRepository
    {
        private readonly List<Canteiro> _store = new();

        public void Add(Canteiro canteiro) => _store.Add(canteiro);

        public IEnumerable<Canteiro> GetAll() => _store.ToList();

        public Canteiro? GetById(Guid id) => _store.FirstOrDefault(x => x.Id == id);

        public void Remove(Guid id)
        {
            var item = GetById(id);
            if (item != null) _store.Remove(item);
        }

        public void Update(Canteiro canteiro)
        {
            var existing = GetById(canteiro.Id);
            if (existing == null) return;
            existing.Nome = canteiro.Nome;
            existing.Localizacao = canteiro.Localizacao;
            existing.Area = canteiro.Area;
            existing.TipoSolo = canteiro.TipoSolo;
            existing.Status = canteiro.Status;
        }
    }
}
