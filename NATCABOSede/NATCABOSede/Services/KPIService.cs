using System;
using NATCABOSede.Interfaces;

namespace NATCABOSede.Services
{
    public class KPIService : IKPIService
    {

        public double CalcularPPM(double paquetesTotales, double minutosTrabajados, int numeroPersonas)
        {
            if (minutosTrabajados <= 0 || numeroPersonas <= 0)
                throw new ArgumentException("Los minutos trabajados y el número de personas deben ser mayores a 0.");

            return paquetesTotales / (minutosTrabajados * numeroPersonas);
        }

        public double CalcularPM(double paquetesTotales, double minutosTrabajados)
        {
            if (minutosTrabajados <= 0)
                throw new ArgumentException("Los minutos trabajados deben ser mayores a 0.");

            return paquetesTotales / minutosTrabajados;
        }

        public double CalcularExtrapeso(double pesoReal, double pesoObjetivo, int paquetes)
        {
            return (pesoReal - (pesoObjetivo * paquetes)) / 1000;     //JMB.- Devolvemos el valor en Kg
        }

        public double CalcularFTT(int paquetesTotales, int paquetesRechazados)
        {
            if (paquetesTotales <= 0)
                return 0;

            return ((double)(paquetesTotales - paquetesRechazados) / paquetesTotales) * 100;
        }
        // Hora de inicio del lote
        public DateTime GetHoraInicioLote(DateTime horaInicio)
        {
            return horaInicio;
        }

        // Hora de fin aproximada
        public DateTime CalcularHoraFin(DateTime horaInicio, int paquetesRestantes, double mediaPaquetesPorMinuto)
        {
            if (mediaPaquetesPorMinuto <= 0)
                throw new ArgumentException("La media de paquetes por minuto debe ser mayor a 0.");

            //double minutosRestantes = paquetesProducidos / mediaPaquetesPorMinuto;
            //return horaInicio.AddMinutes(minutosRestantes);
            double minutosRestantes = paquetesRestantes / mediaPaquetesPorMinuto;
            return DateTime.Now.AddMinutes(minutosRestantes);
        }

        // Porcentaje del pedido completado
        public double CalcularPorcentajePedido(int paquetesProducidos, int paquetesRequeridos)
        {
            if (paquetesRequeridos <= 0)
                throw new ArgumentException("El número total de paquetes debe ser mayor a 0.");

            return (double)paquetesProducidos / paquetesRequeridos * 100;
        }

        /// <summary>
        /// Calcula el coste por kilo de la mano de obra directa.
        /// </summary>
        /// <param name="tiempoTotal">Tiempo total trabajado en horas.</param>
        /// <param name="costoHora">Costo estimado por hora de personal.</param>
        /// <param name="numeroPaquetes">Número total de paquetes producidos.</param>
        /// <param name="pesoMinimo">Peso mínimo por paquete en kilos.</param>
        /// <returns>Coste de mano de obra directa por kilo.</returns>
        public double CalcularCosteMOD(double tiempoTotal, double costoHora, int numeroPaquetes, double pesoMinimo)
        {
            //if (tiempoTotal <= 0 || costoHora <= 0 || numeroPaquetes <= 0 || pesoMinimo <= 0)
            if (numeroPaquetes <= 0 || pesoMinimo <= 0)         //JMB, permitimos valores nulos para tiempo total y costoHora
                throw new ArgumentException("Todos los valores deben ser mayores a 0.");

            double totalCosto = tiempoTotal * costoHora;
            double totalProduccion = (numeroPaquetes * pesoMinimo) / 1000;

            return totalCosto / totalProduccion;
        }
    }
}