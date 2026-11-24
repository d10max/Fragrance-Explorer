import { Chip } from '@/components/shared/chip/Chip';
import { SEARCH_RESULTS_GRID_TEXT } from '@/const/common';
import type { RecommendedPerfume } from '@/types/common/perfume';
import styles from '@/pages/perfume-finder-page/components/search-results-grid/components/SearchResultCard.module.scss';

export interface SearchResultCardProps {
    result: RecommendedPerfume;
    rank: number;
}

const toPercents = (value: number) => Math.round(value * 100);

export const SearchResultCard = ({ result, rank }: SearchResultCardProps) => {
    return (
        <div className={styles.card}>
            <div className={styles.topBar}>
                <span className={styles.rank}>#{rank}</span>
                <Chip
                    label={SEARCH_RESULTS_GRID_TEXT.PERFUME_CARD.MATCH_BADGE_TEXT(
                        toPercents(result.similarityScore),
                    )}
                    variant="green"
                />
            </div>

            <div className={styles.mediaBox}>
                <img
                    src={result.imageUrl ?? ''}
                    alt={result.name}
                    className={styles.image}
                />
            </div>

            <div className={styles.mainInfo}>
                <span className={styles.brand}>{result.brandName}</span>
                <h3 className={styles.name}>{result.name}</h3>
            </div>

            <div className={styles.vibeBox}>
                <span className={styles.vibeLabel}>
                    {
                        SEARCH_RESULTS_GRID_TEXT.PERFUME_CARD
                            .VIBE_LABEL_PLACEHOLDER
                    }
                </span>
                <p className={styles.vibeText}>
                    {result.description ||
                        SEARCH_RESULTS_GRID_TEXT.PERFUME_CARD
                            .VIBE_DEFAULT_LABEL}
                </p>
            </div>
        </div>
    );
};
