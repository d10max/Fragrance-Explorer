import { useState, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { useProfile } from '@/context/ProfileContext';
import { useCompare } from '@/context/CompareContext';
import { PerfumeBottle } from '@/pages/perfume-catalog-page/components/PerfumeBottle';
import { ROUTES } from '@/const/routes';
import styles from './FavoritePerfumes.module.scss';

export const FavoritePerfumes = () => {
    const navigate = useNavigate();
    const { favoritePerfumes, removeFavorite } = useProfile();
    const { compareIds, addToCompare } = useCompare();

    const [selectedFamily, setSelectedFamily] = useState<string>('All');
    const [searchQuery, setSearchQuery] = useState<string>('');

    const families = ['All', 'Woody', 'Citrus', 'Fresh', 'Spicy'];

    const filteredPerfumes = useMemo(() => {
        return favoritePerfumes.filter((perfume) => {
            const matchesFamily =
                selectedFamily === 'All' || perfume.family === selectedFamily;
            const matchesQuery =
                !searchQuery.trim() ||
                perfume.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
                perfume.brand.toLowerCase().includes(searchQuery.toLowerCase()) ||
                perfume.keyNotes.some((n) =>
                    n.toLowerCase().includes(searchQuery.toLowerCase())
                );
            return matchesFamily && matchesQuery;
        });
    }, [favoritePerfumes, selectedFamily, searchQuery]);

    const getFamilyBadgeClass = (family: string) => {
        switch (family) {
            case 'Woody':
                return styles.chipWoody;
            case 'Citrus':
                return styles.chipCitrus;
            case 'Fresh':
                return styles.chipFresh;
            case 'Spicy':
                return styles.chipSpicy;
            default:
                return styles.chipWoody;
        }
    };

    return (
        <section className={styles.favoritesSection}>
            {/* Header with filters and counter */}
            <div className={styles.topBar}>
                <div className={styles.filterGroup}>
                    <div className={styles.familyChips}>
                        {families.map((fam) => {
                            const count =
                                fam === 'All'
                                    ? favoritePerfumes.length
                                    : favoritePerfumes.filter((p) => p.family === fam).length;
                            return (
                                <button
                                    key={fam}
                                    type="button"
                                    className={`${styles.filterChip} ${
                                        selectedFamily === fam ? styles.filterChipActive : ''
                                    }`}
                                    onClick={() => setSelectedFamily(fam)}
                                >
                                    <span>{fam}</span>
                                    <span className={styles.chipCount}>{count}</span>
                                </button>
                            );
                        })}
                    </div>
                </div>

                <div className={styles.searchBox}>
                    <input
                        type="text"
                        placeholder="Search saved perfumes, brands, notes..."
                        value={searchQuery}
                        onChange={(e) => setSearchQuery(e.target.value)}
                        className={styles.searchInput}
                    />
                    {searchQuery && (
                        <button
                            type="button"
                            className={styles.clearBtn}
                            onClick={() => setSearchQuery('')}
                        >
                            &times;
                        </button>
                    )}
                </div>
            </div>

            {/* Empty state */}
            {filteredPerfumes.length === 0 ? (
                <div className={styles.emptyState}>
                    <div className={styles.emptyIcon}>✦</div>
                    <h4 className={styles.emptyTitle}>
                        {favoritePerfumes.length === 0
                            ? 'Your Olfactory Wardrobe is Empty'
                            : 'No saved fragrances match your filter'}
                    </h4>
                    <p className={styles.emptyText}>
                        {favoritePerfumes.length === 0
                            ? 'Discover fragrances in our encyclopedia and save them to your personal wardrobe to view personalized taste insights.'
                            : 'Try selecting a different fragrance family or clearing your search query.'}
                    </p>
                    {favoritePerfumes.length === 0 ? (
                        <button
                            type="button"
                            className={styles.exploreBtn}
                            onClick={() => navigate(ROUTES.PERFUME_CATALOG.FULL)}
                        >
                            Explore Perfume Encyclopedia &rarr;
                        </button>
                    ) : (
                        <button
                            type="button"
                            className={styles.resetBtn}
                            onClick={() => {
                                setSelectedFamily('All');
                                setSearchQuery('');
                            }}
                        >
                            Reset Filters
                        </button>
                    )}
                </div>
            ) : (
                /* Perfumes Grid */
                <div className={styles.grid}>
                    {filteredPerfumes.map((perfume) => {
                        const isInCompare = compareIds.includes(perfume.id);

                        return (
                            <div
                                key={perfume.id}
                                className={styles.card}
                                onClick={() => navigate(ROUTES.PERFUME_DETAIL.FULL(perfume.id))}
                            >
                                {/* Card Header with Family and Rating */}
                                <div className={styles.cardTop}>
                                    <span
                                        className={`${styles.familyBadge} ${getFamilyBadgeClass(
                                            perfume.family
                                        )}`}
                                    >
                                        {perfume.family}
                                    </span>
                                    <div className={styles.topActions}>
                                        <span className={styles.ratingBadge}>
                                            {perfume.rating} ★
                                        </span>
                                        <button
                                            type="button"
                                            className={styles.removeBtn}
                                            title="Remove from favorites"
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                removeFavorite(perfume.id);
                                            }}
                                        >
                                            <svg
                                                width="14"
                                                height="14"
                                                viewBox="0 0 24 24"
                                                fill="currentColor"
                                            >
                                                <path d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z" />
                                            </svg>
                                        </button>
                                    </div>
                                </div>

                                {/* Bottle Display */}
                                <div className={styles.bottleBox}>
                                    <PerfumeBottle
                                        imageUrl={perfume.imageUrl}
                                        alt={perfume.name}
                                        capColor={perfume.capColor}
                                        liquidColor={perfume.liquidColor}
                                        liquidHeight={perfume.liquidHeight}
                                        width={90}
                                        height={130}
                                    />
                                </div>

                                {/* Name & Brand */}
                                <span className={styles.cardBrand}>{perfume.brand}</span>
                                <h4 className={styles.cardName}>{perfume.name}</h4>
                                <p className={styles.cardMeta}>
                                    {perfume.concentration} · {perfume.year}
                                </p>

                                {/* Key Notes */}
                                <div className={styles.notesBlock}>
                                    <span className={styles.notesLabel}>KEY NOTES</span>
                                    <div className={styles.notesChips}>
                                        {perfume.keyNotes.map((note) => (
                                            <span key={note} className={styles.noteChip}>
                                                {note}
                                            </span>
                                        ))}
                                    </div>
                                </div>

                                {/* Story Quote */}
                                <div className={styles.storyBox}>
                                    <p className={styles.storyQuote}>
                                        &ldquo;{perfume.storyAndVibe}&rdquo;
                                    </p>
                                </div>

                                {/* Card Actions */}
                                <div className={styles.cardFooter}>
                                    <span
                                        className={styles.detailLink}
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            navigate(ROUTES.PERFUME_DETAIL.FULL(perfume.id));
                                        }}
                                    >
                                        View dossier &rarr;
                                    </span>
                                    <button
                                        type="button"
                                        className={`${styles.compareBtn} ${
                                            isInCompare ? styles.compareBtnActive : ''
                                        }`}
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            addToCompare(perfume.id);
                                        }}
                                    >
                                        {isInCompare ? 'Comparing' : '+ Compare'}
                                    </button>
                                </div>
                            </div>
                        );
                    })}
                </div>
            )}
        </section>
    );
};
