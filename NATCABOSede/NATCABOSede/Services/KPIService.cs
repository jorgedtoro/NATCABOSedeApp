using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NATCABOSede.Interfaces;
using NATCABOSede.Models;
using NATCABOSede.ViewModels;

namespace NATCABOSede.Services
{
    /// <summary>
    /// Implementación del servicio para el cálculo y gestión de KPIs de producción.
    /// Proporciona métodos para calcular métricas clave de rendimiento en líneas de producción.
    /// </summary>
    public class KPIService : IKPIService
    {

        /// <summary>
        /// Calcula los Paquetes Por Minuto (PPM) ajustados por personal.
        /// </summary>
        /// <param name="paquetesTotales">Cantidad total de paquetes producidos.</param>
        /// <param name="minutosTrabajados">Tiempo total de trabajo en minutos.</param>
        /// <param name="numeroPersonas">Número de personas trabajando.</param>
        /// <returns>Valor de PPM ajustado. Retorna 0 si los parámetros no son válidos.</returns>
        public double CalcularPPM(double paquetesTotales, double minutosTrabajados, int numeroPersonas)
        {
            if (minutosTrabajados > 1 && numeroPersonas > 0)
                return paquetesTotales / (minutosTrabajados * numeroPersonas);
            return 0;
        }

        /// <summary>
        /// Calcula el rendimiento de paquetes por minuto (PM).
        /// </summary>
        /// <param name="paquetesTotales">Cantidad total de paquetes.</param>
        /// <param name="minutosTrabajados">Tiempo total de trabajo en minutos.</param>
        /// <returns>Rendimiento en paquetes por minuto. Retorna 0 si el tiempo es menor o igual a 1 minuto.</returns>
        public double CalcularPM(double paquetesTotales, double minutosTrabajados)
        {
            if (minutosTrabajados > 1)
                return paquetesTotales / minutosTrabajados;
            return 0;
        }

        /// <summary>
        /// Calcula el peso extra en los paquetes producidos.
        /// </summary>
        /// <param name="pesoReal">Peso real de los paquetes en gramos.</param>
        /// <param name="pesoObjetivo">Peso objetivo esperado por paquete en gramos.</param>
        /// <param name="paquetes">Número de paquetes.</param>
        /// <returns>Peso total extra en kilogramos.</returns>
        public double CalcularExtrapeso(double pesoReal, double pesoObjetivo, int paquetes)
        {
            return (pesoReal - (pesoObjetivo * paquetes)) / 1000; // Convertir a kilogramos
        }

        /// <summary>
        /// Calcula el indicador First Time Through (FTT).
        /// Mide el porcentaje de paquetes que pasan la inspección en el primer intento.
        /// </summary>
        /// <param name="paquetesTotales">Total de paquetes producidos.</param>
        /// <param name="paquetesRechazados">Cantidad de paquetes rechazados.</param>
        /// <returns>Porcentaje de paquetes que pasan la inspección en el primer intento.</returns>
        public double CalcularFTT(int paquetesTotales, int paquetesRechazados)
        {
            if (paquetesTotales <= 0)
                return 0;

            return ((double)(paquetesTotales - paquetesRechazados) / paquetesTotales) * 100;
        }

        /// <summary>
        /// Calcula el porcentaje de desperdicio de material.
        /// </summary>
        /// <param name="pesoDesperdicio">Peso del material desperdiciado en kilogramos.</param>
        /// <param name="pesoReal">Peso total del material utilizado en gramos.</param>
        /// <returns>Porcentaje de desperdicio. Retorna 0 si el peso real es 0.</returns>
        public double CalcularPorcentajeDesperdicio(double pesoDesperdicio, double pesoReal)
        {
            if (pesoReal <= 0)
                return 0;

            return (pesoDesperdicio * 1000 / (pesoDesperdicio * 1000 + pesoReal)) * 100;
        }


        /// <summary>
        /// Obtiene la hora de inicio del lote.
        /// </summary>
        /// <param name="horaInicio">Hora de inicio proporcionada.</param>
        /// <returns>La misma hora de inicio sin modificaciones.</returns>
        public DateTime GetHoraInicioLote(DateTime horaInicio)
        {
            return horaInicio;
        }

        /// <summary>
        /// Calcula la hora estimada de finalización de la producción.
        /// </summary>
        /// <param name="horaInicio">Hora de inicio de la producción.</param>
        /// <param name="paquetesRestantes">Cantidad de paquetes que faltan por producir.</param>
        /// <param name="mediaPaquetesPorMinuto">Ritmo de producción en paquetes por minuto.</param>
        /// <returns>Fecha y hora estimada de finalización. Retorna la hora actual si no se puede calcular.</returns>
        public DateTime CalcularHoraFin(DateTime horaInicio, int paquetesRestantes, double mediaPaquetesPorMinuto)
        {
            if (mediaPaquetesPorMinuto > 0)
            {
                double minutosRestantes = paquetesRestantes / mediaPaquetesPorMinuto;
                return DateTime.Now.AddMinutes(minutosRestantes);
            }
            return DateTime.Now;
        }

        /// <summary>
        /// Calcula el porcentaje de avance del pedido.
        /// </summary>
        /// <param name="paquetesProducidos">Cantidad de paquetes ya producidos.</param>
        /// <param name="paquetesRequeridos">Cantidad total de paquetes del pedido.</param>
        /// <returns>Porcentaje de avance entre 0 y 100. Retorna 0 si no hay paquetes requeridos.</returns>
        public double CalcularPorcentajePedido(int paquetesProducidos, int paquetesRequeridos)
        {
            if (paquetesRequeridos > 0)
            {
                return (double)paquetesProducidos / paquetesRequeridos * 100;
            }
            return 0;
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
            //if (numeroPaquetes <= 0 || pesoMinimo <= 0)         //JMB, permitimos valores nulos para tiempo total y costoHora
            //    throw new ArgumentException("Todos los valores deben ser mayores a 0.");

            if (numeroPaquetes > 0 & pesoMinimo > 0)
            {
                double totalCosto = tiempoTotal * costoHora;
                double totalProduccion = (numeroPaquetes * pesoMinimo) / 1000;
                return totalCosto / totalProduccion;
            }
            else return 0;
        }
        /// <summary>
        /// Genera el modelo de vista con los datos de KPIs para la vista.
        /// </summary>
        /// <param name="datos">Datos en bruto de los KPIs.</param>
        /// <param name="mediaPaquetesPorMinuto">Media de paquetes por minuto.</param>
        /// <returns>Modelo de vista con los datos formateados.</returns>
        public DatosKpiViewModel GenerarDatosKpiViewModel(DatosKpisLive datos, double mediaPaquetesPorMinuto)
        {
            //Jorge--> Manejo de valores nulos con ?? para evitar excepciones
            var ppm = datos.PpmMarco ?? 0.0;
            var ppm_disc = datos.PpmBizerba ?? 0.0;
            var pm = CalcularPM(datos.PaquetesValidos ?? 0, datos.MinutosTrabajados ?? 1);
            var extraPeso = CalcularExtrapeso(datos.PesoTotalReal ?? 0.0, datos.PesoObjetivo ?? 1.0, datos.PaquetesValidos ?? 1);
            var extraPeso_Disc = CalcularExtrapeso(datos.PesoTotalRealDisc ?? 0.0, datos.PesoObjetivo ?? 1.0, (datos.PaquetesTotalesDisc ?? 0) - (datos.PaquetesRechazadosDisc ?? 0));

            // Evitar excepciones en cálculos con valores nulos
            var horaFinAproximada = CalcularHoraFin(
                datos.HoraInicioProduccion ?? DateTime.Now,
                (datos.PaquetesRequeridos ?? 0) - (datos.PaquetesValidos ?? 0),
                datos.NPaquetes5min ?? 1.0
            );

            var porcentajePedido = CalcularPorcentajePedido(datos.PaquetesValidos ?? 0, datos.PaquetesRequeridos ?? 1);
            var costeMOD = CalcularCosteMOD(datos.TotalHours ?? 0.0, datos.CosteHora ?? 0.0, datos.PaquetesValidos ?? 1, datos.PesoObjetivo ?? 1.0);
            var ppm5min = (datos.NPaquetes5min ?? 0.0) / (datos.NumeroOperadores ?? 1);

            return new DatosKpiViewModel
            {
                Cliente = datos.NombreCliente ?? "*CLIENTE*",
                Producto = datos.NombreProducto ?? "*PRODUCTO*",
                PPM = ppm,
                PPM_Disc = ppm_disc,
                PM = pm,
                ExtraPeso = extraPeso,
                ExtraPeso_Disc = extraPeso_Disc,
                HoraInicio = datos.HoraInicioProduccion ?? DateTime.Now,
                HoraFinAproximada = horaFinAproximada,
                PorcentajePedido = porcentajePedido,
                CosteMOD = costeMOD,
                Ppm_5min = ppm5min
            };
        }
    }
}