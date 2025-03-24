namespace NATCABOSede.Models
{
    public class KpisHistoricoDto
    {
        public double? MOD { get; set; }
        public double? PPM_Marco { get; set; }
        public double? PPM_Bizerba { get; set; }
        public double? PM_Marco { get; set; }
        public double? PM_Bizerba { get; set; }
        public double? Extrapeso_Marco { get; set; }
        public double? Extrapeso_Bizerba { get; set; }
        public double? FTT { get; set; }
        public double? Desecho_Kg { get; set; }
        public double? Desecho_Perc { get; set; }
        public DateTime? Fecha { get; set; }
        public string? nombreLinea { get; set; }  // Permite nulls
        public string? Confeccion { get; set; }    // Considera también si puede ser null
        public int? paquetesValidos { get; set; }
        public double? pesoTotalReal { get; set; }
        public double? TotalHours { get; set; }
        public int? IdLinea { get; set; }
    }
}
