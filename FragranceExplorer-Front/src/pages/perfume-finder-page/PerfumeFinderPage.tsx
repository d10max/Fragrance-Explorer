import type { PerfumeSearchItem, SearchMode } from '@/types/common/perfume';
import { SearchPanel } from './components/search-panel/SearchPanel';
import { TargetPerfumeCard } from './components/target-perfume-card/TargetPerfumeCard';
import { useState } from 'react';
import { SearchModesPanel } from './components/search-modes-panel/SearchModesPanel';
import { SearchResultsGrid } from './components/search-results-grid/SearchResultsGrid';
import { usePerfumeRecommendations } from '@/hooks/usePerfumeRecommendations';

export const PerfumeFinderPage = () => {
    const [selectedPerfume, setSelectedPerfume] =
        useState<PerfumeSearchItem | null>(null);
    const [selectedMode, setSelectedMode] = useState<SearchMode>('balanced');

    const {
        results,
        isLoading,
        hasSearched,
        fetchRecommendations,
        clearResults,
    } = usePerfumeRecommendations();

    const handleSelectPerfume = (perfume: PerfumeSearchItem) => {
        setSelectedPerfume(perfume);
        clearResults();
    };

    const handleRemovePerfume = () => {
        setSelectedPerfume(null);
        clearResults();
    };

    const handleFindPerfumes = async () => {
        if (!selectedPerfume) return;
        await fetchRecommendations(selectedPerfume.id, selectedMode);
    };

    return (
        <div>
            <SearchPanel
                onSelectPerfume={handleSelectPerfume}
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
                isDisabled={!selectedPerfume || isLoading}
            />
            <SearchResultsGrid results={results} hasSearched={hasSearched} />
        </div>
    );
};
