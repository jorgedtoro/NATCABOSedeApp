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

    }
}
