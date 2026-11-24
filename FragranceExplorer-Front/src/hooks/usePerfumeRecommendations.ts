import { useState } from 'react';
import { PerfumeApi } from '@/api-services/perfumeApi';
import type { RecommendedPerfume, SearchMode } from '@/types/common/perfume';
import { COMMON_TEXT } from '@/const/common';

export const usePerfumeRecommendations = () => {
    const [results, setResults] = useState<RecommendedPerfume[]>([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [hasSearched, setHasSearched] = useState(false);

    const fetchRecommendations = async (
        perfumeId: number,
        mode: SearchMode,
    ) => {
        setIsLoading(true);
        setError(null);

        try {
            const items = await PerfumeApi.getRecommendations({
                perfumeId,
                mode,
            });
            setResults(items);
            setHasSearched(true);
        } catch {
            setError(COMMON_TEXT.FAILED_TO_LOAD_RECOMMENDATIONS);
            setResults([]);
        } finally {
            setIsLoading(false);
        }
    };

    const clearResults = () => {
        setResults([]);
        setError(null);
        setHasSearched(false);
    };

    return {
        results,
        isLoading,
        error,
        hasSearched,
        fetchRecommendations,
        clearResults,
    };
};
