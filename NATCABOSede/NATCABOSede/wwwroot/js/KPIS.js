

document.addEventListener('DOMContentLoaded', function () {
    var lineaSeleccionada = document.getElementById('lineaSeleccionada');

    lineaSeleccionada.addEventListener('change', function () {
        var linea = this.value;

        fetch(obtenerDatosUrlAction + '?lineaSeleccionada=' + linea)
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('Error en la respuesta del servidor');
                }
                return response.text();
            })
            .then(function (html) {
                // Actualiza el título
                document.getElementById('titulo-dashboard').innerText = 'Línea ' + linea + ' - KPIs Dashboard';
                // Actualiza el contenido
                document.getElementById('contenido-dashboard').innerHTML = html;
            })
            .catch(function (error) {
                console.error('Error:', error);
            });
    });
});
