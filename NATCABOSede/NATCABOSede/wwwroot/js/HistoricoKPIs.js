
let datosFiltrados = []; // Variable global, guarda los datos filtrados
let miChart = null;
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
            // Actualizar la tabla
            const tabla = document.getElementById("tabla-historico");
            tabla.innerHTML = data.map(item => `
                    <tr>
                        <td>${item.sName}</td>
                        <td>${item.lote}</td>
                        <td>${item.confeccion}</td>
                        <td>${item.nPaquetes}</td>
                        <td>${item.nMinutos}</td>
                        <td>${item.nOperaciones}</td>
                        <td>${item.totalWeight}</td>
                        <td>${item.fTarget}</td>
                        <td>${item.kpiPpm}</td>
                        <td>${item.kpiPm}</td>
                        <td>${item.kpiExtrapeso}</td>
                        <td>${new Date(item.fecha).toLocaleDateString()}</td>
                    </tr>
                `).join('');
            //Actualizar gráfico
            actualizarGraficos(data);

            // Habilita el botón de "Exportar a Excel" si hay datos
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
document.getElementById("kpiSelect").addEventListener("change", function () {
    actualizarGraficos(datosFiltrados);
});

function actualizarGraficos(data) {
    if (!data || data.length === 0) {
        return;
    }

    
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

    // Destruir el gráfico anterior si existe (evita superposiciones)
    if (miChart) {
        miChart.destroy();
    }

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
                x: {
                    title: {
                        display: true,
                        text: 'sName'
                    }
                },
                y: {
                    title: {
                        display: true,
                        text: kpiSeleccionado.toUpperCase()
                    }
                }
            }
        }
    });
}
//Exportar a Excel los datos filtrados de la tabla.
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

