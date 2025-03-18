using NATCABOSede.Models;
using NATCABOSede.ViewModels;

namespace NATCABOSede.Interfaces
{
    public interface IKPIService
    {

        double CalcularPPM(double paquetesTotales, double minutosTrabajados, int numeroPersonas);
        double CalcularPM(double paquetesTotales, double minutosTrabajados);
        double CalcularExtrapeso(double pesoReal, double pesoObjetivo, int paquetes);
        DateTime CalcularHoraFin(DateTime horaInicio, int paquetesRestantes, double mediaPaquetesPorMinuto);
        double CalcularPorcentajePedido(int paquetesProducidos, int paquetesTotales);
        double CalcularCosteMOD(double tiempoTotal, double costoHora, int numeroPaquetes, double pesoMinimo);
        public double CalcularFTT(int paquetesTotales, int paquetesRechazados);
        public double CalcularPorcentajeDesperdicio(double pesoDesperdicio, double pesoReal);
        DatosKpiViewModel GenerarDatosKpiViewModel(DatosKpisLive datos, double mediaPaquetesPorMinuto);

    }
}
