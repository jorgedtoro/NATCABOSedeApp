document.addEventListener("DOMContentLoaded", function () {
    const link = document.getElementById("infoEnvasadoras-link");

    if (link) {
        link.addEventListener("click", function (event) {
            event.preventDefault();

            fetch("/KPIS/InfoEnvasadoras/ObtenerDiscriminadoras")
                .then(response => response.json())
                .then(discriminadoras => {
                    if (discriminadoras.length > 0) {
                        discriminadoras.forEach(d => {
                            const url = `/KPIS/InfoEnvasadoras/Index?discriminadora=${d}`;
                            window.open(url, `_blank`);
                        });
                    } else {
                        alert("No hay discriminadoras activas.");
                    }
                })
                .catch(error => console.error("Error al obtener discriminadoras:", error));
        });
    }
});
