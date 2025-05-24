// home.js

document.addEventListener('DOMContentLoaded', function () {
    // Enlace Envasadoras
    const linkEnvasadoras = document.getElementById('infoEnvasadoras-linkBis');
    if (linkEnvasadoras) {
        linkEnvasadoras.addEventListener('click', function (event) {
            event.preventDefault();

            fetch('/KPIS/InfoEnvasadoras/ObtenerDiscriminadoras')
                .then((response) => response.json())
                .then((discriminadoras) => {
                    if (discriminadoras.length > 0) {
                        discriminadoras.forEach((d) => {
                            const url = `/KPIS/InfoEnvasadoras/Index?discriminadora=${d}`;
                            window.open(url, `_blank`);
                        });
                    } else {
                        alert('No hay discriminadoras activas.');
                    }
                })
                .catch((error) => console.error('Error al obtener discriminadoras:', error));
        });
    }

    // Enlace Discriminadora
    const linkDiscriminadora = document.getElementById('infoDiscriminadora-linkBis');
    if (linkDiscriminadora) {
        linkDiscriminadora.addEventListener('click', function (event) {
            event.preventDefault();

            fetch('/KPIS/InfoDiscriminadora/ObtenerLineasDisponibles')
                .then((response) => response.json())
                .then((lineas) => {
                    if (lineas.length > 0) {
                        lineas.forEach((linea) => {
                            const url = `/KPIS/InfoDiscriminadora/Index?lineaSeleccionada=${linea}`;
                            window.open(url, `_blank`);
                        });
                    } else {
                        alert('No hay líneas disponibles.');
                    }
                })
                .catch((error) => console.error('Error al obtener líneas:', error));
        });
    }
});
