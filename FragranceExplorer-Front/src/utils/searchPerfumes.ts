import type { Perfume } from '@/types/common/perfume';

export const searchPerfumes = (
    perfumes: Perfume[],
    query: string,
): Perfume[] => {
    const cleanQuery = query.trim().toLowerCase();
    if (!cleanQuery) return [];

    return perfumes.filter((perfume) => {
        const name = perfume.name.toLowerCase();
        const brand = perfume.brand.toLowerCase();
        const hasNoteMatch = perfume.notes.some((note) =>
            note.name.toLowerCase().includes(cleanQuery),
        );

        return (
            name.includes(cleanQuery) ||
            brand.includes(cleanQuery) ||
            hasNoteMatch
        );
    });
};
