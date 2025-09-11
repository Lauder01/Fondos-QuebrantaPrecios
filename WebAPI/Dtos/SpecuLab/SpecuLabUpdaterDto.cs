using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.SpecuLab
{
    /// <summary>
    /// DTO para actualizar datos de edificio desde SpecuLab. Soporta tanto el formato original como JSON Patch.
    /// </summary>
    public class SpecuLabUpdaterDto
    {
        // Propiedades para formato original
        public string? StatusName { get; set; }

        // Propiedades para formato JSON Patch
        public string? Path { get; set; }
        public string? Op { get; set; }
        public string? Value { get; set; }

        // Validación personalizada: debe tener StatusName O las propiedades de JSON Patch
        public bool IsValid()
        {
            // Formato original: solo StatusName
            if (!string.IsNullOrWhiteSpace(StatusName) && 
                string.IsNullOrWhiteSpace(Path) && 
                string.IsNullOrWhiteSpace(Op) && 
                string.IsNullOrWhiteSpace(Value))
            {
                return true;
            }

            // Formato JSON Patch: Path, Op y Value
            if (string.IsNullOrWhiteSpace(StatusName) && 
                !string.IsNullOrWhiteSpace(Path) && 
                !string.IsNullOrWhiteSpace(Op) && 
                !string.IsNullOrWhiteSpace(Value))
            {
                return true;
            }

            return false;
        }
    }
}
