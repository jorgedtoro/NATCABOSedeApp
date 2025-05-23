﻿using Microsoft.AspNetCore.Mvc;
using NATCABOSede.Models;
using System.Diagnostics;

namespace NATCABOSede.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //ViewBag.ErrorMessage = TempData["ErrorMessage"] ?? "An unexpected error occurred."; //JMB

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
