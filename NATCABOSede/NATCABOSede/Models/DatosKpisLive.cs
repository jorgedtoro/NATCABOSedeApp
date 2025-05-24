using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NATCABOSede.Models;

[Table("DatosKpisLive")]
public partial class DatosKpisLive
{
    [Key]
    [Column("Id")]
    public int? Id { get; set; }

    [Column("IdLinea")]
    public short? IdLinea { get; set; }

    [Column("NombreLinea")]
    public string? NombreLinea { get; set; }

    [Column("NombreLote")]
    public string? NombreLote { get; set; }

    [Column("NombreProducto")]
    public string? NombreProducto { get; set; }

    [Column("NombreCliente")]
    public string? NombreCliente { get; set; }

    [Column("HoraInicioLote")]
    public DateTime? HoraInicioLote { get; set; }

    [Column("PaquetesValidos")]
    public int? PaquetesValidos { get; set; }

    [Column("NumeroOperadores")]
    public int? NumeroOperadores { get; set; }

    [Column("PesoObjetivo")]
    public double? PesoObjetivo { get; set; }

    [Column("PaquetesRequeridos")]
    public int? PaquetesRequeridos { get; set; }
    
    [Column("PpmObjetivo")]
    public double? PpmObjetivo { get; set; }
    
    [Column("fInputWeight")]
    public double? FInputWeight { get; set; }
    
    [Column("PesoTotalDesperdicio")]
    public double? PesoTotalDesperdicio { get; set; }
    
    [Column("costeHora")]
    public double? CosteHora { get; set; }
    
    [Column("costeKg")]
    public double? CosteKg { get; set; }
    
    [Column("PaquetesTotales")]
    public int? PaquetesTotales { get; set; }
    
    [Column("PesoTotalReal")]
    public double? PesoTotalReal { get; set; }
    
    [Column("horaInicioProduccion")]
    public DateTime? HoraInicioProduccion { get; set; }
    
    [Column("horaUltimoPaquete")]
    public DateTime? HoraUltimoPaquete { get; set; }
    
    [Column("TotalDowntime")]
    public int? TotalDowntime { get; set; }
    
    [Column("minutosTrabajados")]
    public int? MinutosTrabajados { get; set; }
    
    [Column("NumberOfMinutes")]
    public int? NumberOfMinutes { get; set; }
    
    [Column("lBatchID")]
    public int? LBatchId { get; set; }
    
    [Column("PPM_Marco")]
    public double? PpmMarco { get; set; }
    
    [Column("PPM_Bizerba")]
    public double? PpmBizerba { get; set; }
    
    [Column("nPaquetes_5min")]
    public double? NPaquetes5min { get; set; }
    
    [Column("TotalHours")]
    public double? TotalHours { get; set; }
    
    [Column("PM_Bizerba")]
    public double? PmBizerba { get; set; }
    
    [Column("DeviceMachineNo")]
    public int? DeviceMachineNo { get; set; }
    
    [Column("PM_Bizerba_Total")]
    public double? PmBizerbaTotal { get; set; }
    
    [Column("paquetesValidos_Disc")]
    public int? PaquetesValidosDisc { get; set; }
    
    [Column("paquetesRechazados_Disc")]
    public int? PaquetesRechazadosDisc { get; set; }
    
    [Column("pesoTotalReal_Disc")]
    public int? PesoTotalRealDisc { get; set; }
    
    [Column("paquetesTotales_Disc")]
    public int? PaquetesTotalesDisc { get; set; }
    
    [Column("personalEnBalanza")]
    public int? PersonalEnBalanza { get; set; }
    
    [Column("personalTotal")]
    public int? PersonalTotal { get; set; }
    
    [Column("personalPeriferico")]
    public int? PersonalPeriferico { get; set; }
    
    [Column("personalCorrecto")]
    public int PersonalCorrecto { get; set; }
    
    [Column("discriminadorEnUso")]
    public int DiscriminadorEnUso { get; set; }
    
    [Column("PPM_PersonalEnBalanza")]
    public double? PpmPersonalEnBalanza { get; set; }
    
    [Column("PPM_Obj_PersonaEnBalanza")]
    public double PpmObjPersonaEnBalanza { get; set; }
    
    [Column("PPM_Linea")]
    public double? PpmLinea { get; set; }
    
    [Column("PPM_Obj")]
    public double PpmObj { get; set; }
    
    [Column("MOD_Obj")]
    public double ModObj { get; set; }
    
    [Column("FTT_Obj")]
    public double FttObj { get; set; }
    
    [Column("PPM_Balanzas_Ok")]
    public int PpmBalanzasOk { get; set; }
    
    [Column("PPM_Linea_Ok")]
    public int PpmLineaOk { get; set; }
    
    [Column("MOD")]
    public double? Mod { get; set; }
    
    [Column("FTT")]
    public int? Ftt { get; set; }
    
    [Column("rangos_Ok")]
    public int RangosOk { get; set; }
    
    [Column("expulsionAire_Ok")]
    public int ExpulsionAireOk { get; set; }
}
