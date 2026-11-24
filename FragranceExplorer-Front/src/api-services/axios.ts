import { API_ROUTES } from '@/const/api-routes';
import axios from 'axios';

export const axiosInstance = axios.create({
    baseURL: API_ROUTES.BASE,
    timeout: 10000,
    headers: {
        'Content-Type': 'application/json',
    },
});
