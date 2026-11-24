import { axiosInstance } from '@/api-services/axios';
import { API_ROUTES } from '@/const/api-routes';
import {
    SEARCH_MODE_TO_PROFILE,
    type PerfumeRecommendationsResponseDto,
    type PerfumeSearchItem,
    type PerfumeSearchResponseDto,
    type RecommendedPerfume,
    type SearchMode,
} from '@/types/common/perfume';

export interface SearchPerfumesParams {
    query: string;
    maxResults?: number;
}

export interface GetRecommendationsParams {
    perfumeId: number;
    mode: SearchMode;
    count?: number;
}

export const PerfumeApi = {
    search: async ({
        query,
        maxResults = 10,
    }: SearchPerfumesParams): Promise<PerfumeSearchItem[]> => {
        const response = await axiosInstance.get<PerfumeSearchResponseDto>(
            API_ROUTES.PERFUMES.SEARCH,
            {
                params: { query, maxResults },
            },
        );

        return response.data.items;
    },
    getRecommendations: async ({
        perfumeId,
        mode,
        count = 10,
    }: GetRecommendationsParams): Promise<RecommendedPerfume[]> => {
        const response =
            await axiosInstance.get<PerfumeRecommendationsResponseDto>(
                API_ROUTES.PERFUMES.RECOMMENDATIONS(perfumeId),
                {
                    params: {
                        count,
                        profile: SEARCH_MODE_TO_PROFILE[mode],
                    },
                },
            );

        return response.data.recommendedPerfumes;
    },
};
