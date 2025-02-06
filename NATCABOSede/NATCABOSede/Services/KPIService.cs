using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NATCABOSede.Interfaces;
using NATCABOSede.Models;
using NATCABOSede.ViewModels;

namespace NATCABOSede.Services
{
    public class KPIService : IKPIService
    {

        public double CalcularPPM(double paquetesTotales, double minutosTrabajados, int numeroPersonas)
        {

            //if (minutosTrabajados <= 0 || numeroPersonas <= 0)
            //    throw new ArgumentException("Los minutos trabajados y el número de personas deben ser mayores a 0.");

            if (minutosTrabajados>1 & numeroPersonas>0)
                return paquetesTotales / (minutosTrabajados * numeroPersonas);
            else return 0;
        }

        public double CalcularPM(double paquetesTotales, double minutosTrabajados)
        {
            //if (minutosTrabajados <= 0)
            //    throw new ArgumentException("Los minutos trabajados deben ser mayores a 0.");
            if (minutosTrabajados >1)
                return paquetesTotales / minutosTrabajados;
            else return 0;
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

        public double CalcularPorcentajeDesperdicio(double pesoDesperdicio, double pesoReal)
        {
            if (pesoReal <= 0)
                return 0;

            return (pesoDesperdicio*1000 / (pesoDesperdicio*1000 + pesoReal)) * 100;
        }


        // Hora de inicio del lote
        public DateTime GetHoraInicioLote(DateTime horaInicio)
        {
            return horaInicio;
        }

        // Hora de fin aproximada
        public DateTime CalcularHoraFin(DateTime horaInicio, int paquetesRestantes, double mediaPaquetesPorMinuto)
        {
            //if (mediaPaquetesPorMinuto <= 0)
            //    throw new ArgumentException("La media de paquetes por minuto debe ser mayor a 0.");

            if (mediaPaquetesPorMinuto > 0)
            {
                double minutosRestantes = paquetesRestantes / mediaPaquetesPorMinuto;
                return DateTime.Now.AddMinutes(minutosRestantes);
            }
            else return DateTime.Now;

        }

        // Porcentaje del pedido completado
        public double CalcularPorcentajePedido(int paquetesProducidos, int paquetesRequeridos)
        {
            //if (paquetesRequeridos <= 0)
            //    throw new ArgumentException("El número total de paquetes debe ser mayor a 0.");
            if (paquetesRequeridos > 0)
            {
                return (double)paquetesProducidos / paquetesRequeridos * 100;
            }
            else return 0;
            
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
        public DatosKpiViewModel GenerarDatosKpiViewModel(DatosKpi datos, double mediaPaquetesPorMinuto)
        {
            var ppm = datos.PPM_Marco;
            var ppm_disc = datos.PPM_Bizerba;
            var pm = CalcularPM(datos.PaquetesValidos, datos.MinutosTrabajados);
            var extraPeso = CalcularExtrapeso(datos.PesoTotalReal, datos.PesoObjetivo, datos.PaquetesValidos);
            var extraPeso_Disc = CalcularExtrapeso(datos.PesoTotalRealDisc, datos.PesoObjetivo, datos.PaquetesTotalesDisc - datos.PaquetesRechazadosDisc);
            //var horaFinAproximada = CalcularHoraFin(datos.HoraInicioProduccion, (int)(datos.PaquetesRequeridos - datos.PaquetesValidos), mediaPaquetesPorMinuto);
            var horaFinAproximada = CalcularHoraFin(datos.HoraInicioProduccion, (int)(datos.PaquetesRequeridos - datos.PaquetesValidos), datos.PpmPromedio_5min);
            var porcentajePedido = CalcularPorcentajePedido((int)(datos.PaquetesValidos), (int)(datos.PaquetesRequeridos));
            var costeMOD = CalcularCosteMOD(datos.HorasTotales, datos.CosteHora, datos.PaquetesValidos, datos.PesoObjetivo);
            var ppm5min = datos.PpmPromedio_5min / datos.NumeroOperadores;

            return new DatosKpiViewModel
            {
                Cliente = datos.NombreCliente ?? "*CLIENTE*",
                Producto = datos.NombreProducto ?? "*PRODUCTO*",
                PPM = ppm,
                PPM_Disc = ppm_disc,
                PM = pm,
                ExtraPeso = extraPeso,
                ExtraPeso_Disc = extraPeso_Disc,
                HoraInicio = datos.HoraInicioProduccion,
                HoraFinAproximada = horaFinAproximada,
                PorcentajePedido = porcentajePedido,
                CosteMOD = costeMOD,
                Ppm_5min = ppm5min

            };
        }
    }
}