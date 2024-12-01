
document.addEventListener('DOMContentLoaded', function () {
    var obtenerDatosUrlAction = window.appSettings.obtenerDatosUrlAction;
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

//para llamada automática cada 5min
//function actualizarDatos() {
//    fetch('/tu-endpoint')
//        .then(response => response.json())
//        .then(data => {
//            // Actualiza el contenido de la página con los nuevos datos
//            document.getElementById('contenido').innerText = data.valor;
//        })
//        .catch(error => console.error('Error al obtener los datos:', error));
//}

//// Llama a la función cada 5 minutos (300,000 milisegundos)
//setInterval(actualizarDatos, 300000);

//// Llama a la función inmediatamente al cargar la página
//actualizarDatos();