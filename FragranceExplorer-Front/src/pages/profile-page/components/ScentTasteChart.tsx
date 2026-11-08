import { useState } from 'react';
import { useProfile, type FamilyStat } from '@/context/ProfileContext';
import styles from './ScentTasteChart.module.scss';

// Trigonometric helpers for rock-solid SVG arc geometry
const polarToCartesian = (
    cx: number,
    cy: number,
    r: number,
    angleInDegrees: number
) => {
    const angleInRadians = ((angleInDegrees - 90) * Math.PI) / 180.0;
    return {
        x: cx + r * Math.cos(angleInRadians),
        y: cy + r * Math.sin(angleInRadians),
    };
};

const describeArc = (
    cx: number,
    cy: number,
    r: number,
    startAngle: number,
    endAngle: number
) => {
    const span = endAngle - startAngle;
    if (span >= 359.99) {
        return `M ${cx} ${cy - r} A ${r} ${r} 0 1 1 ${cx - 0.01} ${cy - r} Z`;
    }
    const start = polarToCartesian(cx, cy, r, endAngle);
    const end = polarToCartesian(cx, cy, r, startAngle);
    const largeArcFlag = span > 180 ? 1 : 0;
    return `M ${start.x} ${start.y} A ${r} ${r} 0 ${largeArcFlag} 0 ${end.x} ${end.y}`;
};

export const ScentTasteChart = () => {
    const { analytics } = useProfile();
    const { families, topNotes, radarData, vibePersona, vibeDescription } = analytics;

    const [activeFamily, setActiveFamily] = useState<FamilyStat | null>(null);
    const [hoveredNote, setHoveredNote] = useState<string | null>(null);
    const [hoveredRadarIdx, setHoveredRadarIdx] = useState<number | null>(null);

    // SVG Donut calculation with explicit arc paths (zero transform displacement)
    const donutCenter = 125;
    const donutRadius = 78;
    const strokeWidth = 26;

    let currentAngle = 0;
    const activeFamilies = families.filter((f) => f.percentage > 0);
    const arcSegments = activeFamilies.map((family) => {
        const angleSpan = (family.percentage / 100) * 360;
        const startAngle = currentAngle;
        const endAngle = currentAngle + angleSpan;
        currentAngle = endAngle;

        const pathD = describeArc(
            donutCenter,
            donutCenter,
            donutRadius,
            startAngle,
            endAngle
        );

        return {
            family,
            pathD,
            startAngle,
            endAngle,
        };
    });

    // Radar chart calculations (Center at 160, 160; Max radius = 105)
    const radarCenter = 160;
    const radarRadius = 105;
    const radarCount = radarData.length;

    const getRadarCoordinates = (index: number, valPercent: number) => {
        // -90 deg starts at top
        const angle = (Math.PI * 2 / radarCount) * index - Math.PI / 2;
        const r = (valPercent / 100) * radarRadius;
        const x = radarCenter + r * Math.cos(angle);
        const y = radarCenter + r * Math.sin(angle);
        return { x, y, angle };
    };

    // Grid polygons
    const gridLevels = [0.25, 0.5, 0.75, 1.0];
    const gridPolygons = gridLevels.map((lvl) => {
        return radarData
            .map((_, i) => {
                const { x, y } = getRadarCoordinates(i, lvl * 100);
                return `${x},${y}`;
            })
            .join(' ');
    });

    // Data polygon
    const dataPolygonPoints = radarData
        .map((dim, i) => {
            const { x, y } = getRadarCoordinates(i, dim.value);
            return `${x},${y}`;
        })
        .join(' ');

    const highlightedFamily = activeFamily || families.find((f) => f.percentage > 0) || families[0];
    const activeRadarDim = hoveredRadarIdx !== null ? radarData[hoveredRadarIdx] : null;

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
                        <span className={styles.pillVal}>Moroccan Cedar &amp; Vetiver</span>
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

            {/* Grid of the 2 primary visual diagrams */}
            <div className={styles.diagramsGrid}>
                {/* 1. Fragrance Families Donut Chart */}
                <div className={styles.diagramCard}>
                    <div className={styles.cardHeader}>
                        <div>
                            <span className={styles.eyebrow}>SCENT WHEEL RATIO</span>
                            <h4 className={styles.cardTitle}>Olfactory Families Distribution</h4>
                        </div>
                        <span className={styles.metaHint}>Hover over slices or families</span>
                    </div>

                    <div className={styles.donutLayout}>
                        {/* Interactive SVG Donut */}
                        <div className={styles.donutSvgWrapper}>
                            <svg
                                width="250"
                                height="250"
                                viewBox="0 0 250 250"
                                className={styles.donutSvg}
                            >
                                {/* Base neutral track */}
                                <circle
                                    cx={donutCenter}
                                    cy={donutCenter}
                                    r={donutRadius}
                                    fill="none"
                                    stroke="#eeebe4"
                                    strokeWidth={strokeWidth}
                                />

                                {/* Exact Trigonometric Arc Paths */}
                                {arcSegments.map(({ family, pathD }) => {
                                    const isHovered = activeFamily?.name === family.name;
                                    return (
                                        <path
                                            key={family.name}
                                            d={pathD}
                                            fill="none"
                                            stroke={family.color}
                                            strokeWidth={isHovered ? strokeWidth + 6 : strokeWidth}
                                            strokeLinecap="butt"
                                            className={styles.donutArc}
                                            onMouseEnter={() => setActiveFamily(family)}
                                            onMouseLeave={() => setActiveFamily(null)}
                                            style={{
                                                cursor: 'pointer',
                                                transition: 'stroke-width 0.2s ease, opacity 0.2s ease',
                                                opacity: activeFamily && !isHovered ? 0.4 : 1,
                                            }}
                                        />
                                    );
                                })}

                                {/* Center content */}
                                <g className={styles.donutCenter}>
                                    <text
                                        x={donutCenter}
                                        y={donutCenter - 10}
                                        textAnchor="middle"
                                        className={styles.donutCenterPct}
                                    >
                                        {highlightedFamily.percentage}%
                                    </text>
                                    <text
                                        x={donutCenter}
                                        y={donutCenter + 12}
                                        textAnchor="middle"
                                        className={styles.donutCenterLabel}
                                    >
                                        {highlightedFamily.name}
                                    </text>
                                    <text
                                        x={donutCenter}
                                        y={donutCenter + 30}
                                        textAnchor="middle"
                                        className={styles.donutCenterSub}
                                    >
                                        {highlightedFamily.count} saved scents
                                    </text>
                                </g>
                            </svg>
                        </div>

                        {/* Interactive Family Legend */}
                        <div className={styles.familyLegend}>
                            {families.map((fam) => {
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
                                                <span className={styles.legendName}>{fam.name}</span>
                                                <span className={styles.legendPct}>{fam.percentage}%</span>
                                            </div>
                                            <p className={styles.legendDesc}>{fam.description}</p>
                                        </div>
                                    </div>
                                );
                            })}
                        </div>
                    </div>
                </div>

                {/* 2. Olfactory Radar Chart (Multi-dimensional Profile) */}
                <div className={styles.diagramCard}>
                    <div className={styles.cardHeader}>
                        <div>
                            <span className={styles.eyebrow}>TASTE EQUILIBRIUM</span>
                            <h4 className={styles.cardTitle}>Olfactory Radar Spectrum</h4>
                        </div>
                        <span className={styles.metaHint}>6-Axis Harmony</span>
                    </div>

                    <div className={styles.radarLayout}>
                        <svg
                            width="320"
                            height="320"
                            viewBox="0 0 320 320"
                            className={styles.radarSvg}
                        >
                            {/* Concentric grid polygons */}
                            {gridPolygons.map((pts, idx) => (
                                <polygon
                                    key={idx}
                                    points={pts}
                                    fill="none"
                                    stroke="#e5e0d7"
                                    strokeWidth="1"
                                    strokeDasharray={idx === 3 ? 'none' : '3 3'}
                                />
                            ))}

                            {/* Spoke lines */}
                            {radarData.map((_, i) => {
                                const { x, y } = getRadarCoordinates(i, 100);
                                return (
                                    <line
                                        key={i}
                                        x1={radarCenter}
                                        y1={radarCenter}
                                        x2={x}
                                        y2={y}
                                        stroke="#e5e0d7"
                                        strokeWidth="1"
                                    />
                                );
                            })}

                            {/* Shaded data polygon */}
                            <polygon
                                points={dataPolygonPoints}
                                fill="rgba(197, 155, 39, 0.22)"
                                stroke="#c59b27"
                                strokeWidth="2.5"
                                className={styles.radarPolygon}
                            />

                            {/* Vertex markers & labels with stable hover target (no cursor flee / jitter) */}
                            {radarData.map((dim, i) => {
                                const { x, y } = getRadarCoordinates(i, dim.value);
                                const labelCoord = getRadarCoordinates(i, 118);
                                const isHovered = hoveredRadarIdx === i;

                                return (
                                    <g
                                        key={dim.label}
                                        onMouseEnter={() => setHoveredRadarIdx(i)}
                                        onMouseLeave={() => setHoveredRadarIdx(null)}
                                        style={{ cursor: 'pointer' }}
                                    >
                                        {/* Large invisible circle to maintain solid, jitter-free cursor hover */}
                                        <circle
                                            cx={x}
                                            cy={y}
                                            r="16"
                                            fill="transparent"
                                        />

                                        {/* Subtle halo on hover without coordinate displacement */}
                                        {isHovered && (
                                            <circle
                                                cx={x}
                                                cy={y}
                                                r="9"
                                                fill="rgba(197, 155, 39, 0.28)"
                                            />
                                        )}

                                        {/* Crisp vertex dot */}
                                        <circle
                                            cx={x}
                                            cy={y}
                                            r={isHovered ? 6 : 4.5}
                                            fill={isHovered ? '#836719' : '#c59b27'}
                                            stroke="#fffefc"
                                            strokeWidth="2"
                                            className={styles.radarPoint}
                                        />

                                        {/* Label text */}
                                        <text
                                            x={labelCoord.x}
                                            y={labelCoord.y + 4}
                                            textAnchor={
                                                Math.abs(labelCoord.x - radarCenter) < 15
                                                    ? 'middle'
                                                    : labelCoord.x > radarCenter
                                                    ? 'start'
                                                    : 'end'
                                            }
                                            className={`${styles.radarLabel} ${
                                                isHovered ? styles.radarLabelHovered : ''
                                            }`}
                                        >
                                            {dim.label} ({dim.value}%)
                                        </text>
                                    </g>
                                );
                            })}
                        </svg>

                        <div className={styles.radarSummary}>
                            <div className={styles.radarInsight}>
                                <span className={styles.insightIcon}>✦</span>
                                <div>
                                    {activeRadarDim ? (
                                        <>
                                            <strong>{activeRadarDim.label}:</strong> Affinity level is{' '}
                                            <span className={styles.highlightText}>
                                                {activeRadarDim.value}%
                                            </span>
                                            . Reflects your frequent selection of matching accords and notes.
                                        </>
                                    ) : (
                                        <>
                                            <strong>Signature Equilibrium:</strong> Your profile peaks in{' '}
                                            <span className={styles.highlightText}>Woody &amp; Earthy</span> and{' '}
                                            <span className={styles.highlightText}>Citrus &amp; Bright</span> accords, indicating a preference for timeless freshness that settles into warm, cedar-anchored sillage.
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
                        <span className={styles.eyebrow}>INDIVIDUAL INGREDIENT AFFINITY</span>
                        <h4 className={styles.cardTitle}>Top Selected Notes &amp; Ingredients</h4>
                    </div>
                    <span className={styles.metaHint}>Calculated across search &amp; library selections</span>
                </div>

                <div className={styles.notesGrid}>
                    {topNotes.map((note, index) => {
                        const isHovered = hoveredNote === note.name;
                        return (
                            <div
                                key={note.name}
                                className={`${styles.noteRow} ${isHovered ? styles.noteRowHover : ''}`}
                                onMouseEnter={() => setHoveredNote(note.name)}
                                onMouseLeave={() => setHoveredNote(null)}
                            >
                                <div className={styles.noteRank}>0{index + 1}</div>
                                <div className={styles.noteMain}>
                                    <div className={styles.noteMeta}>
                                        <div className={styles.noteTitleGroup}>
                                            <span className={styles.noteName}>{note.name}</span>
                                            <span className={styles.noteCategory}>{note.category}</span>
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
