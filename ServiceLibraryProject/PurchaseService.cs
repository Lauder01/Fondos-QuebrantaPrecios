using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibraryProject.Entities;
using ServiceLibraryProject.Interfaces;

namespace ServiceLibraryProject
{
    /// <summary>
    /// Servicio para operaciones de negocio relacionadas con compras (Purchase).
    /// Proporciona métodos para obtener, agregar, actualizar y eliminar compras en la base de datos.
    /// </summary>
    public class PurchaseService : IService<Purchase>
    {
        /// <summary>
        /// Agrega una nueva compra.
        /// </summary>
        /// <param name="entity">Entidad Purchase a agregar.</param>
        public void Add(Purchase entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Elimina una compra por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la compra a eliminar.</param>
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene todas las compras.
        /// </summary>
        /// <returns>Una colección de compras.</returns>
        public IEnumerable<Purchase> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene una compra por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la compra.</param>
        /// <returns>La compra encontrada o null si no existe.</returns>
        public Purchase? GetById(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Actualiza una compra existente.
        /// </summary>
        /// <param name="entity">Entidad Purchase a actualizar.</param>
        public void Update(Purchase entity)
        {
            throw new NotImplementedException();
        }
    }
}
