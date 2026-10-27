import { useEffect } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { getPerfumeById, getSimilarPerfumes } from '@/pages/perfume-catalog-page/perfumes.data';
import { PerfumeBottle } from '@/pages/perfume-catalog-page/components/PerfumeBottle';
import { ROUTES } from '@/const/routes';
import styles from './PerfumeDetailPage.module.scss';

export const PerfumeDetailPage = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const perfume = getPerfumeById(id);
    const similarPerfumes = getSimilarPerfumes(perfume);

    // Scroll to top of page when switching perfumes
    useEffect(() => {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }, [id]);

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

    const handleScrollToSimilar = () => {
        const el = document.getElementById('similar-scents');
        if (el) {
            el.scrollIntoView({ behavior: 'smooth' });
        }
    };

    const handleAddToStarterKit = () => {
        alert(`Added ${perfume.name} to Starter Kit!`);
    };

    // Default dossier, if some fields are missing
    const dossier = perfume.dossier || {
        houseLocation: `${perfume.brand}`,
        perfumer: 'Master In-House Perfumer',
        releaseYear: perfume.year,
        concentration: perfume.concentration,
        olfactoryFamily: perfume.family,
        longevity: '8 – 10 hours',
        sillage: 'Moderate',
        bestSeasons: 'All seasons',
    };

    // Default pyramid of notes
    const pyramid = perfume.pyramid || {
        topNotes: {
            timing: 'First 15–30 mins · Bright opening',
            notes: perfume.keyNotes.slice(0, 2),
        },
        heartNotes: {
            timing: 'Hours 1–4 · Core aromatic composition',
            notes: perfume.keyNotes.slice(1, 3),
        },
        baseNotes: {
            timing: 'Hours 4–10 · Long-lasting skin drydown',
            notes: [perfume.keyNotes[perfume.keyNotes.length - 1], 'Cedarwood', 'Musk'],
        },
    };

    const familiesList = perfume.families && perfume.families.length > 0
        ? perfume.families
        : [perfume.family];

    return (
        <div className={styles.container}>
            {/* Breadcrumbs */}
            <nav className={styles.breadcrumbs} aria-label="Breadcrumbs">
                <Link to={ROUTES.PERFUME_CATALOG.FULL} className={styles.backLink}>
                    &larr; Back to Perfume Encyclopedia
                </Link>
                <span className={styles.crumbDivider}>/</span>
                <Link
                    to={`${ROUTES.PERFUME_CATALOG.FULL}?house=${encodeURIComponent(perfume.brand)}`}
                    className={styles.crumbBrand}
                >
                    {perfume.brand}
                </Link>
                <span className={styles.crumbDivider}>/</span>
                <span className={styles.crumbCurrent}>{perfume.name}</span>
            </nav>

            {/* card whith main information */}
            <div className={styles.detailCard}>
                <div className={styles.leftColumn}>
                    <div className={styles.bottleContainer}>
                        <PerfumeBottle
                            capColor={perfume.capColor}
                            liquidColor={perfume.liquidColor}
                            liquidHeight={perfume.liquidHeight}
                            width={170}
                            height={250}
                        />
                    </div>

                    <div className={styles.dossierBox}>
                        <span className={styles.dossierTitle}>
                            Fragrance Dossier &amp; Performance
                        </span>
                        <div className={styles.dossierList}>
                            <div className={styles.dossierRow}>
                                <span className={styles.dossierKey}>House / Brand</span>
                                <span className={styles.dossierValue}>{dossier.houseLocation}</span>
                            </div>
                            <div className={styles.dossierRow}>
                                <span className={styles.dossierKey}>Master Perfumer</span>
                                <span className={styles.dossierValue}>{dossier.perfumer}</span>
                            </div>
                            <div className={styles.dossierRow}>
                                <span className={styles.dossierKey}>Release Year</span>
                                <span className={styles.dossierValue}>{dossier.releaseYear}</span>
                            </div>
                            <div className={styles.dossierRow}>
                                <span className={styles.dossierKey}>Concentration</span>
                                <span className={styles.dossierValue}>{dossier.concentration}</span>
                            </div>
                            <div className={styles.dossierRow}>
                                <span className={styles.dossierKey}>Olfactory Family</span>
                                <span className={styles.dossierValue}>{dossier.olfactoryFamily}</span>
                            </div>
                            <div className={styles.dossierRow}>
                                <span className={styles.dossierKey}>Longevity on Skin</span>
                                <span className={styles.dossierValue}>{dossier.longevity}</span>
                            </div>
                            <div className={styles.dossierRow}>
                                <span className={styles.dossierKey}>Sillage / Projection</span>
                                <span className={styles.dossierValue}>{dossier.sillage}</span>
                            </div>
                            <div className={styles.dossierRow}>
                                <span className={styles.dossierKey}>Best Seasons</span>
                                <span className={styles.dossierValue}>{dossier.bestSeasons}</span>
                            </div>
                        </div>
                    </div>
                </div>

                {/* right column: description, pyramid of notes, action buttons */}
                <div className={styles.rightColumn}>
                    <span className={styles.brandEyebrow}>
                        {perfume.brand} &mdash; {perfume.concentration.toUpperCase()}
                    </span>
                    <h1 className={styles.perfumeName}>{perfume.name}</h1>

                    <div className={styles.badgesRow}>
                        <span className={styles.matchBadge}>
                            {perfume.matchPercentage || 96}% Match with your profile
                        </span>
                        {familiesList.map((fam) => (
                            <span
                                key={fam}
                                className={`${styles.familyChip} ${getFamilyBadgeClass(fam)}`}
                            >
                                {fam}
                            </span>
                        ))}
                        <span className={styles.ratingBadge}>
                            <span className={styles.starIcon}>★</span> {perfume.rating} ({perfume.reviewsCount || 342} reviews)
                        </span>
                    </div>

                    {/* About this fragrance */}
                    <span className={styles.sectionLabel}>About this fragrance</span>
                    <p className={styles.aboutText}>
                        {perfume.about || perfume.storyAndVibe}
                    </p>

                    {/*  Fragrance Vibe & Atmosphere */}
                    <div className={styles.vibeCard}>
                        <span className={styles.vibeLabel}>Fragrance Vibe &amp; Atmosphere</span>
                        <p className={styles.vibeQuote}>
                            {perfume.vibeQuote || `“${perfume.storyAndVibe}”`}
                        </p>
                    </div>

                    {/* Note Breakdown */}
                    <div className={styles.pyramidSection}>
                        <span className={styles.sectionLabel}>
                            Olfactory Pyramid (Note Breakdown)
                        </span>
                        <div className={styles.pyramidStack}>
                            {/* Top Notes */}
                            <div className={styles.pyramidCard}>
                                <div className={styles.pyramidCardHeader}>
                                    <span className={styles.pyramidStageName}>Top Notes</span>
                                    <span className={styles.pyramidStageTiming}>
                                        {pyramid.topNotes.timing}
                                    </span>
                                </div>
                                <div className={styles.pyramidNotesList}>
                                    {pyramid.topNotes.notes.map((note) => (
                                        <span
                                            key={note}
                                            className={`${styles.pyramidNoteChip} ${styles.topNoteChip}`}
                                        >
                                            {note}
                                        </span>
                                    ))}
                                </div>
                            </div>

                            {/* Heart Notes */}
                            <div className={styles.pyramidCard}>
                                <div className={styles.pyramidCardHeader}>
                                    <span className={styles.pyramidStageName}>Heart Notes</span>
                                    <span className={styles.pyramidStageTiming}>
                                        {pyramid.heartNotes.timing}
                                    </span>
                                </div>
                                <div className={styles.pyramidNotesList}>
                                    {pyramid.heartNotes.notes.map((note) => (
                                        <span
                                            key={note}
                                            className={`${styles.pyramidNoteChip} ${styles.heartNoteChip}`}
                                        >
                                            {note}
                                        </span>
                                    ))}
                                </div>
                            </div>

                            {/* Base Notes */}
                            <div className={styles.pyramidCard}>
                                <div className={styles.pyramidCardHeader}>
                                    <span className={styles.pyramidStageName}>Base Notes</span>
                                    <span className={styles.pyramidStageTiming}>
                                        {pyramid.baseNotes.timing}
                                    </span>
                                </div>
                                <div className={styles.pyramidNotesList}>
                                    {pyramid.baseNotes.notes.map((note) => (
                                        <span
                                            key={note}
                                            className={`${styles.pyramidNoteChip} ${styles.baseNoteChip}`}
                                        >
                                            {note}
                                        </span>
                                    ))}
                                </div>
                            </div>
                        </div>
                    </div>

                    {/* buttons for adding to the starter kit and finding similar fragrances */}
                    <div className={styles.actionsRow}>
                        <button
                            type="button"
                            className={styles.starterKitBtn}
                            onClick={handleAddToStarterKit}
                        >
                            + Add to My Starter Kit (3 / 5)
                        </button>
                        <button
                            type="button"
                            className={styles.similarBtn}
                            onClick={handleScrollToSimilar}
                        >
                            Find Similar Scents &rarr;
                        </button>
                    </div>
                </div>
            </div>

            {/* section similar scents in the archive */}
            <section id="similar-scents" className={styles.similarSection}>
                <span className={styles.similarEyebrow}>
                    IF YOU LOVE {perfume.name.toUpperCase()}
                </span>
                <h2 className={styles.similarTitle}>Similar scents in the archive</h2>

                <div className={styles.similarGrid}>
                    {similarPerfumes.map((simPerfume) => (
                        <div
                            key={simPerfume.id}
                            className={styles.similarCard}
                            onClick={() => navigate(ROUTES.PERFUME_DETAIL.FULL(simPerfume.id))}
                        >
                            <div className={styles.similarBottleWrapper}>
                                <PerfumeBottle
                                    capColor={simPerfume.capColor}
                                    liquidColor={simPerfume.liquidColor}
                                    liquidHeight={simPerfume.liquidHeight}
                                    width={74}
                                    height={108}
                                />
                            </div>

                            <span className={styles.cardBrand}>{simPerfume.brand}</span>
                            <h3 className={styles.cardName}>{simPerfume.name}</h3>
                            <p className={styles.cardMeta}>
                                {simPerfume.concentration} &middot; {simPerfume.year}
                            </p>

                            <div className={styles.notesBlock}>
                                <div className={styles.notesChips}>
                                    {simPerfume.keyNotes.map((note) => (
                                        <span key={note} className={styles.noteItem}>
                                            {note}
                                        </span>
                                    ))}
                                </div>
                            </div>

                            <div className={styles.storyBox}>
                                <p className={styles.storyQuote}>
                                    &ldquo;{simPerfume.storyAndVibe}&rdquo;
                                </p>
                            </div>

                            <div className={styles.cardFooter}>
                                <span className={styles.readMoreLink}>
                                    Read full story &amp; notes &rarr;
                                </span>
                            </div>
                        </div>
                    ))}
                </div>
            </section>
        </div>
    );
};
