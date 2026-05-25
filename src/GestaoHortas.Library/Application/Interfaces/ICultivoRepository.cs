using System;
using System.Collections.Generic;
using GestaoHortas.Library.Domain.Entities;

namespace GestaoHortas.Library.Application.Interfaces
{
    public interface ICultivoRepository
    {
        IEnumerable<Cultivo> GetAll();
        IEnumerable<Cultivo> GetByCanteiro(Guid canteiroId);
        Cultivo? GetById(Guid id);
        void Add(Cultivo cultivo);
        void Update(Cultivo cultivo);
        void Remove(Guid id);
    }
}
