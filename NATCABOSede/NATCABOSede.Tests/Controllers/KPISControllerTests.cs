using Moq;
using Xunit;
using NATCABOSede.Areas.KPIS.Controllers;
using NATCABOSede.Services;
using NATCABOSede.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NATCABOSede.Interfaces;

namespace NATCABOSede.Tests.Controllers
{
    public class KPISControllerTests
    {
        [Fact]
        public void ObtenerKPIs_LineaExistente_RetornaPartialViewConModelo()
        {
            // Arrange
            short lineaSeleccionada = 1;

            var mockContext = new Mock<NATCABOContext>();
            var mockService = new Mock<IKPIService>();
            double mediaPaquetesPorMinuto = 2.0;
            // Configurar el contexto simulado
            var datosKpi = new DatosKpi
            {
                IdLinea = lineaSeleccionada,
                MinutosTrabajados = 50,
                NumeroOperadores = 5,
                PesoTotalReal = 0.2,
                PesoObjetivo = 0.1,
                HoraInicioProduccion = DateTime.Now.AddHours(-1),
                PaquetesTotales = 200,
                PaquetesValidos = 100,
                CosteHora = 15,
               
                
            };

            var datosKpis = new List<DatosKpi> { datosKpi }.AsQueryable();

            var mockSet = new Mock<DbSet<DatosKpi>>();
            mockSet.As<IQueryable<DatosKpi>>().Setup(m => m.Provider).Returns(datosKpis.Provider);
            mockSet.As<IQueryable<DatosKpi>>().Setup(m => m.Expression).Returns(datosKpis.Expression);
            mockSet.As<IQueryable<DatosKpi>>().Setup(m => m.ElementType).Returns(datosKpis.ElementType);
            mockSet.As<IQueryable<DatosKpi>>().Setup(m => m.GetEnumerator()).Returns(datosKpis.GetEnumerator());

            //mockContext.Setup(c => c.DatosKpisLives).Returns(mockSet.Object);

            // Configurar el servicio simulado
            mockService.Setup(s => s.CalcularPPM(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>())).Returns(0.4);
            mockService.Setup(s => s.CalcularPM(It.IsAny<double>(), It.IsAny<double>())).Returns(2.0);
            //mockService.Setup(s => s.CalcularExtrapeso(It.IsAny<double>(), It.IsAny<double>(),It.IsAny<int>)).Returns(0.1);
            mockService.Setup(s => s.CalcularHoraFin(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<double>())).Returns(DateTime.Now);
            mockService.Setup(s => s.CalcularPorcentajePedido(It.IsAny<int>(), It.IsAny<int>())).Returns(50);
            mockService.Setup(s => s.CalcularCosteMOD(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<double>())).Returns(1.5);

            var controller = new KPISController(mockContext.Object, mockService.Object);

            // Act
            var result = controller.ObtenerKPIs(lineaSeleccionada);

            // Assert
            var viewResult = Assert.IsType<PartialViewResult>(result);
            Assert.Equal("_KPIsPartial", viewResult.ViewName);
            Assert.NotNull(viewResult.Model);
        }
    }
}
