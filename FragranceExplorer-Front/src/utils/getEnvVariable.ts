const getEnvVar = (key: string, fallback: string): string => {
    const value = import.meta.env[key];

    if (!value) {
        if (import.meta.env.DEV) {
            console.warn(
                `[Env]: Variable ${key} was not found in .env.local. Using default value: ${fallback}`,
            );
        }
        return fallback;
    }

    return value;
};

export const ENV_BASE_URL = {
    API_URL: getEnvVar('VITE_API_URL', 'https://localhost:7281/api'),
} as const;
