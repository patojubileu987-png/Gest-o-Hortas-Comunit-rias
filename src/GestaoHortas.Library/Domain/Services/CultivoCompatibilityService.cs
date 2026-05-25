using System.Collections.Generic;

namespace GestaoHortas.Library.Domain.Services
{
    public class CultivoCompatibilityService
    {
        private readonly HashSet<(string, string)> _incompatibilities = new();

        public CultivoCompatibilityService()
        {
            // exemplo inicial: feijão incompatível com tomate
            _incompatibilities.Add(("Feijão","Tomate"));
            _incompatibilities.Add(("Tomate","Feijão"));
        }

        public bool AreCompatible(string especieA, string especieB)
        {
            if (string.IsNullOrWhiteSpace(especieA) || string.IsNullOrWhiteSpace(especieB)) return true;
            return !_incompatibilities.Contains((especieA, especieB));
        }
    }
}
