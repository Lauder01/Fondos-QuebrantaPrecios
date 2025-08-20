using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryProject.Entities;
/// <summary>
/// Representa un código postal y su relación con distritos.
/// </summary>
public partial class Zipcode
{
    /// <summary>
    /// Identificador único del código postal.
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Código postal.
    /// </summary>
    public string Code { get; set; }
    /// <summary>
    /// Colección de distritos asociados a este código postal.
    /// </summary>
    public virtual ICollection<District> District { get; set; } = new List<District>();
}

