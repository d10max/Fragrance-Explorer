import type {
    Perfume,
    PerfumeSearchResult,
    SearchMode,
} from '@/types/common/perfume';
import { SearchPanel } from './components/search-panel/SearchPanel';
import { TargetPerfumeCard } from './components/target-perfume-card/TargetPerfumeCard';
import { useState } from 'react';
import { SearchModesPanel } from './components/search-modes-panel/SearchModesPanel';
import { SearchResultsGrid } from './components/search-results-grid/SearchResultsGrid';

const perfumes: Perfume[] = [
    {
        id: 1,
        name: 'Test Perfume',
        description: 'A test perfume for demonstration purposes.',
        brand: 'Test Brand',
        imageUrl: 'https://fimgs.net/mdimg/perfume/375x500.8.jpg',
        gender: 1,
        rating: 4.5,
        notes: [],
        accords: [],
    },
    {
        id: 2,
        name: 'Another Perfume',
        description: 'Another test perfume for demonstration purposes.',
        brand: 'Another Brand',
        imageUrl: 'https://fimgs.net/mdimg/perfume/375x500.5.jpg',
        gender: 2,
        rating: 4.0,
        notes: [],
        accords: [],
    },
    {
        id: 3,
        name: 'Arpege Pour Homme',
        description:
            'This is one of the latest fragrances for men by Lanvin. This perfume is completely balanced; interesting harmonies appear with different notes: pepper-like dryness of the top notes and waves of warmth find its way through it.',
        brand: 'Lanvin',
        imageUrl: 'https://fimgs.net/mdimg/perfume/375x500.14.jpg',
        gender: 3,
        rating: 4.0,
        notes: [],
        accords: [],
    },
    {
        id: 4,
        name: 'Another Perfume',
        description: 'Another test perfume for demonstration purposes.',
        brand: 'Another one Brand',
        imageUrl: 'https://fimgs.net/mdimg/perfume/375x500.15.jpg',
        gender: 2,
        rating: 4.0,
        notes: [],
        accords: [],
    },
    {
        id: 5,
        name: 'Another Perfume',
        description: 'Another test perfume for demonstration purposes.',
        brand: 'Another one Brand',
        imageUrl: 'https://fimgs.net/mdimg/perfume/375x500.155.jpg',
        gender: 2,
        rating: 4.0,
        notes: [],
        accords: [],
    },
    {
        id: 6,
        name: 'Another Perfume',
        description: 'Another test perfume for demonstration purposes.',
        brand: 'Another one Brand',
        imageUrl: 'https://fimgs.net/mdimg/perfume/375x500.3222.jpg',
        gender: 2,
        rating: 4.0,
        notes: [],
        accords: [],
    },
    {
        id: 7,
        name: 'Another Perfume',
        description: 'Another test perfume for demonstration purposes.',
        brand: 'Another one Brand',
        imageUrl: 'https://fimgs.net/mdimg/perfume/375x500.5800.jpg',
        gender: 2,
        rating: 4.0,
        notes: [],
        accords: [],
    },
    {
        id: 8,
        name: 'Another Perfume',
        description: 'Another test perfume for demonstration purposes.',
        brand: 'Another one Brand',
        imageUrl: 'https://fimgs.net/mdimg/perfume/375x500.59.jpg',
        gender: 2,
        rating: 4.0,
        notes: [],
        accords: [],
    },
    {
        id: 9,
        name: 'Another Perfume',
        description: 'Another test perfume for demonstration purposes.',
        brand: 'Another one Brand',
        imageUrl: 'https://fimgs.net/mdimg/perfume/375x500.606.jpg',
        gender: 2,
        rating: 4.0,
        notes: [],
        accords: [],
    },
    {
        id: 10,
        name: 'Another Perfume',
        description: 'Another test perfume for demonstration purposes.',
        brand: 'Another one Brand',
        imageUrl: 'https://fimgs.net/mdimg/perfume/375x500.666.jpg',
        gender: 2,
        rating: 4.0,
        notes: [],
        accords: [],
    },
];

export const PerfumeFinderPage = () => {
    const [selectedPerfume, setSelectedPerfume] = useState<Perfume | null>(
        null,
    );
    const [selectedMode, setSelectedMode] = useState<SearchMode>('balanced');
    const [searchResults, setSearchResults] = useState<PerfumeSearchResult[]>(
        [],
    );
    const [hasSearched, setHasSearched] = useState(false);

    const handleSelectPerfume = (perfume: Perfume) => {
        setSelectedPerfume(perfume);
        setSearchResults([]);
        setHasSearched(false);
    };

    const handleRemovePerfume = () => {
        setSelectedPerfume(null);
        setSearchResults([]);
        setHasSearched(false);
    };

    const handleFindPerfumes = () => {
        // Connect to the backend API to fetch search results based on the selected perfume and mode
        const results: PerfumeSearchResult[] = perfumes
            .map((perfume) => ({
                perfume,
                score: Math.floor(Math.random() * 100),
            }))
            .sort((a, b) => b.score - a.score);

        setSearchResults(results);
        setHasSearched(true);
    };

    return (
        <div>
            <SearchPanel
                onSelectPerfume={handleSelectPerfume}
                perfumes={perfumes}
                isDropdownOpen={selectedPerfume === null}
            />
            <TargetPerfumeCard
                selectedPerfume={selectedPerfume}
                onRemove={handleRemovePerfume}
            />
            <SearchModesPanel
                selectedMode={selectedMode}
                onSelectMode={setSelectedMode}
                onFindPerfumes={() => handleFindPerfumes()}
                isDisabled={!selectedPerfume}
            />
            <SearchResultsGrid
                results={searchResults}
                hasSearched={hasSearched}
            />
        </div>
    );
};
