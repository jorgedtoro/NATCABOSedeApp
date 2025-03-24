using Microsoft.AspNetCore.Mvc;
using NATCABOSede.Models;
using NATCABOSede.ViewModels;
using System.Linq;
using System.Collections.Generic;

namespace NATCABOSede.Areas.KPIS.Controllers
{
    [Area("KPIS")]
    public class ConfigurarObjetivosController : Controller
    {
        private readonly NATCABOContext _context;

        public ConfigurarObjetivosController(NATCABOContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ObjetivosConfigurables()
        {
        
            var confeccionesBD = _context.VwObjetivosConfecciones.ToList();

         
            var vm = new ConfigurarObjetivosViewModel
            {
                Confecciones = confeccionesBD.Select(c => new ObjetivosConfeccionVM
                {
                    SCode = c.SCode,
                    SName = c.SName,
                    PercExtraOper = c.PercExtraOper,
                    PpmObj = c.PpmObj,
                    ModObj = c.ModObj,
                    FttObj = c.FttObj
                }).ToList()
            };
            //ModelState.Clear();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ObjetivosConfigurables(ConfigurarObjetivosViewModel model)
        {
            // Al hacer POST, recibimos la lista entera de confecciones con los valores editados
            if (model == null || model.Confecciones == null)
            {
                TempData["Error"] = "No se recibieron datos.";
                return RedirectToAction(nameof(ObjetivosConfigurables));
            }

            try
            {
                foreach (var item in model.Confecciones)
                {
                    // Buscar en T_ObjetivosConfecciones (tu tabla real) según SCode
                    var registro = _context.TObjetivosConfecciones
                        .FirstOrDefault(r => r.SCode == item.SCode);

                    if (registro == null)
                    {
                        // Manejar caso: si no existe, podríamos crearlo o ignorarlo
                        // Por simplicidad, ignoremos
                        continue;
                    }

                    // Asignar nuevas propiedades
                    registro.PercExtraOper = item.PercExtraOper;
                    registro.PpmObj = item.PpmObj;
                    registro.ModObj = item.ModObj;
                    registro.FttObj = item.FttObj;
                }

                _context.SaveChanges(); // Guardar todos los cambios de una vez

                TempData["Success"] = "Objetivos guardados correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al guardar objetivos: " + ex.Message;
            }

            return RedirectToAction(nameof(ObjetivosConfigurables));
        }
    }
}
