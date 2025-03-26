document.addEventListener("DOMContentLoaded", function () {
    // valor "discriminadora" del un input hidden de la vista
    const discriminadora = document.getElementById("discriminadoraHidden")?.value;

    // contenedor principal de la vista
    const contenedor = document.getElementById("contenido-envasadora-completo");

    //timestamp de “última actualización”
    const lastUpdatedElement = document.getElementById("last-updated");

    function refrescarVistaCompleta() {
        if (!discriminadora || !contenedor) return;

        
        fetch(`/KPIS/InfoEnvasadoras/Index?discriminadora=${discriminadora}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error("Error al obtener la vista actualizada");
                }
                return response.text();
            })
            .then(html => {
                // contenedor temporal para parsear el HTML recibido
                const tempDiv = document.createElement("div");
                tempDiv.innerHTML = html;
                console.log("Se actualiza la vista de InfoEnvasadoras");

                // Extraemos el nuevo contenido desde un elemento con el mismo ID
                // (ej. #contenido-envasadora-completo) y reemplazamos el viejo
                const nuevoContenido = tempDiv.querySelector("#contenido-envasadora-completo");
                if (nuevoContenido) {
                    contenedor.innerHTML = nuevoContenido.innerHTML;
                }
            })
            .catch(error => {
                console.error("Error actualizando la vista:", error);
            });
    }

    // refresco automático cada 2 minutos --> (120000 ms)
    setInterval(function () {
        refrescarVistaCompleta();

        if (lastUpdatedElement) {
            const now = new Date();
            lastUpdatedElement.innerText = 'Última actualización: ' +
                now.toLocaleDateString() + ' ' + now.toLocaleTimeString();
        }
    }, 120000);
});
