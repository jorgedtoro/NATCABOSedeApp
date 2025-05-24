const overlayManager = (() => {
    const overlayId = 'global-overlay'; // ID of the overlay element

    // Function to create the overlay if it doesn't exist
    function createOverlay() {
        if (!document.getElementById(overlayId)) {
            const overlay = document.createElement('div');
            overlay.id = overlayId;
            overlay.className = 'loading-overlay'; // Apply the CSS class
            overlay.style.display = 'none'; // Initially hidden

            // Add the spinner content
            overlay.innerHTML = `
                <div class="spinner-border text-primary" role="status">
                    <span class="sr-only">Loading...</span>
                </div>
            `;

            document.body.appendChild(overlay);
        }
    }

    // Function to show the overlay
    function showOverlay() {
        const overlay = document.getElementById(overlayId);
        if (overlay) {
            overlay.style.display = 'flex'; // Show the overlay
        }
    }

    // Function to hide the overlay
    function hideOverlay() {
        const overlay = document.getElementById(overlayId);
        if (overlay) {
            overlay.style.display = 'none'; // Hide the overlay
        }
    }

    // Initialize overlay on load
    createOverlay();
    document.addEventListener('DOMContentLoaded', function () {
        const historicoLink = document.getElementById('historico-link');
        const kpisLiveLink = document.getElementById('kpisLive-link');

        if (historicoLink) {
            historicoLink.addEventListener('click', function () {
                overlayManager.show(); // Muestra el overlay antes de navegar
            });
        }
        if (kpisLiveLink) {
            kpisLiveLink.addEventListener('click', function () {
                overlayManager.show(); // Muestra el overlay antes de navegar
            });
        }
    });

    // Ocultar overlay cuando la página ha cargado completamente
    window.onload = function () {
        overlayManager.hide();
    };

    // Expose public methods
    return {
        show: showOverlay,
        hide: hideOverlay,
    };
})();
