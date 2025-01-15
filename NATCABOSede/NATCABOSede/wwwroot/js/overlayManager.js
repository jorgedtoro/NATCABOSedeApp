const overlayManager = (() => {
    const overlayId = 'global-overlay'; // ID of the overlay element

    // Function to create the overlay if it doesn't exist
    function createOverlay() {
        if (!document.getElementById(overlayId)) {
            const overlay = document.createElement('div');
            overlay.id = overlayId;
            overlay.textContent = 'Loading...'; // Add loading text or spinner
            document.body.appendChild(overlay);
        }
    }

    // Function to show the overlay
    function showOverlay() {
        const overlay = document.getElementById(overlayId);
        if (overlay) {
            overlay.style.display = 'flex';
        }
    }

    // Function to hide the overlay
    function hideOverlay() {
        const overlay = document.getElementById(overlayId);
        if (overlay) {
            overlay.style.display = 'none';
        }
    }

    // Initialize overlay on load
    createOverlay();

    // Expose public methods
    return {
        show: showOverlay,
        hide: hideOverlay,
    };
})();
