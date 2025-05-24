/**
 * Módulo para manejar notificaciones en la interfaz de usuario.
 * @module Notifications
 */

const Notifications = (function() {
    'use strict';

    /**
     * Muestra un mensaje de error al usuario.
     * @param {string} message - Mensaje de error a mostrar.
     * @param {number} [duration=5000] - Duración en milisegundos que se mostrará el mensaje.
     */
    function showError(message, duration = 5000) {
        showMessage(message, 'error', duration);
    }

    /**
     * Muestra un mensaje de éxito al usuario.
     * @param {string} message - Mensaje de éxito a mostrar.
     * @param {number} [duration=3000] - Duración en milisegundos que se mostrará el mensaje.
     */
    function showSuccess(message, duration = 3000) {
        showMessage(message, 'success', duration);
    }

    /**
     * Muestra un mensaje informativo al usuario.
     * @param {string} message - Mensaje a mostrar.
     * @param {string} type - Tipo de mensaje ('error', 'success', 'info', 'warning').
     * @param {number} duration - Duración en milisegundos.
     * @private
     */
    function showMessage(message, type, duration) {
        // Crear elemento de notificación
        const notification = document.createElement('div');
        notification.className = `notification ${type}`;
        notification.textContent = message;
        
        // Estilos básicos para la notificación
        Object.assign(notification.style, {
            position: 'fixed',
            top: '20px',
            right: '20px',
            padding: '15px 20px',
            borderRadius: '4px',
            color: 'white',
            zIndex: '1000',
            boxShadow: '0 2px 10px rgba(0,0,0,0.1)',
            opacity: '0',
            transition: 'opacity 0.3s ease-in-out'
        });
        
        // Establecer colores según el tipo
        const colors = {
            error: '#f44336',    // Rojo
            success: '#4CAF50', // Verde
            warning: '#ff9800', // Naranja
            info: '#2196F3'     // Azul
        };
        notification.style.backgroundColor = colors[type] || colors.info;
        
        // Añadir al DOM
        document.body.appendChild(notification);
        
        // Forzar reflow
        void notification.offsetWidth;
        
        // Mostrar con animación
        notification.style.opacity = '1';
        
        // Ocultar después de la duración especificada
        setTimeout(() => {
            notification.style.opacity = '0';
            // Eliminar después de la animación
            setTimeout(() => {
                if (notification.parentNode) {
                    notification.parentNode.removeChild(notification);
                }
            }, 300);
        }, duration);
        
        // Permitir cerrar haciendo clic
        notification.addEventListener('click', () => {
            notification.style.opacity = '0';
            setTimeout(() => {
                if (notification.parentNode) {
                    notification.parentNode.removeChild(notification);
                }
            }, 300);
        });
    }

    // API pública
    return {
        showError,
        showSuccess,
        showInfo: (msg, duration) => showMessage(msg, 'info', duration || 3000),
        showWarning: (msg, duration) => showMessage(msg, 'warning', duration || 4000)
    };
})();

// Hacer disponible globalmente
window.Notifications = Notifications;
