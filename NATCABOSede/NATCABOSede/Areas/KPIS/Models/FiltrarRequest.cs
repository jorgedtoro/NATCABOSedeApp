namespace NATCABOSede.Areas.KPIS.Models
{
    public class FiltrarRequest
    {
        public int? IdLinea { get; set; }
        public string Confeccion { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public int Page { get; set; } = 1; // Opcional: para manejo de paginación
        public int PageSize { get; set; } = 25; // Opcional: tamaño de página por defecto

        
    }
}
