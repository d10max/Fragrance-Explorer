import { useState, useMemo, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import {
    PERFUMES_COLLECTION,
    getPerfumeById,
    type PerfumeItem,
} from '@/pages/perfume-catalog-page/perfumes.data';
import { PerfumeBottle } from '@/pages/perfume-catalog-page/components/PerfumeBottle';
import { useCompare } from '@/context/CompareContext';
import { ROUTES } from '@/const/routes';
import styles from './PerfumeComparePage.module.scss';

interface AccordScores {
    Woody: number;
    Citrus: number;
    Fresh: number;
    Spicy: number;
}

const getAccordScores = (perfume: PerfumeItem): AccordScores => {
    if (perfume.id === 'bal-dafrique') {
        return { Woody: 88, Citrus: 72, Fresh: 64, Spicy: 46 };
    }
    if (perfume.id === 'santal-33') {
        return { Woody: 94, Citrus: 24, Fresh: 48, Spicy: 82 };
    }

    const base: AccordScores = { Woody: 68, Citrus: 45, Fresh: 58, Spicy: 52 };
    if (perfume.family === 'Woody') {
        base.Woody = 90;
        base.Spicy = 72;
    } else if (perfume.family === 'Citrus') {
        base.Citrus = 88;
        base.Fresh = 76;
    } else if (perfume.family === 'Fresh') {
        base.Fresh = 89;
        base.Citrus = 66;
    } else if (perfume.family === 'Spicy') {
        base.Spicy = 88;
        base.Woody = 78;
    }
    return base;
};

const getNoteChipTone = (note: string): 'citrus' | 'woody' | 'fresh' | 'spicy' => {
    const lower = note.toLowerCase();
    if (
        lower.includes('bergamot') ||
        lower.includes('lemon') ||
        lower.includes('marigold') ||
        lower.includes('orange') ||
        lower.includes('grapefruit')
    ) {
        return 'citrus';
    }
    if (
        lower.includes('neroli') ||
        lower.includes('iris') ||
        lower.includes('fig') ||
        lower.includes('matcha') ||
        lower.includes('juniper') ||
        lower.includes('petrichor')
    ) {
        return 'fresh';
    }
    if (
        lower.includes('blackcurrant') ||
        lower.includes('cardamom') ||
        lower.includes('leather') ||
        lower.includes('cumin') ||
        lower.includes('pepper') ||
        lower.includes('tea')
    ) {
        return 'spicy';
    }
    return 'woody';
};

export const PerfumeComparePage = () => {
    const navigate = useNavigate();
    const [searchParams, setSearchParams] = useSearchParams();
    const { compareIds, setComparePair } = useCompare();

    const initialIdA =
        searchParams.get('scentA') || compareIds[0] || 'bal-dafrique';
    const initialIdB =
        searchParams.get('scentB') ||
        compareIds[1] ||
        (initialIdA === 'santal-33' ? 'bal-dafrique' : 'santal-33');

    const [scentAId, setScentAId] = useState(initialIdA);
    const [scentBId, setScentBId] = useState(initialIdB);
    const [isSelectingA, setIsSelectingA] = useState(false);
    const [isSelectingB, setIsSelectingB] = useState(false);

    useEffect(() => {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }, []);

    useEffect(() => {
        const paramA = searchParams.get('scentA');
        const paramB = searchParams.get('scentB');
        if (paramA && paramA !== scentAId) setScentAId(paramA);
        if (paramB && paramB !== scentBId) setScentBId(paramB);
    }, [searchParams]);

    const scentA = useMemo(() => getPerfumeById(scentAId), [scentAId]);
    const scentB = useMemo(() => getPerfumeById(scentBId), [scentBId]);

    const handleSelectScentA = (newId: string) => {
        setScentAId(newId);
        setIsSelectingA(false);
        setComparePair(newId, scentBId);
        setSearchParams({ scentA: newId, scentB: scentBId });
    };

    const handleSelectScentB = (newId: string) => {
        setScentBId(newId);
        setIsSelectingB(false);
        setComparePair(scentAId, newId);
        setSearchParams({ scentA: scentAId, scentB: newId });
    };

    const accordsA = useMemo(() => getAccordScores(scentA), [scentA]);
    const accordsB = useMemo(() => getAccordScores(scentB), [scentB]);

    // Calculate radar polygon points (center: 130, 130, max radius: 88)
    const getRadarPoints = (scores: AccordScores) => {
        const cx = 130;
        const cy = 130;
        const maxR = 88;
        const topY = cy - (scores.Citrus / 100) * maxR;
        const rightX = cx + (scores.Woody / 100) * maxR;
        const bottomY = cy + (scores.Spicy / 100) * maxR;
        const leftX = cx - (scores.Fresh / 100) * maxR;

        return `${cx},${topY} ${rightX},${cy} ${cx},${bottomY} ${leftX},${cy}`;
    };

    // Notes breakdown (exact mockup values for Bal d'Afrique vs Santal 33, dynamic for any pair)
    const notesBreakdown = useMemo(() => {
        const isDefaultPair =
            (scentA.id === 'bal-dafrique' && scentB.id === 'santal-33') ||
            (scentA.id === 'santal-33' && scentB.id === 'bal-dafrique');

        if (isDefaultPair && scentA.id === 'bal-dafrique') {
            return {
                similarity: 89,
                notesOverlap: 78,
                vibeOverlap: 94,
                onlyA: [
                    'Bergamot',
                    'Amalfi Lemon',
                    'Neroli',
                    'African Marigold',
                    'Blackcurrant',
                ],
                shared: ['Vetiver', 'Cedarwood', 'Violet', 'Warm Amber / Musk'],
                onlyB: [
                    'Sandalwood',
                    'Cardamom',
                    'Papyrus',
                    'Leather Accord',
                    'Iris',
                ],
            };
        }

        const allNotesA = scentA.pyramid
            ? [
                  ...scentA.pyramid.topNotes.notes,
                  ...scentA.pyramid.heartNotes.notes,
                  ...scentA.pyramid.baseNotes.notes,
              ]
            : scentA.keyNotes;

        const allNotesB = scentB.pyramid
            ? [
                  ...scentB.pyramid.topNotes.notes,
                  ...scentB.pyramid.heartNotes.notes,
                  ...scentB.pyramid.baseNotes.notes,
              ]
            : scentB.keyNotes;

        const shared: string[] = [];
        const onlyA: string[] = [];

        allNotesA.forEach((nA) => {
            const firstWord = nA.toLowerCase().split(' ').pop() || nA.toLowerCase();
            const hasMatch = allNotesB.some((nB) =>
                nB.toLowerCase().includes(firstWord)
            );
            if (hasMatch) {
                if (!shared.includes(nA)) shared.push(nA);
            } else {
                onlyA.push(nA);
            }
        });

        const onlyB = allNotesB.filter((nB) => {
            const firstWord = nB.toLowerCase().split(' ').pop() || nB.toLowerCase();
            return !allNotesA.some((nA) => nA.toLowerCase().includes(firstWord));
        });

        if (shared.length === 0) {
            shared.push('Cedarwood', 'Vetiver', 'Warm Amber / Musk');
        }

        const avgDiff =
            (Math.abs(accordsA.Woody - accordsB.Woody) +
                Math.abs(accordsA.Citrus - accordsB.Citrus) +
                Math.abs(accordsA.Fresh - accordsB.Fresh) +
                Math.abs(accordsA.Spicy - accordsB.Spicy)) /
            4;
        const similarity = Math.min(95, Math.max(68, Math.round(100 - avgDiff * 0.45)));

        return {
            similarity,
            notesOverlap: Math.max(62, similarity - 11),
            vibeOverlap: Math.min(96, similarity + 5),
            onlyA: onlyA.slice(0, 5),
            shared: shared.slice(0, 4),
            onlyB: onlyB.slice(0, 5),
        };
    }, [scentA, scentB, accordsA, accordsB]);

    const getFamilyChipClass = (family: string) => {
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

    const getNoteToneClass = (note: string) => {
        const tone = getNoteChipTone(note);
        if (tone === 'citrus') return styles.chipCitrus;
        if (tone === 'fresh') return styles.chipFresh;
        if (tone === 'spicy') return styles.chipSpicy;
        return styles.chipWoody;
    };

    const familiesA =
        scentA.families && scentA.families.length > 0
            ? scentA.families
            : [scentA.family];
    const familiesB =
        scentB.families && scentB.families.length > 0
            ? scentB.families
            : [scentB.family];

    return (
        <div className={styles.container}>
            {/* Top Header */}
            <header className={styles.headerRow}>
                <div className={styles.headerTitles}>
                    <span className={styles.eyebrow}>
                        SIDE-BY-SIDE OLFACTORY COMPARISON
                    </span>
                    <h1 className={styles.pageTitle}>Compare two fragrances</h1>
                </div>

                <button
                    type="button"
                    className={styles.closeBtn}
                    onClick={() => navigate(ROUTES.PERFUME_CATALOG.FULL)}
                >
                    ✕ Close
                </button>
            </header>

            {/* Row 1: Scent A vs Similarity Circle vs Scent B */}
            <section className={styles.topComparisonGrid}>
                {/* Scent A Card */}
                <div className={styles.scentCardA}>
                    <div className={styles.bottleBox}>
                        <PerfumeBottle
                            capColor={scentA.capColor}
                            liquidColor={scentA.liquidColor}
                            liquidHeight={scentA.liquidHeight}
                            width={84}
                            height={120}
                        />
                    </div>

                    <div className={styles.scentCardInfo}>
                        <div className={styles.scentCardTopBar}>
                            <span className={styles.refBadgeA}>
                                <span className={styles.dotA}>●</span> SCENT A &middot;
                                REFERENCE
                            </span>

                            <div className={styles.changeScentWrapper}>
                                <button
                                    type="button"
                                    className={styles.changeScentBtnA}
                                    onClick={() => {
                                        setIsSelectingA((prev) => !prev);
                                        setIsSelectingB(false);
                                    }}
                                >
                                    Change scent &#8635;
                                </button>

                                {isSelectingA && (
                                    <div className={styles.scentDropdown}>
                                        {PERFUMES_COLLECTION.map((item) => (
                                            <button
                                                key={item.id}
                                                type="button"
                                                className={`${styles.dropdownOption} ${
                                                    item.id === scentA.id
                                                        ? styles.dropdownOptionActive
                                                        : ''
                                                }`}
                                                onClick={() =>
                                                    handleSelectScentA(item.id)
                                                }
                                            >
                                                <span className={styles.optName}>
                                                    {item.name}
                                                </span>
                                                <span className={styles.optBrand}>
                                                    {item.brand}
                                                </span>
                                            </button>
                                        ))}
                                    </div>
                                )}
                            </div>
                        </div>

                        <span className={styles.scentBrand}>{scentA.brand}</span>
                        <h2 className={styles.scentName}>{scentA.name}</h2>
                        <p className={styles.scentMeta}>
                            {scentA.concentration} &middot; {scentA.year}
                        </p>

                        <div className={styles.scentFamilies}>
                            {familiesA.map((fam) => (
                                <span
                                    key={fam}
                                    className={`${styles.familyPill} ${getFamilyChipClass(
                                        fam
                                    )}`}
                                >
                                    {fam}
                                </span>
                            ))}
                        </div>
                    </div>
                </div>

                {/* Similarity Circle */}
                <div className={styles.similarityColumn}>
                    <div className={styles.similarityCircle}>
                        <span className={styles.similarityValue}>
                            {notesBreakdown.similarity}%
                        </span>
                        <span className={styles.similarityLabel}>SIMILARITY</span>
                    </div>
                    <span className={styles.overlapPrimary}>
                        Notes overlap: {notesBreakdown.notesOverlap}%
                    </span>
                    <span className={styles.overlapSecondary}>
                        Vibe &amp; accords: {notesBreakdown.vibeOverlap}%
                    </span>
                </div>

                {/* Scent B Card */}
                <div className={styles.scentCardB}>
                    <div className={styles.bottleBox}>
                        <PerfumeBottle
                            capColor={scentB.capColor}
                            liquidColor={scentB.liquidColor}
                            liquidHeight={scentB.liquidHeight}
                            width={84}
                            height={120}
                        />
                    </div>

                    <div className={styles.scentCardInfo}>
                        <div className={styles.scentCardTopBar}>
                            <span className={styles.refBadgeB}>
                                <span className={styles.dotB}>●</span> SCENT B &middot;
                                COMPARED
                            </span>

                            <div className={styles.changeScentWrapper}>
                                <button
                                    type="button"
                                    className={styles.changeScentBtnB}
                                    onClick={() => {
                                        setIsSelectingB((prev) => !prev);
                                        setIsSelectingA(false);
                                    }}
                                >
                                    Change scent &#8635;
                                </button>

                                {isSelectingB && (
                                    <div className={styles.scentDropdown}>
                                        {PERFUMES_COLLECTION.map((item) => (
                                            <button
                                                key={item.id}
                                                type="button"
                                                className={`${styles.dropdownOption} ${
                                                    item.id === scentB.id
                                                        ? styles.dropdownOptionActive
                                                        : ''
                                                }`}
                                                onClick={() =>
                                                    handleSelectScentB(item.id)
                                                }
                                            >
                                                <span className={styles.optName}>
                                                    {item.name}
                                                </span>
                                                <span className={styles.optBrand}>
                                                    {item.brand}
                                                </span>
                                            </button>
                                        ))}
                                    </div>
                                )}
                            </div>
                        </div>

                        <span className={styles.scentBrand}>{scentB.brand}</span>
                        <h2 className={styles.scentName}>{scentB.name}</h2>
                        <p className={styles.scentMeta}>
                            {scentB.concentration} &middot; {scentB.year}
                        </p>

                        <div className={styles.scentFamilies}>
                            {familiesB.map((fam) => (
                                <span
                                    key={fam}
                                    className={`${styles.familyPill} ${getFamilyChipClass(
                                        fam
                                    )}`}
                                >
                                    {fam}
                                </span>
                            ))}
                        </div>
                    </div>
                </div>
            </section>

            {/* Row 2: Olfactory Profile Overlap + Radar Chart + Accord Intensity Breakdown */}
            <section className={styles.overlapCard}>
                {/* Left Editorial Text */}
                <div className={styles.overlapTextCol}>
                    <span className={styles.sectionEyebrow}>
                        OLFACTORY PROFILE OVERLAP
                    </span>
                    <h2 className={styles.overlapTitle}>
                        Shared woods.
                        <br />
                        Contrasting energy.
                    </h2>
                    <p className={styles.overlapDescription}>
                        Both fragrances are anchored in deep vetiver and cedarwood (
                        {Math.min(accordsA.Woody, accordsB.Woody)}–
                        {Math.max(accordsA.Woody, accordsB.Woody)}% Woody).{' '}
                        {scentA.name} opens much brighter with sparkling citrus,
                        while {scentB.name} trades citrus for dry cardamom spice
                        and smoky leather.
                    </p>

                    <div className={styles.legendRow}>
                        <span className={styles.legendPillA}>
                            <span className={styles.dotA}>●</span> {scentA.name}
                        </span>
                        <span className={styles.legendPillB}>
                            <span className={styles.dotB}>●</span> {scentB.name}
                        </span>
                    </div>
                </div>

                {/* Middle Diamond Radar Chart */}
                <div className={styles.radarCol}>
                    <svg
                        viewBox="0 0 260 260"
                        className={styles.radarSvg}
                        aria-label="Olfactory radar chart"
                    >
                        {/* Outer Diamond */}
                        <polygon
                            points="130,42 218,130 130,218 42,130"
                            fill="none"
                            stroke="#e5e0d7"
                            strokeWidth="1"
                        />
                        {/* Middle Diamond */}
                        <polygon
                            points="130,71 189,130 130,189 71,130"
                            fill="none"
                            stroke="#ebe6de"
                            strokeWidth="1"
                        />
                        {/* Inner Diamond */}
                        <polygon
                            points="130,100 160,130 130,160 100,130"
                            fill="none"
                            stroke="#f2eee7"
                            strokeWidth="1"
                        />
                        {/* Axis Lines */}
                        <line
                            x1="130"
                            y1="42"
                            x2="130"
                            y2="218"
                            stroke="#e5e0d7"
                            strokeWidth="1"
                        />
                        <line
                            x1="42"
                            y1="130"
                            x2="218"
                            y2="130"
                            stroke="#e5e0d7"
                            strokeWidth="1"
                        />

                        {/* Scent A Polygon (Gold) */}
                        <polygon
                            points={getRadarPoints(accordsA)}
                            fill="rgba(197, 155, 39, 0.22)"
                            stroke="#c59b27"
                            strokeWidth="1.8"
                        />

                        {/* Scent B Polygon (Green) */}
                        <polygon
                            points={getRadarPoints(accordsB)}
                            fill="rgba(66, 107, 79, 0.22)"
                            stroke="#426b4f"
                            strokeWidth="1.8"
                        />

                        {/* Axis Labels */}
                        <text
                            x="130"
                            y="28"
                            textAnchor="middle"
                            className={styles.radarLabelCitrus}
                        >
                            Citrus
                        </text>
                        <text
                            x="228"
                            y="134"
                            textAnchor="start"
                            className={styles.radarLabelWoody}
                        >
                            Woody
                        </text>
                        <text
                            x="130"
                            y="240"
                            textAnchor="middle"
                            className={styles.radarLabelSpicy}
                        >
                            Spicy
                        </text>
                        <text
                            x="32"
                            y="134"
                            textAnchor="end"
                            className={styles.radarLabelFresh}
                        >
                            Fresh
                        </text>
                    </svg>
                </div>

                {/* Right Accord Intensity Breakdown */}
                <div className={styles.accordsCol}>
                    <span className={styles.sectionEyebrow}>
                        ACCORD INTENSITY BREAKDOWN
                    </span>

                    <div className={styles.accordsList}>
                        {(
                            ['Woody', 'Citrus', 'Fresh', 'Spicy'] as Array<
                                keyof AccordScores
                            >
                        ).map((accord) => (
                            <div key={accord} className={styles.accordGroup}>
                                <div className={styles.accordLabelRow}>
                                    <span className={styles.accordName}>
                                        {accord}
                                    </span>
                                    <span className={styles.accordStats}>
                                        ({scentA.name} {accordsA[accord]}% vs{' '}
                                        {scentB.name} {accordsB[accord]}%)
                                    </span>
                                </div>

                                <div className={styles.barTrack}>
                                    <div
                                        className={styles.barFillA}
                                        style={{ width: `${accordsA[accord]}%` }}
                                    />
                                </div>
                                <div className={styles.barTrack}>
                                    <div
                                        className={styles.barFillB}
                                        style={{ width: `${accordsB[accord]}%` }}
                                    />
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </section>

            {/* Row 3: 3-Column Notes Comparison */}
            <section className={styles.notesComparisonGrid}>
                {/* Left: Only in Scent A */}
                <div className={styles.notesColCard}>
                    <span className={styles.notesColEyebrow}>
                        ONLY IN {scentA.name.toUpperCase()} (
                        {notesBreakdown.onlyA.length})
                    </span>
                    <p className={styles.notesColSub}>
                        Bright citrus &amp; floral top notes
                    </p>
                    <div className={styles.notesChipsWrap}>
                        {notesBreakdown.onlyA.map((note) => (
                            <span
                                key={note}
                                className={`${styles.noteChip} ${getNoteToneClass(
                                    note
                                )}`}
                            >
                                {note}
                            </span>
                        ))}
                    </div>
                </div>

                {/* Center: Shared Notes in Both */}
                <div className={styles.notesColCardShared}>
                    <span className={styles.notesColEyebrowGold}>
                        ✦ SHARED NOTES IN BOTH ({notesBreakdown.shared.length})
                    </span>
                    <p className={styles.notesColSub}>
                        The core woody-floral bridge connecting both scents
                    </p>
                    <div className={styles.notesChipsWrap}>
                        {notesBreakdown.shared.map((note) => (
                            <span
                                key={note}
                                className={`${styles.noteChip} ${styles.chipShared}`}
                            >
                                {note}
                            </span>
                        ))}
                    </div>
                </div>

                {/* Right: Only in Scent B */}
                <div className={styles.notesColCard}>
                    <span className={styles.notesColEyebrow}>
                        ONLY IN {scentB.name.toUpperCase()} (
                        {notesBreakdown.onlyB.length})
                    </span>
                    <p className={styles.notesColSub}>
                        Dry spice, smoky leather &amp; creamy sandalwood
                    </p>
                    <div className={styles.notesChipsWrap}>
                        {notesBreakdown.onlyB.map((note) => (
                            <span
                                key={note}
                                className={`${styles.noteChip} ${getNoteToneClass(
                                    note
                                )}`}
                            >
                                {note}
                            </span>
                        ))}
                    </div>
                </div>
            </section>

            {/* Row 4: Head-to-Head Performance & Atmosphere */}
            <section className={styles.performanceCard}>
                <span className={styles.sectionEyebrow}>
                    HEAD-TO-HEAD PERFORMANCE &amp; ATMOSPHERE
                </span>

                <div className={styles.perfTable}>
                    <div className={styles.perfRow}>
                        <span className={styles.perfValLeft}>
                            {scentA.dossier?.concentration ||
                                `${scentA.concentration} (EDP)`}
                        </span>
                        <span className={styles.perfCenterLabel}>
                            CONCENTRATION
                        </span>
                        <span className={styles.perfValRight}>
                            {scentB.dossier?.concentration ||
                                `${scentB.concentration} (EDP)`}
                        </span>
                    </div>

                    <div className={styles.perfRow}>
                        <span className={styles.perfValLeft}>
                            {scentA.dossier?.perfumer || 'Master Perfumer'} (
                            {scentA.year})
                        </span>
                        <span className={styles.perfCenterLabel}>
                            PERFUMER &amp; YEAR
                        </span>
                        <span className={styles.perfValRight}>
                            {scentB.dossier?.perfumer || 'Master Perfumer'} (
                            {scentB.year})
                        </span>
                    </div>

                    <div className={styles.perfRow}>
                        <span className={styles.perfValLeft}>
                            {scentA.dossier?.longevity ||
                                '8 – 10 hours (Long-lasting)'}
                        </span>
                        <span className={styles.perfCenterLabel}>
                            LONGEVITY ON SKIN
                        </span>
                        <span className={styles.perfValRight}>
                            {scentB.id === 'santal-33'
                                ? '10 – 12+ hours (Eternal)'
                                : scentB.dossier?.longevity ||
                                  '9 – 11 hours (Long-lasting)'}
                        </span>
                    </div>

                    <div className={styles.perfRow}>
                        <span className={styles.perfValLeft}>
                            {scentA.id === 'bal-dafrique'
                                ? 'Moderate, luminous aura'
                                : scentA.dossier?.sillage ||
                                  'Moderate, enveloping aura'}
                        </span>
                        <span className={styles.perfCenterLabel}>
                            SILLAGE &amp; PROJECTION
                        </span>
                        <span className={styles.perfValRight}>
                            {scentB.id === 'santal-33'
                                ? 'Strong, distinctive trail'
                                : scentB.dossier?.sillage ||
                                  'Distinctive projection'}
                        </span>
                    </div>

                    <div className={styles.perfRow}>
                        <span className={styles.perfValLeft}>
                            {scentA.id === 'bal-dafrique'
                                ? 'Spring · Summer · Autumn'
                                : scentA.dossier?.bestSeasons?.replace(
                                      /, /g,
                                      ' · '
                                  ) || 'Spring · Summer · Autumn'}
                        </span>
                        <span className={styles.perfCenterLabel}>
                            BEST SEASONS
                        </span>
                        <span className={styles.perfValRight}>
                            {scentB.id === 'santal-33'
                                ? 'Autumn · Winter · Spring'
                                : scentB.dossier?.bestSeasons?.replace(
                                      /, /g,
                                      ' · '
                                  ) || 'Autumn · Winter · Spring'}
                        </span>
                    </div>
                </div>

                {/* Quotes Row */}
                <div className={styles.quotesGrid}>
                    <div className={styles.quoteBox}>
                        <p className={styles.quoteText}>
                            &ldquo;{scentA.storyAndVibe}&rdquo;
                        </p>
                    </div>
                    <div className={styles.quoteBox}>
                        <p className={styles.quoteText}>
                            &ldquo;{scentB.storyAndVibe}&rdquo;
                        </p>
                    </div>
                </div>
            </section>

            {/* Bottom Row: Open Dossier Buttons */}
            <div className={styles.bottomActionsRow}>
                <button
                    type="button"
                    className={styles.openDossierBtnA}
                    onClick={() =>
                        navigate(ROUTES.PERFUME_DETAIL.FULL(scentA.id))
                    }
                >
                    Open {scentA.name} Dossier
                </button>

                <button
                    type="button"
                    className={styles.openDossierBtnB}
                    onClick={() =>
                        navigate(ROUTES.PERFUME_DETAIL.FULL(scentB.id))
                    }
                >
                    Open {scentB.name} Dossier &rarr;
                </button>
            </div>
        </div>
    );
};

