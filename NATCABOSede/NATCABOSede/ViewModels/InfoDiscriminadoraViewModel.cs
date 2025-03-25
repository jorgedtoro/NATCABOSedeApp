using System;

namespace NATCABOSede.ViewModels
{
    public class InfoDiscriminadoraViewModel
    {
        public short LineaSeleccionada { get; set; }
        public string NombreLinea { get; set; }
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
        public int PersonalPeriferico { get; set; }
        public int PersonalCorrecto { get; set; }
        public int Rangos_Ok { get; set; }
        public int DiscriminadorEnUso { get; set; }
        public int ExpulsionAire_Ok { get; set; }

        // Constructor por defecto
        public InfoDiscriminadoraViewModel()
        {
            NombreLinea = "No disponible";
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
            PersonalCorrecto = 0;
            Rangos_Ok = 0;
            DiscriminadorEnUso= 0;
            ExpulsionAire_Ok = 0;
        }
    }
}
