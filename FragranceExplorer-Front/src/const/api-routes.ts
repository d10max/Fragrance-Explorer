import { ENV_BASE_URL } from '@/utils/getEnvVariable';

export const API_ROUTES = {
    BASE: ENV_BASE_URL.API_URL,
    PERFUMES: {
        SEARCH: '/Perfumes/search',
        RECOMMENDATIONS: (id: number | string) =>
            `/Perfumes/${id}/recommendations`,
    },
};
