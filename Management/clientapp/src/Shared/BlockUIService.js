// src/Shared/BlockUIService.js

export default (function () {
    return {
        Start() {
            // Create overlay
            const overlay = document.createElement('div');
            overlay.className = 'block-ui-overlay';
            overlay.innerHTML = `
                <div class="spinner">
                    <div class="sk-wave">
                        <div class="sk-rect sk-wave-rect"></div>
                        <div class="sk-rect sk-wave-rect"></div>
                        <div class="sk-rect sk-wave-rect"></div>
                        <div class="sk-rect sk-wave-rect"></div>
                        <div class="sk-rect sk-wave-rect"></div>
                    </div>
                </div>`;

            // Apply styles
            overlay.style.position = 'fixed';
            overlay.style.top = '0';
            overlay.style.left = '0';
            overlay.style.width = '100%';
            overlay.style.height = '100%';
            overlay.style.backgroundColor = 'rgba(0, 0, 0, 0.5)';
            overlay.style.zIndex = '9999';
            overlay.style.display = 'flex';
            overlay.style.alignItems = 'center';
            overlay.style.justifyContent = 'center';

            // Append overlay to the body
            document.body.appendChild(overlay);

            // Optional: Automatically remove after timeout
            //setTimeout(() => {
            //    this.Stop();
            //}, 1000); // Change timeout duration as needed
        },

        Stop() {
            const overlay = document.querySelector('.block-ui-overlay');
            if (overlay) {
                document.body.removeChild(overlay);
            }
        }
    };
})();