import { useState } from 'react';
import {
    PieChart,
    Pie,
    Cell,
    RadarChart,
    PolarGrid,
    PolarAngleAxis,
    PolarRadiusAxis,
    Radar,
    Tooltip,
    ResponsiveContainer,
} from 'recharts';
import { Sparkles } from 'lucide-react';
import { useProfile, type FamilyStat, type RadarDimension } from '@/context/ProfileContext';
import styles from './ScentTasteChart.module.scss';

const TEASER_FAMILIES: FamilyStat[] = [
    {
        name: 'Woody',
        count: 3,
        percentage: 50,
        color: '#c59b27',
        bgBadge: '#eee9e2',
        textBadge: '#8b7963',
        description: 'Cedarwood, Sandalwood, Vetiver & Dry Woods',
    },
    {
        name: 'Citrus',
        count: 1,
        percentage: 17,
        color: '#d4a32e',
        bgBadge: '#f5eed9',
        textBadge: '#b89737',
        description: 'Bergamot, Bitter Orange, Neroli & Zest',
    },
    {
        name: 'Fresh',
        count: 1,
        percentage: 17,
        color: '#77958a',
        bgBadge: '#eaf0ec',
        textBadge: '#77958a',
        description: 'Cardamom, Green Fig, Aquatic & Herbal Notes',
    },
    {
        name: 'Spicy',
        count: 1,
        percentage: 16,
        color: '#b67c67',
        bgBadge: '#f5e7e1',
        textBadge: '#b67c67',
        description: 'Pink Pepper, Tonka Bean, Saffron & Warm Amber',
    },
];

const TEASER_RADAR: RadarDimension[] = [
    { label: 'Woody & Earthy', value: 85, maxValue: 100 },
    { label: 'Citrus & Bright', value: 68, maxValue: 100 },
    { label: 'Fresh Green', value: 62, maxValue: 100 },
    { label: 'Resinous Amber', value: 58, maxValue: 100 },
    { label: 'Spicy Warmth', value: 48, maxValue: 100 },
    { label: 'Soft Floral', value: 32, maxValue: 100 },
];

export const ScentTasteChart = () => {
    const { analytics } = useProfile();
    const { hasData, families, topNotes, radarData, vibePersona, vibeDescription } =
        analytics;

    const displayFamilies = hasData ? families : TEASER_FAMILIES;
    const displayRadar = hasData ? radarData : TEASER_RADAR;

    const [activeFamily, setActiveFamily] = useState<FamilyStat | null>(null);
    const [hoveredNote, setHoveredNote] = useState<string | null>(null);
    const [activeRadarLabel, setActiveRadarLabel] = useState<string | null>(null);

    const activeFamiliesData = displayFamilies.filter((f) => f.percentage > 0);
    const highlightedFamily =
        activeFamily ||
        displayFamilies.find((f) => f.percentage > 0) ||
        displayFamilies[0];

    const topNotePair =
        topNotes.length >= 2
            ? `${topNotes[0].name} & ${topNotes[1].name}`
            : topNotes[0]?.name || 'Moroccan Cedar & Vetiver';

    const radarChartData = displayRadar.map((dim) => ({
        ...dim,
        axisLabel: `${dim.label} (${dim.value}%)`,
    }));

    const activeRadarDim = activeRadarLabel
        ? displayRadar.find((d) => d.label === activeRadarLabel) || null
        : null;

    return (
        <section className={styles.chartsContainer}>
            {/* Olfactory Persona Banner */}
            <div className={styles.personaBanner}>
                <div className={styles.personaContent}>
                    <span className={styles.personaBadge}>YOUR SCENT ARCHETYPE</span>
                    <h3 className={styles.personaTitle}>&ldquo;{vibePersona}&rdquo;</h3>
                    <p className={styles.personaDesc}>{vibeDescription}</p>
                </div>
                <div className={styles.personaTags}>
                    <div className={styles.personaPill}>
                        <span className={styles.pillLabel}>Dominant Signature</span>
                        <span className={styles.pillVal}>{topNotePair}</span>
                    </div>
                    <div className={styles.personaPill}>
                        <span className={styles.pillLabel}>Atmosphere &amp; Mood</span>
                        <span className={styles.pillVal}>Quiet Elegance, Sunlit Woods</span>
                    </div>
                    <div className={styles.personaPill}>
                        <span className={styles.pillLabel}>Concentration Bias</span>
                        <span className={styles.pillVal}>Extrait &amp; EDP (86%)</span>
                    </div>
                </div>
            </div>

            {/* Grid of the 2 primary visual diagrams (powered by Recharts) */}
            <div className={styles.diagramsGrid}>
                {/* 1. Fragrance Families Donut Chart (Recharts PieChart) */}
                <div className={styles.diagramCard}>
                    <div className={styles.cardHeader}>
                        <div>
                            <span className={styles.eyebrow}>SCENT WHEEL RATIO</span>
                            <h4 className={styles.cardTitle}>
                                Olfactory Families Distribution
                            </h4>
                        </div>
                        <span className={styles.metaHint}>
                            Hover over slices or families
                        </span>
                    </div>

                    <div className={styles.donutLayout}>
                        <div className={styles.donutSvgWrapper}>
                            <PieChart width={240} height={240}>
                                <Pie
                                    data={activeFamiliesData}
                                    dataKey="percentage"
                                    nameKey="name"
                                    cx="50%"
                                    cy="50%"
                                    innerRadius={64}
                                    outerRadius={92}
                                    paddingAngle={activeFamiliesData.length > 1 ? 2 : 0}
                                    startAngle={90}
                                    endAngle={-270}
                                    stroke="none"
                                    onMouseEnter={(_, index) => {
                                        const hovered = activeFamiliesData[index];
                                        if (hovered) setActiveFamily(hovered);
                                    }}
                                    onMouseLeave={() => setActiveFamily(null)}
                                >
                                    {activeFamiliesData.map((entry) => {
                                        const isCurrent =
                                            activeFamily?.name === entry.name;
                                        return (
                                            <Cell
                                                key={entry.name}
                                                fill={entry.color}
                                                opacity={
                                                    activeFamily && !isCurrent
                                                        ? 0.38
                                                        : 1
                                                }
                                                style={{
                                                    cursor: 'pointer',
                                                    transition: 'opacity 0.2s ease',
                                                    outline: 'none',
                                                }}
                                            />
                                        );
                                    })}
                                </Pie>
                            </PieChart>

                            {/* Centered HTML overlay inside Donut hole */}
                            <div className={styles.donutCenterOverlay}>
                                <span className={styles.donutCenterPct}>
                                    {highlightedFamily.percentage}%
                                </span>
                                <span className={styles.donutCenterLabel}>
                                    {highlightedFamily.name}
                                </span>
                                <span className={styles.donutCenterSub}>
                                    {highlightedFamily.count} saved scents
                                </span>
                            </div>
                        </div>

                        {/* Interactive Family Legend */}
                        <div className={styles.familyLegend}>
                            {displayFamilies.map((fam) => {
                                const isCurrent = activeFamily?.name === fam.name;
                                return (
                                    <div
                                        key={fam.name}
                                        className={`${styles.legendItem} ${
                                            isCurrent ? styles.legendItemActive : ''
                                        }`}
                                        onMouseEnter={() => setActiveFamily(fam)}
                                        onMouseLeave={() => setActiveFamily(null)}
                                    >
                                        <div
                                            className={styles.legendColorLine}
                                            style={{ backgroundColor: fam.color }}
                                        />
                                        <div className={styles.legendDetails}>
                                            <div className={styles.legendTop}>
                                                <span className={styles.legendName}>
                                                    {fam.name}
                                                </span>
                                                <span className={styles.legendPct}>
                                                    {fam.percentage}%
                                                </span>
                                            </div>
                                            <p className={styles.legendDesc}>
                                                {fam.description}
                                            </p>
                                        </div>
                                    </div>
                                );
                            })}
                        </div>
                    </div>
                </div>

                {/* 2. Olfactory Radar Chart (Recharts RadarChart) */}
                <div className={styles.diagramCard}>
                    <div className={styles.cardHeader}>
                        <div>
                            <span className={styles.eyebrow}>TASTE EQUILIBRIUM</span>
                            <h4 className={styles.cardTitle}>
                                Olfactory Radar Spectrum
                            </h4>
                        </div>
                        <span className={styles.metaHint}>6-Axis Harmony</span>
                    </div>

                    <div className={styles.radarLayout}>
                        <div className={styles.radarChartWrapper}>
                            <ResponsiveContainer width="100%" height={290}>
                                <RadarChart
                                    cx="50%"
                                    cy="50%"
                                    outerRadius="68%"
                                    data={radarChartData}
                                    onMouseMove={(state) => {
                                        if (state && state.activeLabel) {
                                            const rawLabel = String(
                                                state.activeLabel
                                            ).replace(/\s*\(\d+%\)$/, '');
                                            setActiveRadarLabel(rawLabel);
                                        }
                                    }}
                                    onMouseLeave={() => setActiveRadarLabel(null)}
                                >
                                    <PolarGrid
                                        stroke="#e5e0d7"
                                        strokeDasharray="3 3"
                                    />
                                    <PolarAngleAxis
                                        dataKey="axisLabel"
                                        tick={{
                                            fill: '#77736c',
                                            fontSize: 10.5,
                                            fontWeight: 500,
                                            fontFamily: 'Inter, sans-serif',
                                        }}
                                    />
                                    <PolarRadiusAxis
                                        angle={90}
                                        domain={[0, 100]}
                                        tick={false}
                                        axisLine={false}
                                    />
                                    <Radar
                                        name="Olfactory Affinity"
                                        dataKey="value"
                                        stroke="#c59b27"
                                        strokeWidth={2.5}
                                        fill="#c59b27"
                                        fillOpacity={0.22}
                                        dot={{
                                            r: 4.5,
                                            fill: '#c59b27',
                                            stroke: '#fffefc',
                                            strokeWidth: 2,
                                        }}
                                        activeDot={{
                                            r: 6,
                                            fill: '#836719',
                                            stroke: '#fffefc',
                                            strokeWidth: 2,
                                        }}
                                    />
                                    <Tooltip
                                        formatter={(value) => [`${value}% Affinity`, 'Score']}
                                        contentStyle={{
                                            backgroundColor: '#fffefc',
                                            border: '1px solid #e5e0d7',
                                            borderRadius: '8px',
                                            fontSize: '12px',
                                            color: '#18181b',
                                            boxShadow: '0 6px 20px rgba(0, 0, 0, 0.06)',
                                        }}
                                    />
                                </RadarChart>
                            </ResponsiveContainer>
                        </div>

                        <div className={styles.radarSummary}>
                            <div className={styles.radarInsight}>
                                <span className={styles.insightIcon}>
                                    <Sparkles size={14} />
                                </span>
                                <div>
                                    {activeRadarDim ? (
                                        <>
                                            <strong>{activeRadarDim.label}:</strong>{' '}
                                            Affinity level is{' '}
                                            <span className={styles.highlightText}>
                                                {activeRadarDim.value}%
                                            </span>
                                            . Reflects your frequent selection of matching
                                            accords and notes.
                                        </>
                                    ) : (
                                        <>
                                            <strong>Signature Equilibrium:</strong> Your
                                            profile peaks in{' '}
                                            <span className={styles.highlightText}>
                                                {displayRadar[0]?.label || 'Woody & Earthy'}
                                            </span>{' '}
                                            and{' '}
                                            <span className={styles.highlightText}>
                                                {displayRadar[1]?.label || 'Citrus & Bright'}
                                            </span>{' '}
                                            accords, indicating a preference for timeless
                                            freshness that settles into warm, cedar-anchored
                                            sillage.
                                        </>
                                    )}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            {/* 3. Most Chosen & Explored Notes (Horizontal Progress Bars) */}
            <div className={styles.notesSection}>
                <div className={styles.cardHeader}>
                    <div>
                        <span className={styles.eyebrow}>
                            INDIVIDUAL INGREDIENT AFFINITY
                        </span>
                        <h4 className={styles.cardTitle}>
                            Top Selected Notes &amp; Ingredients
                        </h4>
                    </div>
                    <span className={styles.metaHint}>
                        Calculated across search &amp; library selections
                    </span>
                </div>

                <div className={styles.notesGrid}>
                    {topNotes.map((note, index) => {
                        const isHovered = hoveredNote === note.name;
                        return (
                            <div
                                key={note.name}
                                className={`${styles.noteRow} ${
                                    isHovered ? styles.noteRowHover : ''
                                }`}
                                onMouseEnter={() => setHoveredNote(note.name)}
                                onMouseLeave={() => setHoveredNote(null)}
                            >
                                <div className={styles.noteRank}>0{index + 1}</div>
                                <div className={styles.noteMain}>
                                    <div className={styles.noteMeta}>
                                        <div className={styles.noteTitleGroup}>
                                            <span className={styles.noteName}>
                                                {note.name}
                                            </span>
                                            <span className={styles.noteCategory}>
                                                {note.category}
                                            </span>
                                        </div>
                                        <div className={styles.noteStats}>
                                            <span className={styles.noteCount}>
                                                {note.count} selections
                                            </span>
                                            <span className={styles.notePercentage}>
                                                {note.percentage}% Match
                                            </span>
                                        </div>
                                    </div>

                                    {/* Progress Track */}
                                    <div className={styles.progressTrack}>
                                        <div
                                            className={styles.progressBar}
                                            style={{ width: `${note.percentage}%` }}
                                        />
                                    </div>
                                </div>
                            </div>
                        );
                    })}
                </div>
            </div>
        </section>
    );
};
