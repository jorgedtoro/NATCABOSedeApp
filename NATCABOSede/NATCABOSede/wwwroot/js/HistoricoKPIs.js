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
        })
        .catch(error => console.error('Error:', error));
});

function actualizarGraficos(data) {
    const ctx1 = document.getElementById('graficoBarras1').getContext('2d');
    const ctx2 = document.getElementById('graficoBarras2').getContext('2d');

    const productos = data.map(item => item.Producto);
    const paquetes = data.map(item => item.N_Paquetes);
    const operaciones = data.map(item => item.N_Operaciones);

    new Chart(ctx1, {
        type: 'bar',
        data: {
            labels: productos,
            datasets: [{
                label: 'N° Paquetes',
                data: paquetes,
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1
            }]
        }
    });

    new Chart(ctx2, {
        type: 'bar',
        data: {
            labels: productos,
            datasets: [{
                label: 'N° Operaciones',
                data: operaciones,
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderColor: 'rgba(255, 99, 132, 1)',
                borderWidth: 1
            }]
        }
    });
}