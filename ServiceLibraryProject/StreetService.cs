using System;
using System.Collections.Generic;
using System.Linq;
using FQP.Entities;
using FQP.Service.Interfaces;
using FQP.Repository.Interfaces;
using FQP.Enums;

namespace FQP.Service
{
    /// <summary>
    /// Servicio para operaciones de negocio relacionadas con Street.
    /// </summary>
    public class StreetService : IService<Street>
    {
        private readonly IRepository<Street> _streetRepository;

        public StreetService(IRepository<Street> streetRepository)
        {
            _streetRepository = streetRepository;
        }

        public void Add(Street entity)
        {
            // Ejemplo: Validar unicidad antes de agregar
            var uniqueCode = GetUniqueStreetCode(entity.Name, entity.AddressStreetType);
            if (uniqueCode == null)
                throw new InvalidOperationException("No se puede generar un código único para la calle.");
            entity.Code = uniqueCode;
            // Aquí iría la lógica para guardar la entidad
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Street> GetAll()
        {
            throw new NotImplementedException();
        }

        public Street? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Street entity)
        {
            throw new NotImplementedException();
        }

        public string? GetUniqueStreetCode(string name, StreetTypeEnum type)
        {
            var existingCodes = new HashSet<string>(_streetRepository.GetAll().Select(s => s.Code));
            var words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int lastWordLength = 2;
            string code;
            do
            {
                var initials = Street.GetInitials(name, lastWordLength);
                var acronym = new Street { AddressStreetType = type }.GetStreetTypeAcronym();
                code = string.IsNullOrEmpty(acronym) ? initials : $"{acronym.ToUpper()}-{initials}";
                if (!existingCodes.Contains(code))
                    return code;
                if (words[^1].Length > lastWordLength)
                    lastWordLength++;
                else
                    return null;
            } while (true);
        }
    }
}
