export const NoteLayer = {
    Top: 1,
    Middle: 2,
    Base: 3,
    Flat: 4,
} as const;

export type NoteLayer = (typeof NoteLayer)[keyof typeof NoteLayer];

export const GenderCategory = {
    Unisex: 1,
    Male: 2,
    Female: 3,
} as const;

export type GenderCategory =
    (typeof GenderCategory)[keyof typeof GenderCategory];

export interface Note {
    id: number;
    name: string;
    weight: number;
    layer: NoteLayer;
}

export interface Accord {
    id: number;
    name: string;
    weight: number;
}

export interface Perfume {
    id: number;
    name: string;
    description: string;
    brand: string;
    gender: GenderCategory;
    imageUrl: string | null;
    rating: number;
    notes: Note[];
    accords: Accord[];
}

export type SearchMode = 'balanced' | 'clones' | 'similar';

export interface PerfumeSearchResult {
    perfume: Perfume;
    score: number;
}

export interface SearchResults {
    perfumes: PerfumeSearchResult[];
}
