import { useState, useMemo, useRef, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { PERFUMES_COLLECTION, CATALOG_FEATURED_IDS } from './perfumes.data';
import { HOUSES_DATA } from './houses.data';
import { PerfumeBottle } from './components/PerfumeBottle';
import { ROUTES } from '@/const/routes';
import styles from './PerfumeCatalogPage.module.scss';

// Quick filters by family
const FAMILIES_FILTER = [
    { id: 'All', label: 'All Fragrances', count: 128 },
    { id: 'Woody', label: 'Woody', count: 42 },
    { id: 'Citrus', label: 'Citrus', count: 28 },
    { id: 'Fresh', label: 'Fresh', count: 31 },
    { id: 'Spicy', label: 'Spicy', count: 19 },
    { id: 'Amber', label: 'Amber & Floral', count: 24 },
];

const CONCENTRATIONS = [
    'All Concentrations',
    'Eau de Parfum',
    'Eau de Toilette',
    'Extrait',
];

export const PerfumeCatalogPage = () => {
    const navigate = useNavigate();
    const [searchParams, setSearchParams] = useSearchParams();
    const collectionRef = useRef<HTMLElement>(null);

    const houseFromUrl = searchParams.get('house') || searchParams.get('brand');

    // States for interactivity
    const [searchQuery, setSearchQuery] = useState('');
    const [selectedFamily, setSelectedFamily] = useState('All');
    const [selectedHouse, setSelectedHouse] = useState<string | null>(houseFromUrl);
    const [isAllHousesOpen, setIsAllHousesOpen] = useState(false);
    const [selectedConcentration, setSelectedConcentration] = useState('All Concentrations');
    const [sortAscending, setSortAscending] = useState(false);
    const [isAllPerfumesLoaded, setIsAllPerfumesLoaded] = useState(false);

    // Sync selectedHouse when URL search parameter changes
    useEffect(() => {
        const currentHouseParam = searchParams.get('house') || searchParams.get('brand');
        setSelectedHouse(currentHouseParam);

        if (currentHouseParam) {
            setTimeout(() => {
                collectionRef.current?.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }, 100);
        }
    }, [searchParams]);

    // Update house and sync with URL query params
    const updateSelectedHouse = (houseName: string | null) => {
        setSelectedHouse(houseName);
        if (houseName) {
            setSearchParams({ house: houseName });
            setTimeout(() => {
                collectionRef.current?.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }, 100);
        } else {
            const next = new URLSearchParams(searchParams);
            next.delete('house');
            next.delete('brand');
            setSearchParams(next);
        }
    };

    // smooth scroll to the list of perfumes when Search is clicked or Enter is pressed
    const handleSearch = (e?: React.FormEvent) => {
        if (e) e.preventDefault();
        collectionRef.current?.scrollIntoView({ behavior: 'smooth', block: 'start' });
    };

    // Filter perfumes based on selected filters
    const filteredPerfumes = useMemo(() => {
        const hasActiveFilter =
            Boolean(searchQuery.trim()) ||
            selectedFamily !== 'All' ||
            Boolean(selectedHouse) ||
            selectedConcentration !== 'All Concentrations' ||
            isAllPerfumesLoaded;

        const basePool = hasActiveFilter
            ? PERFUMES_COLLECTION
            : PERFUMES_COLLECTION.filter((p) => CATALOG_FEATURED_IDS.includes(p.id));

        return basePool.filter((perfume) => {
            // Search by name, brand or notes
            if (searchQuery.trim()) {
                const query = searchQuery.toLowerCase().trim();
                const matchesName = perfume.name.toLowerCase().includes(query);
                const matchesBrand = perfume.brand.toLowerCase().includes(query);
                const matchesNote = perfume.keyNotes.some((note) =>
                    note.toLowerCase().includes(query)
                );
                if (!matchesName && !matchesBrand && !matchesNote) {
                    return false;
                }
            }

            // Filter by family (Woody, Citrus, Fresh, Spicy, Amber)
            if (selectedFamily !== 'All') {
                if (selectedFamily === 'Amber') {
                    // Perfumes with oriental or amber notes or spicy
                    const hasAmberOrFloral = perfume.keyNotes.some((n) =>
                        ['Vanilla', 'Bourbon Vanilla', 'Cardamom', 'Elemi'].includes(n)
                    );
                    if (!hasAmberOrFloral && perfume.family !== 'Spicy') return false;
                } else if (perfume.family !== selectedFamily) {
                    return false;
                }
            }

            // Filter by concentration
            if (
                selectedConcentration !== 'All Concentrations' &&
                perfume.concentration !== selectedConcentration
            ) {
                return false;
            }

            // Filter by selected house 
            if (selectedHouse && perfume.brand.toUpperCase() !== selectedHouse.toUpperCase()) {
                return false;
            }

            return true;
        }).sort((a, b) => {
            return sortAscending ? a.rating - b.rating : b.rating - a.rating;
        });
    }, [searchQuery, selectedFamily, selectedHouse, selectedConcentration, sortAscending, isAllPerfumesLoaded]);

    // Reset all filters
    const handleResetFilters = () => {
        setSearchQuery('');
        setSelectedFamily('All');
        setSelectedConcentration('All Concentrations');
        updateSelectedHouse(null);
    };

    // Get the corresponding CSS class for the badge color
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
                return styles.chipAmber;
        }
    };

    return (
        <div className={styles.container}>
            {/* Main block with live search */}
            <section className={styles.hero}>
                <span className={styles.eyebrow}>THE FRAGRANCE ARCHIVE</span>
                <h1 className={styles.title}>Explore the world of fine fragrance.</h1>
                <p className={styles.subtitle}>
                    Search by perfume name, iconic house, or olfactory note — and read the story,
                    composition, and atmosphere behind every bottle.
                </p>

                {/* Search block */}
                <form
                    className={styles.searchContainer}
                    onSubmit={handleSearch}
                >
                    <input
                        className={styles.searchInput}
                        type="text"
                        value={searchQuery}
                        onChange={(e) => setSearchQuery(e.target.value)}
                        placeholder="Search perfume, brand, or note (e.g. Bal d'Afrique, Byredo, Vetiver)..."
                    />
                    <button
                        type="submit"
                        className={styles.searchBtn}
                    >
                        Search
                    </button>
                </form>
            </section>

            {/* Popular Families */}
            <section className={styles.filtersSection}>
                <span className={styles.filtersLabel}>POPULAR FAMILIES:</span>
                <div className={styles.chipsList}>
                    {FAMILIES_FILTER.map((fam) => {
                        const isActive = selectedFamily === fam.id;
                        let customClass = '';
                        if (isActive) {
                            customClass = styles.chipActive;
                        } else if (fam.id === 'Woody') {
                            customClass = styles.chipWoody;
                        } else if (fam.id === 'Citrus') {
                            customClass = styles.chipCitrus;
                        } else if (fam.id === 'Fresh') {
                            customClass = styles.chipFresh;
                        } else if (fam.id === 'Spicy') {
                            customClass = styles.chipSpicy;
                        } else if (fam.id === 'Amber') {
                            customClass = styles.chipAmber;
                        }

                        return (
                            <button
                                key={fam.id}
                                className={`${styles.chip} ${customClass}`}
                                onClick={() => setSelectedFamily(fam.id)}
                            >
                                {fam.label} ({fam.count})
                            </button>
                        );
                    })}
                </div>
            </section>

            {/* Browse by house */}
            <section className={styles.housesSection}>
                <div className={styles.housesHeader}>
                    <div className={styles.housesTitleGroup}>
                        <span className={styles.eyebrow}>ICONIC FRAGRANCE HOUSES</span>
                        <h2 className={styles.housesTitle}>Browse by house</h2>
                    </div>
                    <div className={styles.housesActions}>
                        {selectedHouse && (
                            <button
                                className={styles.clearHouseBtn}
                                onClick={() => updateSelectedHouse(null)}
                            >
                                Clear filter: {selectedHouse} ✕
                            </button>
                        )}
                        <button
                            className={styles.viewAllLink}
                            onClick={() => setIsAllHousesOpen((prev) => !prev)}
                        >
                            {isAllHousesOpen
                                ? 'Show fewer houses ↑'
                                : `View all ${HOUSES_DATA.length} houses →`}
                        </button>
                    </div>
                </div>

                {/* Houses cards list */}
                <div className={styles.housesGrid}>
                    {(isAllHousesOpen ? HOUSES_DATA : HOUSES_DATA.slice(0, 6)).map(
                        (house) => {
                            const isSelected = selectedHouse === house.name;
                            return (
                                <div
                                    key={house.id}
                                    className={`${styles.houseCard} ${isSelected ? styles.houseCardSelected : ''
                                        }`}
                                    onClick={() =>
                                        updateSelectedHouse(
                                            isSelected ? null : house.name
                                        )
                                    }
                                >
                                    <h3 className={styles.houseName}>
                                        {house.name}
                                    </h3>
                                    <p className={styles.houseMeta}>
                                        {house.location}
                                    </p>
                                    <span className={styles.houseBadge}>
                                        {house.count}
                                    </span>
                                    <button
                                        type="button"
                                        className={styles.exploreHouseBtn}
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            navigate(ROUTES.PERFUME_HOUSE.FULL(house.id));
                                        }}
                                    >
                                        Explore House &rarr;
                                    </button>
                                </div>
                            );
                        }
                    )}
                </div>
            </section>

            {/* The Collection / All fragrances */}
            <section
                ref={collectionRef}
                id="all-fragrances"
                className={styles.collectionSection}
            >
                <div className={styles.collectionHeader}>
                    <div className={styles.collectionTitleGroup}>
                        <span className={styles.eyebrow}>THE COLLECTION</span>
                        <h2 className={styles.collectionTitle}>
                            {selectedHouse ? `${selectedHouse} Fragrances` : 'All fragrances'}
                        </h2>
                        <p className={styles.collectionSubtitle}>
                            Click any fragrance to explore its olfactory pyramid, story, and longevity.
                        </p>
                    </div>

                    {/* Concentration and sorting filters */}
                    <div className={styles.collectionControls}>
                        <div className={styles.concentrationTabs}>
                            {CONCENTRATIONS.map((c) => (
                                <button
                                    key={c}
                                    className={`${styles.tabBtn} ${selectedConcentration === c
                                        ? styles.tabBtnActive
                                        : ''
                                        }`}
                                    onClick={() => setSelectedConcentration(c)}
                                >
                                    {c}
                                </button>
                            ))}
                        </div>
                        <button
                            className={styles.sortBtn}
                            onClick={() => setSortAscending((prev) => !prev)}
                        >
                            Sorted by: {sortAscending ? 'Rating: Low to High ↑' : 'Most Iconic ↓'}
                        </button>
                    </div>
                </div>

                {/* perfume cards grid */}
                <div className={styles.perfumesGrid}>
                    {filteredPerfumes.length === 0 ? (
                        <div className={styles.emptyState}>
                            <h3 className={styles.emptyTitle}>No fragrances found</h3>
                            <p className={styles.emptySubtitle}>
                                We couldn&apos;t find any perfumes matching your current filter combination.
                            </p>
                            <button
                                className={styles.resetBtn}
                                onClick={handleResetFilters}
                            >
                                Reset All Filters
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
                                    {/* Upper left perfume family badge and right top rating */}
                                    <div className={styles.cardTop}>
                                        <span
                                            className={`${styles.chip} ${getFamilyBadgeClass(
                                                perfume.family
                                            )}`}
                                        >
                                            {perfume.family}
                                        </span>
                                        <span className={styles.ratingBadge}>
                                            {perfume.rating} ★
                                        </span>
                                    </div>

                                    {/* hestory of perfume bottle */}
                                    <div className={styles.bottleDisplay}>
                                        <PerfumeBottle
                                            capColor={perfume.capColor}
                                            liquidColor={perfume.liquidColor}
                                            liquidHeight={perfume.liquidHeight}
                                        />
                                    </div>

                                    {/* Name and brand */}
                                    <span className={styles.cardBrand}>{perfume.brand}</span>
                                    <h3 className={styles.cardName}>{perfume.name}</h3>
                                    <p className={styles.cardMeta}>
                                        {perfume.concentration} · {perfume.year}
                                    </p>

                                    {/* Key notes */}
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

                                    {/* The Story & Vibe */}
                                    <div className={styles.storyBox}>
                                        <span className={styles.storyLabel}>
                                            THE STORY &amp; VIBE
                                        </span>
                                        <p className={styles.storyQuote}>
                                            &ldquo;{perfume.storyAndVibe}&rdquo;
                                        </p>
                                    </div>

                                    {/* Card footer */}
                                    <div className={styles.cardFooter}>
                                        <span
                                            className={styles.readMoreLink}
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                navigate(ROUTES.PERFUME_DETAIL.FULL(perfume.id));
                                            }}
                                        >
                                            Read full story &amp; notes &rarr;
                                        </span>
                                        <button
                                            className={styles.starterKitBtn}
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                alert(`Added ${perfume.name} to Starter Kit!`);
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

                {/* Load More button */}
                {filteredPerfumes.length > 0 && (
                    <div className={styles.loadMoreWrapper}>
                        <button
                            type="button"
                            className={styles.loadMoreBtn}
                            onClick={() => setIsAllPerfumesLoaded((prev) => !prev)}
                        >
                            {isAllPerfumesLoaded
                                ? `Show Featured Fragrances (Showing ${filteredPerfumes.length} of 128)`
                                : `Load More Fragrances (Showing ${filteredPerfumes.length} of 128)`}
                        </button>
                    </div>
                )}
            </section>
        </div>
    );
};
