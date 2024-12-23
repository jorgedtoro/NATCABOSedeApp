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

            var historico = _context.KpisHistoricos
               .OrderByDescending(h => h.Fecha)
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ToList();

            // Total de registros
            var totalRecords = _context.KpisHistoricos.Count();

            // Pasar datos a la vista
            ViewBag.Historico = historico;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            
            return View(historico);
        }
        [HttpPost]
        public IActionResult Filtrar([FromBody] FiltrarRequest request)
        {
            var query = _context.KpisHistoricos.AsQueryable();

            if (request.LineaId.HasValue)
            {
                query = query.Where(h => h.LineaId == request.LineaId.Value);
            }

            if (request.Desde.HasValue)
            {
                query = query.Where(h => h.Fecha >= request.Desde.Value);
            }

            if (request.Hasta.HasValue)
            {
                query = query.Where(h => h.Fecha <= request.Hasta.Value);
            }

            var resultados = query.OrderByDescending(h => h.Fecha).ToList();

            return Json(resultados);
            
        }
        [HttpPost]
        public IActionResult ExportarExcel([FromBody] FiltrarRequest request)
        {

            var query = _context.KpisHistoricos.AsQueryable();

            if (request.LineaId.HasValue)
            {
                query = query.Where(h => h.LineaId == request.LineaId.Value);
            }

            if (request.Desde.HasValue)
            {
                query = query.Where(h => h.Fecha >= request.Desde.Value);
            }

            if (request.Hasta.HasValue)
            {
                query = query.Where(h => h.Fecha <= request.Hasta.Value);
            }

            var resultados = query.OrderByDescending(h => h.Fecha).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("KPIs");

                // Encabezados
                worksheet.Cell(1, 1).Value = "Producto";
                worksheet.Cell(1, 2).Value = "Lote";
                worksheet.Cell(1, 3).Value = "Confección";
                worksheet.Cell(1, 4).Value = "N° Paquetes";
                worksheet.Cell(1, 5).Value = "N° Minutos";
                worksheet.Cell(1, 6).Value = "N° Operaciones";
                worksheet.Cell(1, 7).Value = "Peso Total";
                worksheet.Cell(1, 8).Value = "Target";
                worksheet.Cell(1, 9).Value = "KPI PPM";
                worksheet.Cell(1, 10).Value = "KPI PM";
                worksheet.Cell(1, 11).Value = "KPI Extrapeso";
                worksheet.Cell(1, 12).Value = "Fecha Registro";

                // Estilo a los encabezados
                var headerRange = worksheet.Range("A1:L1");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                int row = 2;
                foreach (var item in resultados)
                {
                    worksheet.Cell(row, 1).Value = item.SName;
                    worksheet.Cell(row, 2).Value = item.Lote;
                    worksheet.Cell(row, 3).Value = item.Confeccion;
                    worksheet.Cell(row, 4).Value = item.NPaquetes;
                    worksheet.Cell(row, 5).Value = item.NMinutos;
                    worksheet.Cell(row, 6).Value = item.NOperaciones;
                    worksheet.Cell(row, 7).Value = item.TotalWeight;
                    worksheet.Cell(row, 8).Value = item.FTarget;
                    worksheet.Cell(row, 9).Value = item.KpiPpm;
                    worksheet.Cell(row, 10).Value = item.KpiPm;
                    worksheet.Cell(row, 11).Value = item.KpiExtrapeso;
                    worksheet.Cell(row, 12).Value = item.Fecha?.ToString("dd/MM/yyyy");
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
    }
}


