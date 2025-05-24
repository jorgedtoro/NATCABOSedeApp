/**
 * Módulo principal para la gestión de la interfaz de KPIs.
 * Maneja la carga y actualización de datos de KPIs para las líneas de producción.
 * @module KPIS
 */

// Asegurarse de que el módulo de notificaciones esté cargado
const hasNotifications = !!window.Notifications;

document.addEventListener('DOMContentLoaded', function () {
    const obtenerDatosUrlAction = window.appSettings.obtenerDatosUrlAction;
    const obtenerLineasUrlAction = window.appSettings.obtenerLineasUrlAction;

    const lineaSeleccionada = document.getElementById('lineaSeleccionada');
    const lastUpdatedElement = document.getElementById('last-updated');
    
    if (!lineaSeleccionada) {
        if (hasNotifications) {
            window.Notifications.showError('No se pudo inicializar el selector de líneas');
        }
        return;
    }

    /**
     * Actualiza el dropdown de líneas activas obteniendo los datos del servidor.
     * @async
     * @function actualizarDropdownLineasActivas
     * @returns {Promise<void>}
     */

    // Configuración del overlay
    if (!window.overlayManager && hasNotifications) {
        window.Notifications.showWarning('El gestor de overlay no está disponible');
    }
    //// Show loading overlay
    //function showLoading() {
    //    document.getElementById('loading-overlay').style.display = 'flex';
    //}

    //// Hide loading overlay
    //function hideLoading() {
    //    document.getElementById('loading-overlay').style.display = 'none';
    //}
    ////***************/

    /**
     * Actualiza el dropdown de líneas activas obteniendo los datos del servidor.
     * @async
     * @function actualizarDropdownLineasActivas
     * @returns {Promise<void>}
     */
    function actualizarDropdownLineasActivas() {
        // Almacenamos el valor actual seleccionado para volver a asignarlo una vez se refresque el dropdown
        var selectedValue = lineaSeleccionada.value;

        fetch(obtenerLineasUrlAction)
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('Error al obtener las líneas activas desde la bbdd');
                }
                return response.json();
            })
            .then(function (lineas) {
                if ('success' in lineas && !lineas.success) {
                    // Usar el mensaje de error para notificación al usuario
                    const errorMessage = lineas.message || 'Error desconocido al cargar las líneas';
                    if (hasNotifications) {
                        window.Notifications.showError(errorMessage);
                    }
                    return;
                }
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
                const errorMsg = 'Error al actualizar la lista de líneas: ' + (error.message || 'Error desconocido');
                if (hasNotifications) {
                    window.Notifications.showError(errorMsg);
                }
                // Redirigir a la página de error con el mensaje correspondiente
                window.location.href = '/Home/Error?message=' + encodeURIComponent(errorMsg);
            });
    }

    // Función para actualizar KPIs con datos vacíos
    /**
     * Muestra un mensaje cuando no hay datos de KPIs disponibles.
     * @function actualizarKPIsConDatosVacios
     */
    function actualizarKPIsConDatosVacios() {
        document.getElementById('titulo-dashboard').innerText = 'Sin Producción actual en líneas';
        document.getElementById('contenido-dashboard').innerHTML = `
            <div class="text-center">
                <p>No KPIs disponibles</p>
            </div>`;
    }

    /**
     * Obtiene y actualiza los KPIs para la línea de producción seleccionada.
     * @async
     * @function obtenerYActualizarKPIs
     * @param {HTMLSelectElement} selectElement - Elemento select que contiene la línea seleccionada.
     * @returns {Promise<void>}
     */
    function obtenerYActualizarKPIs(selectElement) {
        var linea = selectElement.value;

        if (!linea || linea === 'No existen líneas activas') {
            actualizarKPIsConDatosVacios();
            return;
        }

        const lineaText = selectElement.options[selectElement.selectedIndex]?.text || 'Línea desconocida';

        // Verificar si overlayManager está disponible
        if (window.overlayManager) {
            window.overlayManager.show(); // Show overlay before the request
        }

        fetch(obtenerDatosUrlAction + '?lineaSeleccionada=' + linea)
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('Error en la respuesta del servidor');
                }
                return response.text();
            })
            .then(function (html) {
                // Actualización del título con el nombre de línea seleccionado
                document.getElementById('titulo-dashboard').innerText =
                    lineaText + ' - KPIs Dashboard';
                // Actualización del contenido del dashboard
                document.getElementById('contenido-dashboard').innerHTML = html;
                // Actualización del último timestamp
                //var now = new Date();
                //lastUpdatedElement.innerText = 'Última actualización: ' + now.toLocaleDateString() + ' ' + now.toLocaleTimeString();
            })
            .catch(function (fetchError) {
                // Mostrar error al usuario
                const errorMsg = 'Error al cargar los KPIs: ' + (fetchError.message || 'Error desconocido');
                if (hasNotifications) {
                    window.Notifications.showError(errorMsg);
                }
                // Ocultar overlay si está disponible
                if (window.overlayManager) {
                    window.overlayManager.hide();
                }
            })
            .finally(function () {
                // Ocultar overlay si está disponible
                if (window.overlayManager) {
                    window.overlayManager.hide();
                }
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
        lastUpdatedElement.innerText =
            'Última actualización: ' + now.toLocaleDateString() + ' ' + now.toLocaleTimeString();
    }, 120000);
});
