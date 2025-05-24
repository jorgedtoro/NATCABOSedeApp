document.addEventListener('DOMContentLoaded', function () {
    const linea = document.getElementById('lineaSeleccionadaHidden')?.value;
    const contenedor = document.getElementById('contenido-discriminadora-completo');
    var lastUpdatedElement = document.getElementById('last-updated');

    function refrescarVistaCompleta() {
        if (!linea || !contenedor) return;

        fetch(`/KPIS/InfoDiscriminadora/Index?lineaSeleccionada=${linea}`)
            .then((response) => {
                if (!response.ok) throw new Error('Error al obtener la vista actualizada');
                return response.text();
            })
            .then((html) => {
                const tempDiv = document.createElement('div');
                tempDiv.innerHTML = html;
                console.log('Se actualizan las vistas de InfoDis');
                const nuevoContenido = tempDiv.querySelector('#contenido-discriminadora-completo');
                if (nuevoContenido) {
                    contenedor.innerHTML = nuevoContenido.innerHTML;
                }
            })
            .catch((error) => {
                console.error('Error actualizando la vista:', error);
            });
    }

    // Refresco cada 2 minutos
    //setInterval(refrescarVistaCompleta, 120000);
    setInterval(function () {
        refrescarVistaCompleta();
        /* Actualización del último timestamp*/
        var now = new Date();
        lastUpdatedElement.innerText =
            'Última actualización: ' + now.toLocaleDateString() + ' ' + now.toLocaleTimeString();
    }, 120000);
});
