using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Enums;
using ClassLibraryProject.Extensions;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
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
        // Implementación requerida por la interfaz
        public void Add(Street entity)
        {
            // Validación: Nombre compuesto requerido y longitud
            if (string.IsNullOrWhiteSpace(entity.Name) || entity.Name.Length < 2 || entity.Name.Length > 255)
                throw new ArgumentException("El nombre de la calle es obligatorio y debe tener entre 2 y 255 caracteres.");
            // Validación: Unicidad de nombre compuesto
            if (_streetRepository.GetAll().Any(s => s.Name == entity.Name))
                throw new InvalidOperationException("Ya existe una calle con ese nombre.");
            // Validación: Código único
            if (_streetRepository.GetAll().Any(s => s.Code == entity.Code))
                throw new InvalidOperationException("Ya existe una calle con ese código.");
            _streetRepository.Add(entity);
        }

        public void Delete(string id)
        {
            _ = _streetRepository.GetById(id) ?? throw new ArgumentException("La calle no existe.", nameof(id));
            _streetRepository.Delete(id);
        }

        public IEnumerable<Street> GetAll()
        {
            return _streetRepository.GetAll();
        }

        public Street? GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El identificador proporcionado no puede estar vacío.", nameof(id));
            return _streetRepository.GetById(id);
        }

        public Street? GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;
            return _streetRepository.Find(s => s.Code == code);
        }

        public void Update(Street entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La calle no puede ser nula.");
            _streetRepository.Update(entity);
        }

        public string? GetUniqueStreetCode(string baseName, StreetTypeEnum type)
        {
            var existingCodes = new HashSet<string>(_streetRepository.GetAll().Select(s => s.Code));
            var words = baseName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int lastWordLength = 2;
            string code;
            do
            {
                var initials = Street.GetInitials(baseName, lastWordLength);
                var acronym = type.GetAcronym();
                code = string.IsNullOrEmpty(acronym) ? initials : $"{acronym}-{initials}";
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
