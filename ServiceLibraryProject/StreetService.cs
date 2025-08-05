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

        public bool Add(Street entity)
        {
            // Validación: Nombre requerido y longitud
            if (string.IsNullOrWhiteSpace(entity.Name) || entity.Name.Length < 2 || entity.Name.Length > 255)
                throw new ArgumentException("El nombre de la calle es obligatorio y debe tener entre 2 y 255 caracteres.");
            // Validación: Unicidad de nombre
            if (_streetRepository.GetAll().Any(s => s.Name == entity.Name))
                throw new InvalidOperationException("Ya existe una calle con ese nombre.");
            // Validación: Código único
            var uniqueCode = GetUniqueStreetCode(entity.Name, entity.AddressStreetType) ??
                throw new InvalidOperationException("No se puede generar un código único para la calle.");
            if (_streetRepository.GetAll().Any(s => s.Code == uniqueCode))
                throw new InvalidOperationException("Ya existe una calle con ese código.");
            entity.Code = uniqueCode;
            try
            {
                _streetRepository.Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al agregar la calle: " + ex.Message);
            }
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

        void IService<Street>.Add(Street entity)
        {
            throw new NotImplementedException();
        }
    }
}
