using Microsoft.AspNetCore.Mvc;
using System;
using NATCABOSede.Areas.KPIS.Models;
using NATCABOSede.Models;
using NATCABOSede.Interfaces;
using ClosedXML.Excel;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;

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
                .Select(d => new { d.LineaId, d.NombreLinea })
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
            // var historico = _context.KpisHistoricos
            //    .OrderByDescending(h => h.Fecha)
            //    .Skip((page - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToList();
            ////lineas disponibles en el histórico
            // var lineas = _context.KpisHistoricos
            //    .Select(d => new { d.LineaId, d.SName })
            //    .ToList();

            // // Total de registros
            // var totalRecords = _context.KpisHistoricos.Count();

            // // Pasar datos a la vista
            // ViewBag.Historico = historico;
            // ViewBag.Page = page;
            // ViewBag.PageSize = pageSize;
            // ViewBag.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            // ViewBag.LineasDisponibles = lineas;

            // return View(historico);
        }
        [HttpPost]
        public IActionResult Filtrar([FromBody] FiltrarRequest request)
        {
            if (request == null)
            {
                return BadRequest("Solicitud inválida.");
            }
            var query = _context.KpisHistoricos.AsQueryable();

            //Filtro de linea si se proporciona
            if (request.LineaId.HasValue)
            {
                query = query.Where(h => h.LineaId == request.LineaId.Value);
            }
            // Filtrar por Confección si se proporciona y no está vacío
            if (!string.IsNullOrWhiteSpace(request.Confeccion))
            {
                query = query.Where(h => h.Confeccion == request.Confeccion);
            }
            if (request.Desde.HasValue)
            {
                query = query.Where(h => h.Fecha >= request.Desde.Value);
            }

            if (request.Hasta.HasValue)
            {
                query = query.Where(h => h.Fecha <= request.Hasta.Value);
            }

            //var resultados = query.OrderByDescending(h => h.Fecha).ToList();
            // Ordenar y aplicar paginación
            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)request.PageSize);

            var resultados = query.OrderByDescending(h => h.Fecha)
                                  .Skip((request.Page - 1) * request.PageSize)
                                  .Take(request.PageSize)
                                  .ToList();

            ////Preparar respuesta
            var response = new
            {
                Data = resultados,
                TotalPages = totalPages
            };

            return Json(response);
            
        }
        [HttpPost]
        public IActionResult ExportarExcel([FromBody] FiltrarRequest request)
        {
            if (request == null)
            {
                return BadRequest("Solicitud inválida.");
            }

            var query = _context.KpisHistoricos.AsQueryable();

            // Filtrar por Línea si se proporciona
            if (request.LineaId.HasValue)
            {
                query = query.Where(h => h.LineaId == request.LineaId.Value);
            }

            // Filtrar por Confección si se proporciona y no está vacío
            if (!string.IsNullOrWhiteSpace(request.Confeccion))
            {
                query = query.Where(h => h.Confeccion == request.Confeccion);
            }

            // Filtrar por fecha desde si se proporciona
            if (request.Desde.HasValue)
            {
                query = query.Where(h => h.Fecha >= request.Desde.Value);
            }

            // Filtrar por fecha hasta si se proporciona
            if (request.Hasta.HasValue)
            {
                query = query.Where(h => h.Fecha <= request.Hasta.Value);
            }

            var resultados = query.OrderByDescending(h => h.Fecha).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("KPIs");

                // Encabezados
                worksheet.Cell(1, 1).Value = "Fecha Lote";
                worksheet.Cell(1, 2).Value = "Línea";
                worksheet.Cell(1, 3).Value = "Confección";
                worksheet.Cell(1, 4).Value = "PPM";
                worksheet.Cell(1, 5).Value = "PM Marco";
                worksheet.Cell(1, 6).Value = "PM Bizerba";
                worksheet.Cell(1, 7).Value = "Extrapeso Marco";
                worksheet.Cell(1, 8).Value = "Extrapeso Bizerba";
                worksheet.Cell(1, 9).Value = "Desecho (Kg)";
                worksheet.Cell(1, 10).Value = "Desecho (%)";
                worksheet.Cell(1, 11).Value = "FTT";
                worksheet.Cell(1, 12).Value = "MOD";

                // Estilo a los encabezados
                var headerRange = worksheet.Range("A1:L1");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int row = 2;
                foreach (var item in resultados)
                {
                    worksheet.Cell(row, 1).Value = item.Fecha?.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 2).Value = item.NombreLinea;
                    worksheet.Cell(row, 3).Value = item.Confeccion;
                    worksheet.Cell(row, 4).Value = item.PPM_Marco;
                    worksheet.Cell(row, 5).Value = item.PM_Marco;
                    worksheet.Cell(row, 6).Value = item.PM_Bizerba;
                    worksheet.Cell(row, 7).Value = item.Extrapeso_Marco;
                    worksheet.Cell(row, 8).Value = item.Extrapeso_Bizerba;
                    worksheet.Cell(row, 9).Value = item.Desecho_Kg;
                    worksheet.Cell(row, 10).Value = item.Desecho_Perc;
                    worksheet.Cell(row, 11).Value = item.FTT;
                    worksheet.Cell(row, 12).Value = item.MOD;
                    row++;
                }

                // Ajusta ancho de columnas automáticamente
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "KpisHistorico.xlsx");
                }
            }
        }

        // Recuperación de líneas desde la vista de históricos en la bbdd
        [HttpGet]
        public JsonResult ObtenerLineasHistorico()
        {
            var lineas = _context.KpisHistoricos
                .Select(d => new { d.LineaId, d.NombreLinea })
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
                .ToList();

            if (!confecciones.Any())
            {
                Console.WriteLine("No data found in KPIsHistorico table");
            }

            return Json(confecciones);
        }
    }
}


