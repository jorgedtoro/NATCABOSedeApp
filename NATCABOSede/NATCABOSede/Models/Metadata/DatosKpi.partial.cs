using System.ComponentModel.DataAnnotations;

namespace NATCABOSede.Models.Metadata
{
    [MetadataType(typeof(DatosKpiMetadata))]
    public partial class DatosKpi
    {
    }
    public class DatosKpiMetadata
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Línea")]
        public int IdLinea { get; set; }

        [Display(Name = "Cliente")]
        public int? IdCliente { get; set; }

        [Display(Name = "Producto")]
        public string? IdProducto { get; set; }

        [Display(Name = "Paquetes Trabajados")]
        public double? PaquetesTrabajados { get; set; }

      
    }
}
