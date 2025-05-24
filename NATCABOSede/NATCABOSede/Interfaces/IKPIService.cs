using NATCABOSede.Models;
using NATCABOSede.ViewModels;

namespace NATCABOSede.Interfaces
{
    /// <summary>
    /// Interfaz que define los servicios para el cálculo y gestión de KPIs de producción.
    /// </summary>
    public interface IKPIService
    {
        /// <summary>
        /// Calcula los Paquetes Por Minuto (PPM) ajustados por personal.
        /// </summary>
        /// <param name="paquetesTotales">Cantidad total de paquetes producidos.</param>
        /// <param name="minutosTrabajados">Tiempo total de trabajo en minutos.</param>
        /// <param name="numeroPersonas">Número de personas trabajando.</param>
        /// <returns>Valor de PPM ajustado.</returns>
        double CalcularPPM(double paquetesTotales, double minutosTrabajados, int numeroPersonas);

        /// <summary>
        /// Calcula el rendimiento de paquetes por minuto (PM).
        /// </summary>
        /// <param name="paquetesTotales">Cantidad total de paquetes.</param>
        /// <param name="minutosTrabajados">Tiempo total de trabajo en minutos.</param>
        /// <returns>Rendimiento en paquetes por minuto.</returns>
        double CalcularPM(double paquetesTotales, double minutosTrabajados);

        /// <summary>
        /// Calcula el peso extra en los paquetes producidos.
        /// </summary>
        /// <param name="pesoReal">Peso real de los paquetes.</param>
        /// <param name="pesoObjetivo">Peso objetivo esperado.</param>
        /// <param name="paquetes">Número de paquetes.</param>
        /// <returns>Peso total extra en kilogramos.</returns>
        double CalcularExtrapeso(double pesoReal, double pesoObjetivo, int paquetes);

        /// <summary>
        /// Calcula la hora estimada de finalización de la producción.
        /// </summary>
        /// <param name="horaInicio">Hora de inicio de la producción.</param>
        /// <param name="paquetesRestantes">Cantidad de paquetes que faltan por producir.</param>
        /// <param name="mediaPaquetesPorMinuto">Ritmo de producción en paquetes por minuto.</param>
        /// <returns>Fecha y hora estimada de finalización.</returns>
        DateTime CalcularHoraFin(DateTime horaInicio, int paquetesRestantes, double mediaPaquetesPorMinuto);

        /// <summary>
        /// Calcula el porcentaje de avance del pedido.
        /// </summary>
        /// <param name="paquetesProducidos">Cantidad de paquetes ya producidos.</param>
        /// <param name="paquetesTotales">Cantidad total de paquetes del pedido.</param>
        /// <returns>Porcentaje de avance entre 0 y 100.</returns>
        double CalcularPorcentajePedido(int paquetesProducidos, int paquetesTotales);

        /// <summary>
        /// Calcula el costo de la Mano de Obra Directa (MOD) por paquete.
        /// </summary>
        /// <param name="tiempoTotal">Tiempo total de producción en horas.</param>
        /// <param name="costoHora">Costo por hora de mano de obra.</param>
        /// <param name="numeroPaquetes">Cantidad de paquetes producidos.</param>
        /// <param name="pesoMinimo">Peso mínimo por paquete en kilogramos.</param>
        /// <returns>Costo de MOD por paquete.</returns>
        double CalcularCosteMOD(double tiempoTotal, double costoHora, int numeroPaquetes, double pesoMinimo);

        /// <summary>
        /// Calcula el indicador First Time Through (FTT).
        /// </summary>
        /// <param name="paquetesTotales">Total de paquetes producidos.</param>
        /// <param name="paquetesRechazados">Cantidad de paquetes rechazados.</param>
        /// <returns>Porcentaje de paquetes que pasan la inspección en el primer intento.</returns>
        double CalcularFTT(int paquetesTotales, int paquetesRechazados);

        /// <summary>
        /// Calcula el porcentaje de desperdicio de material.
        /// </summary>
        /// <param name="pesoDesperdicio">Peso del material desperdiciado.</param>
        /// <param name="pesoReal">Peso total del material utilizado.</param>
        /// <returns>Porcentaje de desperdicio.</returns>
        double CalcularPorcentajeDesperdicio(double pesoDesperdicio, double pesoReal);

        /// <summary>
        /// Genera el modelo de vista con los datos de KPIs para la vista.
        /// </summary>
        /// <param name="datos">Datos en bruto de los KPIs.</param>
        /// <param name="mediaPaquetesPorMinuto">Media de paquetes por minuto.</param>
        /// <returns>Modelo de vista con los datos formateados.</returns>
        DatosKpiViewModel GenerarDatosKpiViewModel(DatosKpisLive datos, double mediaPaquetesPorMinuto);
    }
}
