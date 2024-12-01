using NATCABOSede.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NATCABOSede.Tests.Services
{
    public class KPIServiceTests
    {
        [Fact]
        public void CalcularPPM_ValoresValidos_RetornaResultadoCorrecto()
        {
            // Arrange
            var service = new KPIService();
            double paquetesTotales = 100;
            double minutosTrabajados = 50;
            int numeroPersonas = 5;

            // Act
            double resultado = service.CalcularPPM(paquetesTotales, minutosTrabajados, numeroPersonas);

            // Assert
            Assert.Equal(0.4, resultado);
        }
        [Fact]
        public void CalcularPM_ValoresValidos_RetornaResultadoCorrecto()
        {
            // Arrange
            var service = new KPIService();
            double paquetesTotales = 100;
            double minutosTrabajados = 50;

            // Act
            double resultado = service.CalcularPM(paquetesTotales, minutosTrabajados);

            // Assert
            Assert.Equal(2, resultado);
        }
    }
}
