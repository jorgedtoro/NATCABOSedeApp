
document.addEventListener("DOMContentLoaded", function () {
    const infoDiscriminadoraLink = document.getElementById("infoDiscriminadora-link");

    if (infoDiscriminadoraLink) {
        infoDiscriminadoraLink.addEventListener("click", function (event) {
            event.preventDefault(); // Evita la navegación por defecto

            fetch("/KPIS/InfoDiscriminadora/ObtenerLineasDisponibles")
                .then(response => response.json())
                .then(lineas => {
                    if (lineas.length > 0) {
                        lineas.forEach(linea => {
                            const url = `/KPIS/InfoDiscriminadora/Index?lineaSeleccionada=${linea}`;
                            window.open(url, `_blank`);
                        });
                    } else {
                        alert("No hay líneas disponibles.");
                    }
                })
                .catch(error => console.error("Error al obtener líneas:", error));
        });
    }
});
