using System;
using System.Collections.Generic;
using GestaoHortas.Library.Domain.Entities;

namespace GestaoHortas.Library.Application.Interfaces
{
    public interface ICanteiroRepository
    {
        Canteiro? GetById(Guid id);
        IEnumerable<Canteiro> GetAll();
        void Add(Canteiro canteiro);
        void Update(Canteiro canteiro);
        void Remove(Guid id);
    }
}
