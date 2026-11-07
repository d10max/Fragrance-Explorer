import type { PerfumeSearchResult } from '@/types/common/perfume';
import { SearchResultCard } from './components/SearchResultCard';
import { SEARCH_RESULTS_GRID_TEXT } from '@/const/common';
import styles from '@/pages/perfume-finder-page/components/search-results-grid/SearchResults.Grid.module.scss';

export interface SearchResultsGridProps {
    results: PerfumeSearchResult[];
    hasSearched: boolean;
}

export const SearchResultsGrid = ({
    results,
    hasSearched = false,
}: SearchResultsGridProps) => {
    if (!hasSearched) {
        return null;
    }

    if (results.length === 0) {
        return (
            <div className={styles.emptySection}>
                <div className={styles.content}>
                    <span className={styles.tag}>
                        {SEARCH_RESULTS_GRID_TEXT.EMPTY_GRID.TAG}
                    </span>
                    <h3 className={styles.title}>
                        {SEARCH_RESULTS_GRID_TEXT.EMPTY_GRID.TITLE}
                    </h3>
                    <p className={styles.subtitle}>
                        {SEARCH_RESULTS_GRID_TEXT.EMPTY_GRID.SUBTITLE}
                    </p>
                </div>
            </div>
        );
    }

    return (
        <div className={styles.section}>
            <div className={styles.header}>
                <div className={styles.titleBlock}>
                    <span className={styles.tag}>
                        {SEARCH_RESULTS_GRID_TEXT.GRID.TAG}
                    </span>
                    <h2 className={styles.title}>
                        {SEARCH_RESULTS_GRID_TEXT.GRID.TITLE}
                    </h2>
                    <p className={styles.subtitle}>
                        {SEARCH_RESULTS_GRID_TEXT.GRID.SUBTITLE}
                    </p>
                </div>
            </div>

            <div className={styles.grid}>
                {results.map((item, index) => (
                    <SearchResultCard result={item} rank={index + 1} />
                ))}
            </div>
        </div>
    );
};
