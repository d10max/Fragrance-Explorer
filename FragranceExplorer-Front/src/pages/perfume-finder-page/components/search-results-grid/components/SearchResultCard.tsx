import { Chip } from '@/components/shared/chip/Chip';
import { SEARCH_RESULTS_GRID_TEXT } from '@/const/common';
import type { Perfume, PerfumeSearchResult } from '@/types/common/perfume';
import styles from '@/pages/perfume-finder-page/components/search-results-grid/components/SearchResultCard.module.scss';

export interface SearchResultCardProps {
    result: PerfumeSearchResult;
    rank: number;
    targetPerfume?: Perfume;
}

export const SearchResultCard = ({ result, rank }: SearchResultCardProps) => {
    const { perfume, score } = result;

    return (
        <div className={styles.card}>
            <div className={styles.topBar}>
                <span className={styles.rank}>#{rank}</span>
                <Chip
                    label={SEARCH_RESULTS_GRID_TEXT.PERFUME_CARD.MATCH_BADGE_TEXT(
                        score,
                    )}
                    variant="green"
                />
            </div>

            <div className={styles.mediaBox}>
                <img
                    src={perfume.imageUrl ?? ''}
                    alt={perfume.name}
                    className={styles.image}
                />
            </div>

            <div className={styles.mainInfo}>
                <span className={styles.brand}>{perfume.brand}</span>
                <h3 className={styles.name}>{perfume.name}</h3>
            </div>

            <div className={styles.vibeBox}>
                <span className={styles.vibeLabel}>
                    {
                        SEARCH_RESULTS_GRID_TEXT.PERFUME_CARD
                            .VIBE_LABEL_PLACEHOLDER
                    }
                </span>
                <p className={styles.vibeText}>
                    {perfume.description ||
                        SEARCH_RESULTS_GRID_TEXT.PERFUME_CARD
                            .VIBE_DEFAULT_LABEL}
                </p>
            </div>
        </div>
    );
};
