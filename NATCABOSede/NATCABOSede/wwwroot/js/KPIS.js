
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
    //var obtenerLineasUrlAction = '@(Url.Action("ObtenerLineas", "KPIS", new { area = "KPIS/KPIS" }))';
    //var obtenerLineasUrlAction = '@Url.Action("ObtenerLineas", "KPIS", new { area = "KPIS/KPIS" })';
    //var obtenerLineasUrlAction = 'https://localhost:7114/KPIS/KPIS/ObtenerLineas';
    var obtenerLineasUrlAction = window.appSettings.obtenerLineasUrlAction;

    // Función para actualizar el dropdown de líneas activas
    function actualizarDropdownLineasActivas() {
        console.log('Fetching lines from:', obtenerLineasUrlAction);

        // Almacenamos el valor actual seleccionado para volver a asignarlo una vez se refresque el dropdown
        var selectedValue = lineaSeleccionada.value;

        fetch(obtenerLineasUrlAction)
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('Error al obtener las línas activas desde la bbdd');
                }
                return response.json();
            })
            .then(function (lineas) {
                // Eliminamos el contenido del dropdown
                lineaSeleccionada.innerHTML = '';

                if (lineas.length === 0) {
                    // No existen líneas activas...
                    var noLinesOption = document.createElement('option');
                    noLinesOption.textContent = 'No existen líneas activas';
                    noLinesOption.disabled = true;
                    noLinesOption.selected = true;
                    lineaSeleccionada.appendChild(noLinesOption);

                    // Eliminamos los kpis, ya que no hay nada que mostrar
                    actualizarKPIsConDatosVacios();
                    return;
                }

                // Populamos de nuevo el dropdown
                let foundSelectedValue = false;

                lineas.forEach(function (linea) {
                    var option = document.createElement('option');
                    option.value = linea.idLinea;
                    option.textContent = linea.nombreLinea;

                    // Mantenemos la lína seleccionada previamente, si es que sigue activa
                    if (linea.idLinea == selectedValue) {
                        option.selected = true;
                        foundSelectedValue = true;
                    }

                    lineaSeleccionada.appendChild(option);
                });

                // Si la línea seleccionada previamente ya no está activa, seleccionamos la siguiente en la lista
                if (!foundSelectedValue) {
                    lineaSeleccionada.selectedIndex = 0;
                }

                // Obtenemos los KPIs de la nueva línea seleccionada
                obtenerYActualizarKPIs(lineaSeleccionada);
            })
            .catch(function (error) {
                console.error('Error en la actualización del dropdown:', error);
            });
    }

    // Función para actualizar KPIs con datos vacíos
    function actualizarKPIsConDatosVacios() {
        document.getElementById('titulo-dashboard').innerText = 'Sin Producción actual en líneas';
        document.getElementById('contenido-dashboard').innerHTML = `
            <div class="text-center">
                <p>No KPIs disponibles</p>
            </div>`;
    }

    // Función para obtener y actualizar los KPIs
    function obtenerYActualizarKPIs(element) {
        var linea = element.value;

        if (!linea || linea === 'No existen líneas activas') {
            actualizarKPIsConDatosVacios();
            return;
        }

        var lineaText = element.options[element.selectedIndex]?.text || 'Línea desconocida';

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
                // Actualización del último timestamp
                //var now = new Date();
                //lastUpdatedElement.innerText = 'Última actualización: ' + now.toLocaleDateString() + ' ' + now.toLocaleTimeString();
            })
            .catch(function (error) {
                console.error('Error:', error);
            });
    }

    // Listener para el cambio de selección del dropdown
    lineaSeleccionada.addEventListener('change', function () {
        obtenerYActualizarKPIs(this);
    });

    // Llamada inicial al cargar la página
    actualizarDropdownLineasActivas();

    // Intervalo de llamada a la función de actualización , en milisegundos
    setInterval(function () {
        actualizarDropdownLineasActivas();
        // Actualización del último timestamp
        var now = new Date();
        lastUpdatedElement.innerText = 'Última actualización: ' + now.toLocaleDateString() + ' ' + now.toLocaleTimeString();
    }, 5000);
});


