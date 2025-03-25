using System;
using System.Collections.Generic;

namespace NATCABOSede.Models;

public partial class DatosKpisLive
{
    public short? IdLinea { get; set; }

    public string? NombreLinea { get; set; }

    public string? NombreLote { get; set; }

    public string? NombreProducto { get; set; }

    public string? NombreCliente { get; set; }

    public DateTime? HoraInicioLote { get; set; }

    public int? PaquetesValidos { get; set; }

    public int? NumeroOperadores { get; set; }

    public double? PesoObjetivo { get; set; }

    public int? PaquetesRequeridos { get; set; }

    public double? PpmObjetivo { get; set; }

    public double? FInputWeight { get; set; }

    public double? PesoTotalDesperdicio { get; set; }

    public double? CosteHora { get; set; }

    public double? CosteKg { get; set; }

    public int? PaquetesTotales { get; set; }

    public double? PesoTotalReal { get; set; }

    public DateTime? HoraInicioProduccion { get; set; }

    public DateTime? HoraUltimoPaquete { get; set; }

    public int? Id { get; set; }

    public int? TotalDowntime { get; set; }

    public int? MinutosTrabajados { get; set; }

    public int? NumberOfMinutes { get; set; }

    public int? LBatchId { get; set; }

    public double? PpmMarco { get; set; }

    public double? PpmBizerba { get; set; }

    public double? NPaquetes5min { get; set; }

    public double? TotalHours { get; set; }

    public double? PmBizerba { get; set; }

    public int? DeviceMachineNo { get; set; }

    public double? PmBizerbaTotal { get; set; }

    public int? PaquetesValidosDisc { get; set; }

    public int? PaquetesRechazadosDisc { get; set; }

    public int? PesoTotalRealDisc { get; set; }

    public int? PaquetesTotalesDisc { get; set; }

    public int? PersonalEnBalanza { get; set; }

    public int? PersonalTotal { get; set; }

    public int? PersonalPeriferico { get; set; }

    public int PersonalCorrecto { get; set; }

    public int DiscriminadorEnUso { get; set; }

    public double? PpmPersonalEnBalanza { get; set; }

    public double PpmObjPersonaEnBalanza { get; set; }

    public double? PpmLinea { get; set; }

    public double PpmObj { get; set; }

    public double ModObj { get; set; }

    public double FttObj { get; set; }

    public int PpmBalanzasOk { get; set; }

    public int PpmLineaOk { get; set; }

    public double? Mod { get; set; }

    public int? Ftt { get; set; }

    public int RangosOk { get; set; }

    public int ExpulsionAireOk { get; set; }
}
