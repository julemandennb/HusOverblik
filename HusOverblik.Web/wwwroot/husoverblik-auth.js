window.husOverblikAuth = {
    login: async function (email, password) {
        const response = await fetch(
            "/api/auth/login?useCookies=true",
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                credentials: "same-origin",
                body: JSON.stringify({
                    email: email,
                    password: password
                })
            }
        );

        return response.ok;
    },

    logout: async function () {
        const response = await fetch("/api/auth/logout", {
            method: "POST",
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({})
        });

        return response.ok;
    }
};