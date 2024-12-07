using Microsoft.AspNetCore.Mvc;
using System;
using NATCABOSede.Areas.KPIS.Models;
using NATCABOSede.Models;
using NATCABOSede.Interfaces;

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
               .OrderByDescending(h => h.Fecha) // Ordenar por fecha descendente
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ToList();

            // Total de registros
            var totalRecords = _context.KpisHistoricos.Count();

            // Pasar datos a la vista
            //ViewBag.Historico = historico;
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
    }
}
