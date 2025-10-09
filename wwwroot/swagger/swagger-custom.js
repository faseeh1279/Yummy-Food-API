// wwwroot/swagger/swagger-custom.js
window.onload = function () {
    const ui = SwaggerUIBundle({
        url: '/swagger/v1/swagger.json',
        dom_id: '#swagger-ui',
        presets: [
            SwaggerUIBundle.presets.apis,
            SwaggerUIStandalonePreset
        ],
        layout: "BaseLayout",
        requestInterceptor: async (req) => {
            // Automatically inject token (for demo, fetch once and reuse)
            if (!window.localStorage.getItem('access_token')) {
                // Call your login endpoint to get the token
                const loginResponse = await fetch('/api/Auth/Login', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ email: 'admin@test.com', password: '123456' })
                });
                const data = await loginResponse.json();
                window.localStorage.setItem('access_token', data.token);
            }

            const token = window.localStorage.getItem('access_token');
            if (token) {
                req.headers['Authorization'] = `Bearer ${token}`;
            }
            return req;
        }
    });

    window.ui = ui;
};
