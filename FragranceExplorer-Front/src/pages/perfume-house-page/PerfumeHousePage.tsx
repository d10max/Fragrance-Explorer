import { useState, useMemo, useEffect } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { getHouseByIdOrName, getSimilarHouses } from '@/pages/perfume-catalog-page/houses.data';
import { getPerfumesByHouse } from '@/pages/perfume-catalog-page/perfumes.data';
import { PerfumeBottle } from '@/pages/perfume-catalog-page/components/PerfumeBottle';
import { ROUTES } from '@/const/routes';
import styles from './PerfumeHousePage.module.scss';

type FamilyFilterType = 'All' | 'Woody' | 'Spicy' | 'Fresh' | 'Citrus';

export const PerfumeHousePage = () => {
    const { houseId } = useParams<{ houseId: string }>();
    const navigate = useNavigate();

    const house = useMemo(() => getHouseByIdOrName(houseId), [houseId]);
    const housePerfumes = useMemo(
        () => getPerfumesByHouse(house.name, house.id),
        [house.name, house.id]
    );
    const similarHouses = useMemo(() => getSimilarHouses(house), [house]);

    const [searchQuery, setSearchQuery] = useState('');
    const [selectedFamily, setSelectedFamily] = useState<FamilyFilterType>('All');
    const [showAllArchive, setShowAllArchive] = useState(false);

    // Reset filters and scroll to top when switching between fragrance houses
    useEffect(() => {
        setSearchQuery('');
        setSelectedFamily('All');
        setShowAllArchive(false);
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }, [houseId]);

    const familyTabs: { id: FamilyFilterType; count: number }[] = useMemo(() => {
        const counts = house.familyCounts || {
            All: house.archiveCount || housePerfumes.length,
            Woody: 8,
            Spicy: 5,
            Fresh: 4,
            Citrus: 2,
        };
        return [
            { id: 'All', count: counts.All },
            { id: 'Woody', count: counts.Woody ?? 0 },
            { id: 'Spicy', count: counts.Spicy ?? 0 },
            { id: 'Fresh', count: counts.Fresh ?? 0 },
            { id: 'Citrus', count: counts.Citrus ?? 0 },
        ];
    }, [house, housePerfumes.length]);

    const filteredPerfumes = useMemo(() => {
        return housePerfumes.filter((perfume) => {
            if (searchQuery.trim()) {
                const q = searchQuery.toLowerCase().trim();
                const matchesName = perfume.name.toLowerCase().includes(q);
                const matchesNote = perfume.keyNotes.some((n) =>
                    n.toLowerCase().includes(q)
                );
                const matchesStory = perfume.storyAndVibe.toLowerCase().includes(q);
                if (!matchesName && !matchesNote && !matchesStory) {
                    return false;
                }
            }

            if (selectedFamily !== 'All' && perfume.family !== selectedFamily) {
                return false;
            }

            return true;
        });
    }, [housePerfumes, searchQuery, selectedFamily]);

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

    const handleResetFilters = () => {
        setSearchQuery('');
        setSelectedFamily('All');
    };

    return (
        <div className={styles.container}>
            {/* Breadcrumbs */}
            <nav className={styles.breadcrumbs} aria-label="Breadcrumbs">
                <Link to={ROUTES.PERFUME_CATALOG.FULL} className={styles.backLink}>
                    &larr; Back to Perfume Encyclopedia
                </Link>
                <span className={styles.crumbDivider}>/</span>
                <Link to={ROUTES.PERFUME_CATALOG.FULL} className={styles.crumbSection}>
                    Fragrance Houses
                </Link>
                <span className={styles.crumbDivider}>/</span>
                <span className={styles.crumbCurrent}>{house.name}</span>
            </nav>

            {/* Fragrance House Dossier Card */}
            <section className={styles.dossierCard}>
                {/* Left Monogram Box */}
                <div className={styles.monogramBox}>
                    <span className={styles.monogramText}>{house.initials}</span>
                    <span className={styles.monogramOrigin}>{house.originLabel}</span>
                    <span className={styles.monogramEst}>{house.establishedLabel}</span>
                </div>

                {/* Middle House Story & Badges */}
                <div className={styles.houseInfo}>
                    <span className={styles.dossierEyebrow}>{house.dossierEyebrow}</span>
                    <h1 className={styles.houseTitle}>{house.displayName}</h1>

                    <div className={styles.badgesRow}>
                        <span className={styles.archivePill}>
                            {house.archiveCount} Fragrances in Archive
                        </span>
                        <span className={styles.dnaPill}>{house.dnaBadge}</span>
                        <span className={styles.ratingPill}>
                            ★ {house.rating} House Average ({house.reviewsLabel})
                        </span>
                    </div>

                    <p className={styles.houseDescription}>{house.description}</p>
                </div>

                {/* Right House Olfactory DNA Card */}
                <div className={styles.dnaCard}>
                    <span className={styles.dnaCardTitle}>HOUSE OLFACTORY DNA</span>
                    <div className={styles.dnaList}>
                        {house.olfactoryDna?.map((item) => (
                            <div key={item.family} className={styles.dnaRow}>
                                <span
                                    className={`${styles.dnaFamilyChip} ${getFamilyBadgeClass(
                                        item.family
                                    )}`}
                                >
                                    {item.family}
                                </span>
                                <span className={styles.dnaNotes}>{item.notes}</span>
                                <span className={styles.dnaPercentage}>
                                    {item.percentage}%
                                </span>
                            </div>
                        ))}
                    </div>
                </div>
            </section>

            {/* Complete House Archive Section */}
            <section className={styles.archiveSection}>
                <span className={styles.archiveEyebrow}>COMPLETE HOUSE ARCHIVE</span>

                <div className={styles.archiveHeaderRow}>
                    <h2 className={styles.archiveTitle}>
                        All fragrances by {house.displayName}
                    </h2>

                    <div className={styles.searchWrapper}>
                        <input
                            type="text"
                            className={styles.houseSearchInput}
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                            placeholder={`Search within ${house.displayName} (e.g. Santal 33, Rose 31)...`}
                        />
                    </div>

                    <div className={styles.familyFilters}>
                        {familyTabs.map((tab) => {
                            const isActive = selectedFamily === tab.id;
                            const colorClass = isActive
                                ? styles.filterChipActive
                                : tab.id === 'All'
                                  ? styles.filterChipDefault
                                  : getFamilyBadgeClass(tab.id);

                            return (
                                <button
                                    key={tab.id}
                                    type="button"
                                    className={`${styles.filterChip} ${colorClass}`}
                                    onClick={() => setSelectedFamily(tab.id)}
                                >
                                    {tab.id} ({tab.count})
                                </button>
                            );
                        })}
                    </div>
                </div>

                {/* Fragrances Grid */}
                <div className={styles.perfumesGrid}>
                    {filteredPerfumes.length === 0 ? (
                        <div className={styles.emptyState}>
                            <h3 className={styles.emptyTitle}>
                                No {house.displayName} fragrances found
                            </h3>
                            <p className={styles.emptySubtitle}>
                                Try adjusting your search query or switching olfactory family filters.
                            </p>
                            <button
                                type="button"
                                className={styles.resetBtn}
                                onClick={handleResetFilters}
                            >
                                Reset Filters
                            </button>
                        </div>
                    ) : (
                        filteredPerfumes.map((perfume) => {
                            return (
                                <div
                                    key={perfume.id}
                                    className={styles.perfumeCard}
                                    onClick={() =>
                                        navigate(ROUTES.PERFUME_DETAIL.FULL(perfume.id))
                                    }
                                >
                                    <div className={styles.cardTop}>
                                        <span
                                            className={`${styles.cardFamilyChip} ${getFamilyBadgeClass(
                                                perfume.family
                                            )}`}
                                        >
                                            {perfume.family}
                                        </span>
                                        <span className={styles.matchRatingBadge}>
                                            {perfume.matchPercentage || 92}% Match &middot;{' '}
                                            {perfume.rating} ★
                                        </span>
                                    </div>

                                    <div className={styles.bottleDisplay}>
                                        <PerfumeBottle
                                            capColor={perfume.capColor}
                                            liquidColor={perfume.liquidColor}
                                            liquidHeight={perfume.liquidHeight}
                                        />
                                    </div>

                                    <span className={styles.cardBrand}>{perfume.brand}</span>
                                    <h3 className={styles.cardName}>{perfume.name}</h3>
                                    <p className={styles.cardMeta}>
                                        {perfume.concentration} &middot; {perfume.year}
                                    </p>

                                    <div className={styles.notesBlock}>
                                        <span className={styles.notesLabel}>KEY NOTES</span>
                                        <div className={styles.notesChips}>
                                            {perfume.keyNotes.map((note) => (
                                                <span key={note} className={styles.noteItem}>
                                                    {note}
                                                </span>
                                            ))}
                                        </div>
                                    </div>

                                    <div className={styles.storyBox}>
                                        <span className={styles.storyLabel}>
                                            THE STORY &amp; VIBE
                                        </span>
                                        <p className={styles.storyQuote}>
                                            &ldquo;{perfume.storyAndVibe}&rdquo;
                                        </p>
                                    </div>

                                    <div className={styles.cardFooter}>
                                        <span
                                            className={styles.readMoreLink}
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                navigate(
                                                    ROUTES.PERFUME_DETAIL.FULL(perfume.id)
                                                );
                                            }}
                                        >
                                            Read full story &amp; notes &rarr;
                                        </span>
                                        <button
                                            type="button"
                                            className={styles.starterKitBtn}
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                alert(
                                                    `Added ${perfume.name} to Starter Kit!`
                                                );
                                            }}
                                        >
                                            + Starter Kit
                                        </button>
                                    </div>
                                </div>
                            );
                        })
                    )}
                </div>

                {/* Show All House Fragrances Button */}
                {filteredPerfumes.length > 0 && (
                    <div className={styles.showAllWrapper}>
                        <button
                            type="button"
                            className={styles.showAllBtn}
                            onClick={() => setShowAllArchive((prev) => !prev)}
                        >
                            {showAllArchive
                                ? `Showing All ${house.archiveCount} ${house.displayName} Fragrances`
                                : `Show All ${house.archiveCount} ${house.displayName} Fragrances`}
                        </button>
                    </div>
                )}
            </section>

            {/* Continue Exploring: Other houses with similar DNA */}
            <section className={styles.otherHousesSection}>
                <span className={styles.otherEyebrow}>CONTINUE EXPLORING</span>
                <h2 className={styles.otherTitle}>Other houses with similar DNA</h2>

                <div className={styles.otherHousesGrid}>
                    {similarHouses.map((simHouse) => (
                        <div
                            key={simHouse.id}
                            className={styles.otherHouseCard}
                            onClick={() =>
                                navigate(ROUTES.PERFUME_HOUSE.FULL(simHouse.id))
                            }
                        >
                            <h3 className={styles.otherHouseName}>{simHouse.name}</h3>
                            <p className={styles.otherHouseMeta}>{simHouse.location}</p>
                            <span className={styles.otherHouseBadge}>
                                {simHouse.count}
                            </span>
                        </div>
                    ))}
                </div>
            </section>
        </div>
    );
};

