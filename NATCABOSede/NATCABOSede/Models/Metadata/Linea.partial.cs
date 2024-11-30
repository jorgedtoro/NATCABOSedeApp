using System.ComponentModel.DataAnnotations;

namespace NATCABOSede.Models.Metadata
{
    [MetadataType(typeof(LineaMetadata))]
    public partial class Linea
    {
    }
    public class LineaMetadata
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Número de Línea")]
        public int NumeroLinea { get; set; }

        [Display(Name = "Nombre de Línea")]
        public string NombreLinea { get; set; } = null!;
    }
}
