
//document.addEventListener('DOMContentLoaded', function () {
//    var obtenerDatosUrlAction = window.appSettings.obtenerDatosUrlAction;
//    var lineaSeleccionada = document.getElementById('lineaSeleccionada');

//    lineaSeleccionada.addEventListener('change', function () {
//        var linea = this.value;

//        fetch(obtenerDatosUrlAction + '?lineaSeleccionada=' + linea)
//            .then(function (response) {
//                if (!response.ok) {
//                    throw new Error('Error en la respuesta del servidor');
//                }
//                return response.text();
//            })
//            .then(function (html) {
//                // Actualiza el título
//                document.getElementById('titulo-dashboard').innerText = 'Línea ' + linea + ' - KPIs Dashboard';
//                // Actualiza el contenido
//                document.getElementById('contenido-dashboard').innerHTML = html;
//            })
//            .catch(function (error) {
//                console.error('Error:', error);
//            });
//    });
//});

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

document.addEventListener('DOMContentLoaded', function () {
    var obtenerDatosUrlAction = window.appSettings.obtenerDatosUrlAction;
    var lineaSeleccionada = document.getElementById('lineaSeleccionada');
    var lastUpdatedElement = document.getElementById('last-updated'); // Etiqueta última actualzación de kpis


    // Function to fetch and update KPIs
    function obtenerYActualizarKPIs(element) {
        var linea = element.value;
        var lineaText = element.options[element.selectedIndex].text; // Obtención del nombre de la línea tras la selección en el dropdown

        fetch(obtenerDatosUrlAction + '?lineaSeleccionada=' + linea)
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('Error en la respuesta del servidor');
                }
                return response.text();
            })
            .then(function (html) {
                // Actualización del título con el nombre de línea seleccionado
                document.getElementById('titulo-dashboard').innerText = lineaText + ' - KPIs Dashboard';
                // Actualización del contenido del dashboard
                document.getElementById('contenido-dashboard').innerHTML = html;
                // Actualízación del último timestamp
                var now = new Date();
                lastUpdatedElement.innerText = 'Última actualización: ' + now.toLocaleDateString() + ' ' + now.toLocaleTimeString();

            })
            .catch(function (error) {
                console.error('Error:', error);
            });
    }

    // Add event listener for dropdown change
    lineaSeleccionada.addEventListener('change', function () {
        obtenerYActualizarKPIs(this);
    });

    // Immediate call on page load
    obtenerYActualizarKPIs(lineaSeleccionada);

    // Interval call in milliseconds
    setInterval(function () {
        obtenerYActualizarKPIs(lineaSeleccionada);
    }, 60000);
});

