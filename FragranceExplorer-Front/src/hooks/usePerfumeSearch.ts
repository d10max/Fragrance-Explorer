import { useState, useEffect } from 'react';
import { PerfumeApi } from '@/api-services/perfumeApi';
import type { PerfumeSearchItem } from '@/types/common/perfume';
import { COMMON_TEXT } from '@/const/common';

export const usePerfumeSearch = (query: string, minLength = 2) => {
    const [results, setResults] = useState<PerfumeSearchItem[]>([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const trimmedQuery = query.trim();
    const isValid = trimmedQuery.length >= minLength;

    useEffect(() => {
        if (!isValid) return;

        let isIgnored = false;

        const timer = setTimeout(async () => {
            setIsLoading(true);
            setError(null);

            try {
                const items = await PerfumeApi.search({
                    query: trimmedQuery,
                });

                if (!isIgnored) {
                    setResults(items);
                }
            } catch {
                if (!isIgnored) {
                    setError(COMMON_TEXT.FAILED_TO_LOAD_PERFUMES_LIST);
                    setResults([]);
                }
            } finally {
                if (!isIgnored) {
                    setIsLoading(false);
                }
            }
        }, 350);

        return () => {
            isIgnored = true;
            clearTimeout(timer);
        };
    }, [trimmedQuery, isValid]);

    return {
        results: isValid ? results : [],
        isLoading: isValid ? isLoading : false,
        error: isValid ? error : null,
    };
};
