using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibraryProject.Entities;
using RepositoryLibraryProject.Interfaces;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    /// <summary>
    /// Servicio para operaciones de negocio relacionadas con direcciones (Address).
    /// Proporciona métodos para obtener, agregar, actualizar y eliminar direcciones en la base de datos.
    /// </summary>
    public class AdressService : IService<Address>
    {
        /// <summary>
        /// Repositorio de direcciones utilizado para acceder a la base de datos.
        /// </summary>
        private readonly IRepository<Address> _addressRepository;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de direcciones.
        /// </summary>
        /// <param name="addressRepository">Repositorio de direcciones.</param>
        public AdressService(IRepository<Address> addressRepository)
        {
            _addressRepository = addressRepository;
        }

        /// <summary>
        /// Obtiene todas las direcciones.
        /// </summary>
        /// <returns>Una colección de direcciones.</returns>
        public IEnumerable<Address> GetAll()
        {
            return _addressRepository.GetAll();
        }

        /// <summary>
        /// Obtiene una dirección por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la dirección.</param>
        /// <returns>La dirección encontrada o null si no existe.</returns>
        public Address? GetById(string id)
        {
            return _addressRepository.GetById(id);
        }

        /// <summary>
        /// Agrega una nueva dirección.
        /// </summary>
        /// <param name="entity">Entidad Address a agregar.</param>
        public void Add(Address entity)
        {
            _ = 
            _addressRepository.Add(entity);
        }

        /// <summary>
        /// Actualiza una dirección existente.
        /// </summary>
        /// <param name="entity">Entidad Address a actualizar.</param>
        public void Update(Address entity)
        {
            _addressRepository.Update(entity);
        }

        /// <summary>
        /// Elimina una dirección por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la dirección a eliminar.</param>
        public void Delete(string id)
        {
            _ = _addressRepository.GetById(id) ?? throw new ArgumentException("La dirección no existe.", nameof(id));
            _addressRepository.Delete(id);
        }
    }
}
