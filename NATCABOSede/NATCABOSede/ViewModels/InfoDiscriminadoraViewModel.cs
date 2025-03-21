using System;

namespace NATCABOSede.ViewModels
{
    public class InfoDiscriminadoraViewModel
    {
        public short LineaSeleccionada { get; set; }
        public string NombreCliente { get; set; }
        public string NombreProducto { get; set; }
        public double PM { get; set; }
        public double PPM { get; set; }
        public double PPM_Disc { get; set; }
        public double PMObjetivo { get; set; }
        public double PM_Disc { get; set; }
        public double ExtraPeso { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFinAproximada { get; set; }
        public double PorcentajePedido { get; set; }
        public double CosteMOD { get; set; }

        public int PersonalEnBalanza { get; set; }
        public int PersonalTotal { get; set; }

        // Constructor por defecto
        public InfoDiscriminadoraViewModel()
        {
            NombreCliente = "No disponible";
            NombreProducto = "No disponible";
            PPM = 0;
            PPM_Disc = 0;
            PM = 0;
            PM_Disc = 0;
            ExtraPeso = 0;
            HoraInicio = DateTime.Now;
            HoraFinAproximada = DateTime.Now;
            PorcentajePedido = 0;
            CosteMOD = 0;
            PersonalEnBalanza = 0;
            PersonalTotal = 0;
        }
    }
}
