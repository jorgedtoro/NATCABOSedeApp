let datosFiltrados = []; // Global variable to store filtered data
let miChart = null;


let totalRecords = 0; // Total records for pagination
const pageSize = 10; // Number of rows per page
let currentPage = 1; // Current page


// Handle the "Filtrar" button click
document.getElementById("btn-filtrar").addEventListener("click", function () {
    const lineaId = parseInt(document.getElementById("lineaSeleccionada").value, 10);
    const desde = new Date(document.getElementById("desde").value).toISOString();
    const hasta = new Date(document.getElementById("hasta").value).toISOString();

    if (!lineaId || !desde || !hasta) {
        alert("Por favor, complete todos los campos del filtro.");
        return;
    }

    fetch('/KPIS/Historico/Filtrar', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ lineaId, desde, hasta })
    })
        .then(response => response.json())
        .then(data => {
            datosFiltrados = data;
            updateTable(data); // Update the table
            actualizarGraficos(data); // Update the chart

            // Enable the "Exportar a Excel" button if there is data
            const exportBtn = document.getElementById("btn-export-excel");
            if (data && data.length > 0) {
                exportBtn.removeAttribute("disabled");
                exportBtn.setAttribute("title", "Exportar datos a Excel");
            } else {
                exportBtn.setAttribute("disabled", "true");
                exportBtn.setAttribute("title", "No hay datos para exportar, realice un filtro");
            }
        })
        .catch(error => console.error('Error:', error));
});

// Handle KPI selection change
document.getElementById("kpiSelect").addEventListener("change", function () {
    actualizarGraficos(datosFiltrados);
});

// Function to update the table
function updateTable(data) {
    const tabla = document.getElementById("tabla-historico");
    tabla.innerHTML = data.map(item => `
        <tr>
            <td>${item.sName}</td>
            <td>${item.lote}</td>
            <td>${item.confeccion}</td>
            <td class="numeric">${item.nPaquetes}</td>
            <td class="numeric">${item.nMinutos}</td>
            <td class="numeric">${item.nOperarios}</td>
            <td class="numeric">${new Intl.NumberFormat('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }).format(item.totalWeight)}</td>
            <td class="numeric">${item.fTarget}</td>
            <td class="numeric">${new Intl.NumberFormat('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }).format(item.kpiPpm)}</td>
            <td class="numeric">${new Intl.NumberFormat('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }).format(item.kpiPm)}</td>
            <td class="numeric">${new Intl.NumberFormat('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }).format(item.kpiExtrapeso)}</td>
            <td class="numeric">${new Date(item.fecha).toLocaleDateString()}</td>
        </tr>
    `).join('');
}

// Function to update charts
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

// Handle "Exportar a Excel" button click
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

// Function to load a specific page with filters applied
function loadPage(page) {
    const lineaId = document.getElementById("lineaSeleccionada").value;
    const desde = document.getElementById("desde").value;
    const hasta = document.getElementById("hasta").value;

    if (!lineaId || !desde || !hasta) {
        alert("Por favor, complete todos los campos del filtro antes de cambiar de página.");
        return;
    }

    fetch(`/KPIS/Historico/Filtrar?page=${page}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ lineaId, desde, hasta })
    })
        .then(response => response.json())
        .then(data => {
            datosFiltrados = data;
            updateTable(data); // Update the table with the new page's data
        })
        .catch(error => console.error('Error:', error));
}

// Populate dropdown for line selection
document.addEventListener('DOMContentLoaded', function () {
    const obtenerLineasHistoricoUrlAction = window.appSettings.obtenerLineasHistoricoUrlAction;
    const lineaSeleccionada = document.getElementById("lineaSeleccionada");

    function cargarLineasHistorico() {
        fetch(obtenerLineasHistoricoUrlAction)
            .then(response => {
                if (!response.ok) throw new Error('Error fetching lineas historico');
                return response.json();
            })
            .then(lineas => {
                lineaSeleccionada.innerHTML = ''; // Clear existing options
                const placeholderOption = document.createElement('option');
                placeholderOption.value = '';
                placeholderOption.textContent = 'Seleccione una línea';
                placeholderOption.disabled = true;
                placeholderOption.selected = true;
                lineaSeleccionada.appendChild(placeholderOption);

                if (lineas.length === 0) {
                    const noLinesOption = document.createElement('option');
                    noLinesOption.textContent = 'No lines available';
                    noLinesOption.disabled = true;
                    noLinesOption.selected = true;
                    lineaSeleccionada.appendChild(noLinesOption);
                    return;
                }

                const uniqueLineas = new Set();
                lineas.forEach(linea => uniqueLineas.add(JSON.stringify(linea)));
                const filteredLineas = Array.from(uniqueLineas).map(linea => JSON.parse(linea));
                filteredLineas.sort((a, b) => a.sName.localeCompare(b.sName));

                filteredLineas.forEach(linea => {
                    const option = document.createElement('option');
                    option.value = linea.lineaId;
                    option.textContent = linea.sName;
                    lineaSeleccionada.appendChild(option);
                });
            })
            .catch(error => console.error('Error loading lines:', error));
    }

    cargarLineasHistorico(); // Populate the dropdown when the page loads
});
