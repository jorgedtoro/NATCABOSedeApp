﻿// ----------------------------
// 1. Declaraciones de Variables
// ----------------------------
let datosFiltrados = []; // Global variable to store filtered data
let miChart = null;


let totalRecords = 0; // Total records for pagination
const pageSize = 25; // Number of rows per page
let currentPage = 1; // Current page
let totalPages = 0;

// ----------------------------
// 2. Funciones Utilitarias
// ----------------------------

/**
 * Muestra una alerta al usuario.
 * @param {string} mensaje - El mensaje a mostrar.
 */
function mostrarAlerta(mensaje) {
    alert(mensaje);
}
/**
 * Valida que la fecha 'hasta' sea posterior o igual a la fecha 'desde'.
 * @param {string} desde - Fecha 'desde' en formato ISO.
 * @param {string} hasta - Fecha 'hasta' en formato ISO.
 * @returns {boolean} - True si 'hasta' >= 'desde', de lo contrario, False.
 */

function validarFechas(desde, hasta) {
    const desdeDate = new Date(desde);
    const hastaDate = new Date(hasta);
    return hastaDate >= desdeDate;
}
/**
 * Formatea un número según las opciones especificadas.
 * @param {number} numero - Número a formatear.
 * @param {string} locale - Localización para el formateo (por defecto 'en-US').
 * @param {Object} opciones - Opciones de formateo.
 * @returns {string} - Número formateado.
 */
function formatearNumero(numero, locale = 'en-US', opciones = { minimumFractionDigits: 2, maximumFractionDigits: 2 }) {
    return new Intl.NumberFormat(locale, opciones).format(numero);
}


// ----------------------------
// 3. Funciones Principales
// ----------------------------

/**
 * Actualiza el contenido de la tabla con los datos proporcionados.
 * @param {Array} data - Array de objetos con los datos a mostrar en la tabla.
 */

function updateTable(data) {
    const tabla = document.getElementById("tabla-historico");
    let contenido = ''; // Variable para acumular el HTML

    if (!data || data.length === 0) {
        contenido = `
            <tr>
                <td colspan="12" class="text-center">Sin datos encontrados, realice un filtro nuevo.</td>
            </tr>
        `;
    } else {
        contenido = data.map(item => `
            <tr>
                <td>${item.sName || item.SName}</td>
                <td>${item.lote}</td>
                <td>${item.confeccion || item.Confeccion}</td>
                <td class="numeric">${item.nPaquetes}</td>
                <td class="numeric">${item.nMinutos}</td>
                <td class="numeric">${item.nOperarios}</td>
                <td class="numeric">${formatearNumero(item.totalWeight)}</td>
                <td class="numeric">${item.fTarget}</td>
                <td class="numeric">${formatearNumero(item.kpiPpm)}</td>
                <td class="numeric">${formatearNumero(item.kpiPm)}</td>
                <td class="numeric">${formatearNumero(item.kpiExtrapeso)}</td>
                <td class="numeric">${new Date(item.fecha).toLocaleDateString()}</td>
            </tr>
        `).join('');
    }

    tabla.innerHTML = contenido;
}

/**
 * Actualiza el gráfico con los datos proporcionados.
 * @param {Array} data - Array de objetos con los datos para el gráfico.
 */
function actualizarGraficos(data) {
    if (!data || data.length === 0) return;

    const kpiSeleccionado = document.getElementById("kpiSelect").value;
    const etiquetas = data.map(item => item.sName);
    const valores = data.map(item => {
        switch (kpiSeleccionado) {
            case "kpiPpm":
                return item.kpiPpm;
            case "kpiPm":
                return item.kpiPm;
            case "kpiExtrapeso":
                return item.kpiExtrapeso;
            default:
                return item.kpiPpm;
        }
    });

    if (miChart) miChart.destroy(); // Destroy the previous chart to avoid overlap

    const ctx = document.getElementById('graficoKPIs').getContext('2d');
    miChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: etiquetas,
            datasets: [{
                label: kpiSeleccionado.toUpperCase(),
                data: valores,
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                x: { title: { display: true, text: 'sName' } },
                y: { title: { display: true, text: kpiSeleccionado.toUpperCase() } }
            }
        }
    });
}

/**
 * Actualiza la paginación en la vista.
 */
function actualizarPaginacion() {
    const pagination = document.querySelector(".pagination");
    pagination.innerHTML = ''; // Limpiar la paginación existente

    // Botón "Anterior"
    if (currentPage > 1) {
        const prevLi = document.createElement('li');
        prevLi.classList.add('page-item');
        const prevLink = document.createElement('a');
        prevLink.classList.add('page-link');
        prevLink.href = '#';
        prevLink.textContent = 'Anterior';
        prevLink.addEventListener('click', function (e) {
            e.preventDefault();
            loadPage(currentPage - 1);
        });
        prevLi.appendChild(prevLink);
        pagination.appendChild(prevLi);
    }

    // Botones de página
    for (let i = 1; i <= totalPages; i++) {
        const pageLi = document.createElement('li');
        pageLi.classList.add('page-item');
        if (i === currentPage) {
            pageLi.classList.add('active');
        }

        const pageLink = document.createElement('a');
        pageLink.classList.add('page-link');
        pageLink.href = '#';
        pageLink.textContent = i;
        pageLink.addEventListener('click', function (e) {
            e.preventDefault();
            loadPage(i);
        });

        pageLi.appendChild(pageLink);
        pagination.appendChild(pageLi);
    }

    // Botón "Siguiente"
    if (currentPage < totalPages) {
        const nextLi = document.createElement('li');
        nextLi.classList.add('page-item');
        const nextLink = document.createElement('a');
        nextLink.classList.add('page-link');
        nextLink.href = '#';
        nextLink.textContent = 'Siguiente';
        nextLink.addEventListener('click', function (e) {
            e.preventDefault();
            loadPage(currentPage + 1);
        });
        nextLi.appendChild(nextLink);
        pagination.appendChild(nextLi);
    }
}
/**
 * Carga una página específica con los filtros aplicados.
 * @param {number} page - Número de página a cargar.
 */
function loadPage(page) {
    const lineaId = document.getElementById("lineaSeleccionada").value;
    const confeccion = document.getElementById("confeccionSeleccionada").value; // Opcional
    const desde = document.getElementById("desde").value;
    const hasta = document.getElementById("hasta").value;

    if (!lineaId || !desde || !hasta) {
        alert("Por favor, complete todos los campos del filtro antes de cambiar de página.");
        return;
    }
    // Validación: 'hasta' debe ser mayor o igual a 'desde'
    if (!validarFechas(desde, hasta)) {
        mostrarAlerta("La fecha 'Hasta' debe ser posterior o igual a la fecha 'Desde'.");
        return;
    }
    // Preparar los datos de la solicitud
    const requestData = {
        lineaId,
        confeccion: confeccion ? confeccion : null, // Incluir confección si está seleccionada
        desde: desde ? desde : null,
        hasta: hasta ? hasta : null,
        page: page,
        pageSize: pageSize
    };

    // Realizar la solicitud de filtrado para la página específica
    fetch('/KPIS/Historico/Filtrar', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(requestData)
    })
        .then(response => response.json())
        .then(data => {
            datosFiltrados = data.data;
            totalPages = data.totalPages;
            currentPage = page;

            updateTable(datosFiltrados); // Actualizar la tabla
            actualizarGraficos(datosFiltrados); // Actualizar el gráfico
            actualizarPaginacion(); // Actualizar la paginación
        })
        .catch(error => console.error('Error:', error));
}

// ----------------------------
// 4. Manejadores de Eventos
// ----------------------------

// Manejar el clic en el botón "Filtrar"
document.getElementById("btn-filtrar").addEventListener("click", function () {
    const lineaId = parseInt(document.getElementById("lineaSeleccionada").value, 10);
    const confeccion = document.getElementById("confeccionSeleccionada").value;          //JMB, es necesario filtrar también por Confección
    const desde = new Date(document.getElementById("desde").value).toISOString();
    const hasta = new Date(document.getElementById("hasta").value).toISOString();

    // Validación: Línea, Desde y Hasta son obligatorios
    if (!lineaId || !desde || !hasta) {
        mostrarAlerta("Por favor, complete los campos de Línea, Desde y Hasta.");
        return;
    }
    // Validación: 'hasta' debe ser mayor o igual a 'desde'
    if (!validarFechas(desde, hasta)) {
        mostrarAlerta("La fecha 'Hasta' debe ser posterior o igual a la fecha 'Desde'.");
        return;
    }
    // Preparar los datos de la solicitud
    const requestData = {
        lineaId,
        Confeccion: confeccion ? confeccion : null, // Incluir confección si está seleccionada
        desde: desde ? desde : null,
        hasta: hasta ? hasta : null,
        page: 1,
        pageSize: pageSize
    };

    fetch('/KPIS/Historico/Filtrar', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        // body: JSON.stringify({ lineaId, desde, hasta })
        body: JSON.stringify(requestData)
    })
        .then(response => response.json())
        .then(data => {
            datosFiltrados = data.data;
            totalPages = data.totalPages;
            currentPage = 1; //reseteamos a la primera página
            updateTable(datosFiltrados); // Actualiza tabla
            actualizarGraficos(data.data); // Actualiza gráfico
            actualizarPaginacion(); // Actualizar la paginación

            // Enable the "Exportar a Excel" button if there is data
            const exportBtn = document.getElementById("btn-export-excel");
            if (datosFiltrados && datosFiltrados.length > 0) {
                exportBtn.removeAttribute("disabled");
                exportBtn.setAttribute("title", "Exportar datos a Excel");
            } else {
                exportBtn.setAttribute("disabled", "true");
                exportBtn.setAttribute("title", "No hay datos para exportar, realice un filtro");
            }
        })
        .catch(error => console.error('Error:', error));
});

// Manejar el cambio en la selección de KPI
document.getElementById("kpiSelect").addEventListener("change", function () {
    actualizarGraficos(datosFiltrados);
});


// Manejar el clic en el botón "Exportar a Excel"
document.getElementById("btn-export-excel").addEventListener("click", function () {
    const lineaId = parseInt(document.getElementById("lineaSeleccionada").value, 10);
    const desde = new Date(document.getElementById("desde").value).toISOString();
    const hasta = new Date(document.getElementById("hasta").value).toISOString();

    if (!lineaId || !desde || !hasta) {
        alert("Por favor, para exportar datos complete todos los campos del filtro.");
        return;
    }

    fetch('/KPIS/Historico/ExportarExcel', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ lineaId, desde, hasta })
    })
        .then(response => response.blob())
        .then(blob => {
            const link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = "KpisHistorico.xlsx";
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        })
        .catch(error => console.error('Error:', error));
});


// ----------------------------
// 5. Carga Inicial de Dropdowns
// ----------------------------
/**
 * Carga opciones en un dropdown desde una URL específica.
 * @param {string} url - URL para obtener los datos.
 * @param {HTMLElement} selectElement - Elemento select donde se cargarán las opciones.
 * @param {string} placeholderText - Texto del placeholder.
 * @param {function} formatOption - Función para formatear cada opción.
 */
function cargarDropdown(url, selectElement, placeholderText, formatOption) {
    fetch(url)
        .then(response => {
            if (!response.ok) throw new Error(`Error fetching data from ${url}`);
            return response.json();
        })
        .then(items => {
            selectElement.innerHTML = ''; // Limpiar opciones existentes

            const placeholderOption = document.createElement('option');
            placeholderOption.value = '';
            placeholderOption.textContent = placeholderText;
            placeholderOption.disabled = true;
            placeholderOption.selected = true;
            selectElement.appendChild(placeholderOption);

            if (items.length === 0) {
                const noItemsOption = document.createElement('option');
                noItemsOption.textContent = `No existen ${placeholderText.toLowerCase()}`;
                noItemsOption.disabled = true;
                noItemsOption.selected = true;
                selectElement.appendChild(noItemsOption);
                return;
            }

            const uniqueItems = Array.from(new Set(items.map(item => JSON.stringify(item)))).map(item => JSON.parse(item));
            uniqueItems.sort((a, b) => {
                if (a.sName && b.sName) return a.sName.localeCompare(b.sName);
                if (a.confeccion && b.confeccion) return a.confeccion.localeCompare(b.confeccion);
                return 0;
            });

            uniqueItems.forEach(item => {
                const option = document.createElement('option');
                option.value = formatOption(item);
                option.textContent = formatOption(item, true);
                selectElement.appendChild(option);
            });
        })
        .catch(error => console.error(`Error cargando ${placeholderText.toLowerCase()}:`, error));
}
function cargarLineasHistorico() {
    cargarDropdown(
        window.appSettings.obtenerLineasHistoricoUrlAction,
        document.getElementById("lineaSeleccionada"),
        'Seleccione una línea',
        (item, isText = false) => isText ? item.sName : item.lineaId
    );
}

function cargarConfeccionesHistorico() {
    cargarDropdown(
        window.appSettings.obtenerConfeccionesHistoricoUrlAction,
        document.getElementById("confeccionSeleccionada"),
        'Seleccione una confección',
        (item) => item.confeccion
    );
}
// Cargar las líneas al cargar la página
cargarLineasHistorico();
// Cargar las confecciones al cargar la página (sin filtrar por línea)
cargarConfeccionesHistorico();

//document.addEventListener('DOMContentLoaded', function () {
//    const obtenerLineasHistoricoUrlAction = window.appSettings.obtenerLineasHistoricoUrlAction;
//    const obtenerConfeccionesHistoricoUrlAction = window.appSettings.obtenerConfeccionesHistoricoUrlAction;

//    const lineaSeleccionada = document.getElementById("lineaSeleccionada");
//    const confeccionSeleccionada = document.getElementById("confeccionSeleccionada");

//    /**
//     * Carga las líneas históricas desde el servidor y las agrega al dropdown.
//     */
//    function cargarLineasHistorico() {
//        fetch(obtenerLineasHistoricoUrlAction)
//            .then(response => {
//                if (!response.ok) throw new Error('Error fetching lineas historico');
//                return response.json();
//            })
//            .then(lineas => {
//                lineaSeleccionada.innerHTML = ''; // Clear existing options
//                const placeholderOption = document.createElement('option');
//                placeholderOption.value = '';
//                placeholderOption.textContent = 'Seleccione una línea';
//                placeholderOption.disabled = true;
//                placeholderOption.selected = true;
//                lineaSeleccionada.appendChild(placeholderOption);

//                if (lineas.length === 0) {
//                    const noLinesOption = document.createElement('option');
//                    noLinesOption.textContent = 'No existen líneas';
//                    noLinesOption.disabled = true;
//                    noLinesOption.selected = true;
//                    lineaSeleccionada.appendChild(noLinesOption);
//                    return;
//                }

//                const uniqueLineas = new Set();
//                lineas.forEach(linea => uniqueLineas.add(JSON.stringify(linea)));
//                const filteredLineas = Array.from(uniqueLineas).map(linea => JSON.parse(linea));
//                filteredLineas.sort((a, b) => a.sName.localeCompare(b.sName));

//                filteredLineas.forEach(linea => {
//                    const option = document.createElement('option');
//                    option.value = linea.lineaId;
//                    option.textContent = linea.sName;
//                    lineaSeleccionada.appendChild(option);
//                });
//            })
//            .catch(error => console.error('Error cargando líneas:', error));
//    }
//    /**
//     * Carga las confecciones históricas desde el servidor y las agrega al dropdown.
//     * @param {number} [lineaId=null] - Opcional. Filtra las confecciones por línea.
//     */
//    function cargarConfeccionesHistorico() {
//       // console.log('Cargando Confecciones disponibles para el histórico...')
//        fetch(obtenerConfeccionesHistoricoUrlAction)
//            .then(response => {
//                if (!response.ok) throw new Error('Error fetching confecciones historico');
//                return response.json();
//            })

//            .then(confecciones => {
//                confeccionSeleccionada.innerHTML = ''; // Clear existing options
//                const placeholderOption = document.createElement('option');
//                placeholderOption.value = '';
//                placeholderOption.textContent = 'Seleccione una confección';
//                placeholderOption.disabled = true;
//                placeholderOption.selected = true;
//                confeccionSeleccionada.appendChild(placeholderOption);
//                if (confecciones.length === 0) {
//                    const noConfeccionesOption = document.createElement('option');
//                    noConfeccionesOption.textContent = 'No existen confecciones';
//                    noConfeccionesOption.disabled = true;
//                    noConfeccionesOption.selected = true;
//                    confeccionSeleccionada.appendChild(noConfeccionesOption);
//                    return;
//                }

//                const uniqueConfecciones = new Set();
//                confecciones.forEach(confeccion => uniqueConfecciones.add(JSON.stringify(confeccion)));
//                const filteredConfecciones = Array.from(uniqueConfecciones).map(confeccion => JSON.parse(confeccion));
//                filteredConfecciones.sort((a, b) => a.confeccion.localeCompare(b.confeccion));


//                filteredConfecciones.forEach(confeccion => {
//                    const option = document.createElement('option');
//                    option.value = confeccion.confeccion;
//                    option.textContent = confeccion.confeccion;
//                    confeccionSeleccionada.appendChild(option);
//                });
//            })
//            .catch(error => console.error('Error cargando confecciones:', error));
//    }
//    // Cargar las líneas al cargar la página
//    cargarLineasHistorico();
//    // Cargar las confecciones al cargar la página (sin filtrar por línea)
//    cargarConfeccionesHistorico();
//});
