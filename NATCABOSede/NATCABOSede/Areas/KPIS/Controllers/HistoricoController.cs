using Microsoft.AspNetCore.Mvc;
using System;
using NATCABOSede.Areas.KPIS.Models;
using NATCABOSede.Models;
using NATCABOSede.Interfaces;
using ClosedXML.Excel;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NATCABOSede.Models.Metadata;

namespace NATCABOSede.Areas.KPIS.Controllers
{
    [Area("KPIS")]
    public class HistoricoController : Controller
    {
        private readonly NATCABOContext _context;
        private readonly IKPIService _kpiService;

        public HistoricoController(NATCABOContext context)
        {
            _context = context;
        }

        public IActionResult Historico(int page = 1, int pageSize = 25)
        {
            // No cargar datos históricos inicialmente
            // Obtener líneas disponibles
            var lineas = _context.KpisHistoricos
                .Select(d => new { d.IdLinea, d.NombreLinea })
                .Distinct()
                .ToList();

            ViewBag.LineasDisponibles = lineas;

            // Obtener confecciones disponibles
            var confecciones = _context.KpisHistoricos
                .Select(d => d.Confeccion)
                .Distinct()
                .ToList();

            ViewBag.ConfeccionesDisponibles = confecciones;

            return View();
        
        }
        [HttpPost]
        public async Task<IActionResult> Filtrar([FromBody] FiltrarRequest request)
        {
            if (request == null)
            {
                return BadRequest("Solicitud inválida.");
            }

            try
            {
                // Preparar los parámetros para el SP
                var idLineaParam = new SqlParameter("@idLinea", System.Data.SqlDbType.Int);
                idLineaParam.Value = request.IdLinea.HasValue ? (object)request.IdLinea.Value : DBNull.Value;

                var confeccionParam = new SqlParameter("@confeccion", System.Data.SqlDbType.NVarChar, 50);
                confeccionParam.Value = string.IsNullOrWhiteSpace(request.Confeccion) ? (object)DBNull.Value : request.Confeccion;

                var desdeParam = new SqlParameter("@desde", request.Desde);
                //var hastaParam = new SqlParameter("@hasta", request.Hasta);
                var hastaParam = new SqlParameter("@hasta", request.Hasta.HasValue
                    ? request.Hasta.Value.Date.AddDays(1).AddTicks(-1)
                    :DBNull.Value);

                // Ejecutar el procedimiento almacenado y mapear el resultado al DTO
                var data = await _context.KpisHistoricoDtos
                    .FromSqlRaw("EXEC dbo.Filtrar_DatosKPIs_Historico @idLinea, @confeccion, @desde, @hasta",
                                idLineaParam, confeccionParam, desdeParam, hastaParam)
                    .ToListAsync();

                // Aplicar paginación en memoria (el SP no incluye paginación)
                var totalRecords = data.Count;
                var totalPages = (int)Math.Ceiling(totalRecords / (double)request.PageSize);
                var pagedData = data
                    .OrderByDescending(d => d.Fecha)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                return Json(new { data = pagedData, totalPages });
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex);
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExportarExcel([FromBody] FiltrarRequest request)
        {
            if (request == null)
            {
                return BadRequest("Solicitud inválida.");
            }

            // Preparar parámetros para el SP
            var IdLineaParam = new SqlParameter("@IdLinea", System.Data.SqlDbType.Int);
            IdLineaParam.Value = request.IdLinea.HasValue ? (object)request.IdLinea.Value : DBNull.Value;

            var confeccionParam = new SqlParameter("@confeccion", System.Data.SqlDbType.NVarChar, 50);
            confeccionParam.Value = string.IsNullOrWhiteSpace(request.Confeccion) ? (object)DBNull.Value : request.Confeccion;

            var desdeParam = new SqlParameter("@desde", request.Desde);
            var hastaParam = new SqlParameter("@hasta", request.Hasta);

            // Ejecutar el procedimiento almacenado
            var data = await _context.KpisHistoricoDtos
                .FromSqlRaw("EXEC dbo.Filtrar_DatosKPIs_Historico @idLinea, @confeccion, @desde, @hasta",
                    IdLineaParam, confeccionParam, desdeParam, hastaParam)
                .ToListAsync();

            // Generar el archivo Excel
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("KPIs");

                // Encabezados (ajusta según los campos que devuelve el SP)
                worksheet.Cell(1, 1).Value = "Fecha";
                worksheet.Cell(1, 2).Value = "NombreLinea";
                worksheet.Cell(1, 3).Value = "Confeccion";
                worksheet.Cell(1, 4).Value = "MOD";
                worksheet.Cell(1, 5).Value = "PPM_Marco";
                worksheet.Cell(1, 6).Value = "PPM_Bizerba";
                worksheet.Cell(1, 7).Value = "PM_Marco";
                worksheet.Cell(1, 8).Value = "PM_Bizerba";
                worksheet.Cell(1, 9).Value = "Extrapeso_Marco";
                worksheet.Cell(1, 10).Value = "Extrapeso_Bizerba";
                worksheet.Cell(1, 11).Value = "FTT";
                worksheet.Cell(1, 12).Value = "Desecho_Kg";
                worksheet.Cell(1, 13).Value = "Desecho_Perc";
                worksheet.Cell(1, 14).Value = "paquetesValidos";
                worksheet.Cell(1, 15).Value = "pesoTotalReal";
                worksheet.Cell(1, 16).Value = "TotalHours";
              
                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cell(row, 1).Value = item.Fecha?.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 2).Value = item.nombreLinea;
                    worksheet.Cell(row, 3).Value = item.Confeccion;
                    worksheet.Cell(row, 4).Value = item.MOD;
                    worksheet.Cell(row, 5).Value = item.PPM_Marco;
                    worksheet.Cell(row, 6).Value = item.PPM_Bizerba;
                    worksheet.Cell(row, 7).Value = item.PM_Marco;
                    worksheet.Cell(row, 8).Value = item.PM_Bizerba;
                    worksheet.Cell(row, 9).Value = item.Extrapeso_Marco;
                    worksheet.Cell(row, 10).Value = item.Extrapeso_Bizerba;
                    worksheet.Cell(row, 11).Value = item.FTT;
                    worksheet.Cell(row, 12).Value = item.Desecho_Kg;
                    worksheet.Cell(row, 13).Value = item.Desecho_Perc;
                    worksheet.Cell(row, 14).Value = item.paquetesValidos;
                    worksheet.Cell(row, 15).Value = item.pesoTotalReal;
                    worksheet.Cell(row, 16).Value = item.TotalHours;
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "KpisHistorico.xlsx");
                }
            }
        }
        //[HttpPost]
        //public IActionResult Filtrar([FromBody] FiltrarRequest request)
        //{
        //    if (request == null)
        //    {
        //        return BadRequest("Solicitud inválida.");
        //    }
        //    var query = _context.KpisHistoricos.AsQueryable();

        //    //Filtro de linea si se proporciona
        //    if (request.LineaId.HasValue)
        //    {
        //        query = query.Where(h => h.IdLinea == request.LineaId.Value);
        //    }
        //    // Filtrar por Confección si se proporciona y no está vacío
        //    if (!string.IsNullOrWhiteSpace(request.Confeccion))
        //    {
        //        query = query.Where(h => h.Confeccion == request.Confeccion);
        //    }
        //    if (request.Desde.HasValue)
        //    {
        //        query = query.Where(h => h.Fecha >= request.Desde.Value);
        //    }

        //    if (request.Hasta.HasValue)
        //    {
        //        query = query.Where(h => h.Fecha <= request.Hasta.Value);
        //    }

        //    //var resultados = query.OrderByDescending(h => h.Fecha).ToList();
        //    // Ordenar y aplicar paginación
        //    var totalRecords = query.Count();
        //    var totalPages = (int)Math.Ceiling(totalRecords / (double)request.PageSize);

        //    var resultados = query.OrderByDescending(h => h.Fecha)
        //                          .Skip((request.Page - 1) * request.PageSize)
        //                          .Take(request.PageSize)
        //                          .ToList();

        //    ////Preparar respuesta
        //    var response = new
        //    {
        //        Data = resultados,
        //        TotalPages = totalPages
        //    };

        //    return Json(response);

        //}
        //[HttpPost]
        //public IActionResult ExportarExcel([FromBody] FiltrarRequest request)
        //{
        //    if (request == null)
        //    {
        //        return BadRequest("Solicitud inválida.");
        //    }

        //    var query = _context.KpisHistoricos.AsQueryable();

        //    // Filtrar por Línea si se proporciona
        //    if (request.LineaId.HasValue)
        //    {
        //        query = query.Where(h => h.IdLinea == request.LineaId.Value);
        //    }

        //    // Filtrar por Confección si se proporciona y no está vacío
        //    if (!string.IsNullOrWhiteSpace(request.Confeccion))
        //    {
        //        query = query.Where(h => h.Confeccion == request.Confeccion);
        //    }

        //    // Filtrar por fecha desde si se proporciona
        //    if (request.Desde.HasValue)
        //    {
        //        query = query.Where(h => h.Fecha >= request.Desde.Value);
        //    }

        //    // Filtrar por fecha hasta si se proporciona
        //    if (request.Hasta.HasValue)
        //    {
        //        query = query.Where(h => h.Fecha <= request.Hasta.Value);
        //    }

        //    var resultados = query.OrderByDescending(h => h.Fecha).ToList();

        //    using (var workbook = new XLWorkbook())
        //    {
        //        var worksheet = workbook.Worksheets.Add("KPIs");

        //        // Encabezados
        //        worksheet.Cell(1, 1).Value = "Producto";
        //        worksheet.Cell(1, 2).Value = "Lote";
        //        worksheet.Cell(1, 3).Value = "Confección";
        //        worksheet.Cell(1, 4).Value = "N° Paquetes";
        //        worksheet.Cell(1, 5).Value = "N° Minutos";
        //        worksheet.Cell(1, 6).Value = "N° Operarios";
        //        worksheet.Cell(1, 7).Value = "Peso Total";
        //        worksheet.Cell(1, 8).Value = "Target";
        //        worksheet.Cell(1, 9).Value = "KPI PPM";
        //        worksheet.Cell(1, 10).Value = "KPI PM";
        //        worksheet.Cell(1, 11).Value = "KPI Extrapeso";
        //        worksheet.Cell(1, 12).Value = "Fecha Registro";

        //        // Estilo a los encabezados
        //        var headerRange = worksheet.Range("A1:L1");
        //        headerRange.Style.Font.Bold = true;
        //        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
        //        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        //        int row = 2;
        //        foreach (var item in resultados)
        //        {
        //           // worksheet.Cell(row, 1).Value = item.SName;
        //           // worksheet.Cell(row, 2).Value = item.Lote;
        //            worksheet.Cell(row, 3).Value = item.Confeccion;
        //           // worksheet.Cell(row, 4).Value = item.NPaquetes;
        //           // worksheet.Cell(row, 5).Value = item.NMinutos;
        //          //  worksheet.Cell(row, 6).Value = item.NOperarios;
        //          //  worksheet.Cell(row, 7).Value = item.TotalWeight;
        //          //  worksheet.Cell(row, 8).Value = item.FTarget;
        //           // worksheet.Cell(row, 9).Value = item.KpiPpm;
        //          //  worksheet.Cell(row, 10).Value = item.KpiPm;
        //          //  worksheet.Cell(row, 11).Value = item.KpiExtrapeso;
        //            worksheet.Cell(row, 12).Value = item.Fecha?.ToString("dd/MM/yyyy");
        //            row++;
        //        }

        //        // Ajusta ancho de columnas automáticamente
        //        worksheet.Columns().AdjustToContents();

        //        using (var stream = new MemoryStream())
        //        {
        //            workbook.SaveAs(stream);
        //            var content = stream.ToArray();
        //            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "KpisHistorico.xlsx");
        //        }
        //    }
        //}

        // Recuperación de líneas desde la vista de históricos en la bbdd
        [HttpGet]
        public JsonResult ObtenerLineasHistorico()
        {
            var lineas = _context.KpisHistoricos
                .Select(d => new { d.IdLinea, d.NombreLinea })
                 .Distinct()
                .ToList();

            if (!lineas.Any())
            {
                Console.WriteLine("No data found in KPIsHistorico table");
            }

            return Json(lineas);
        }

        // Recuperación de confecciones desde la vista de históricos en la bbdd
        [HttpGet]
        public JsonResult ObtenerConfeccionesHistorico()
        {
            var confecciones = _context.KpisHistoricos
                .Select(d => new { d.Confeccion })
                 .Distinct()
                .ToList();

            if (!confecciones.Any())
            {
                Console.WriteLine("No data found in KPIsHistorico table");
            }

            return Json(confecciones);
        }
    }
}


