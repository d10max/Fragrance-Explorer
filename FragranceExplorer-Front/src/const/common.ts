export const COMMON_TEXT = {
    APP_NAME: 'Fragrance-Explorer',
    TABS: {
        PERFUME_FINDER: 'Finder',
        PERFUME_CATALOG: 'Perfume Encyclopedia',
        PROFILE: 'Profile',
    },
    PLACEHOLDERS: {
        SEARCH_INPUT:
            "Search perfume, brand, or note (e.g. Bal d'Afrique, Byredo, Vetiver)...",
        BUTTON_ADD_PERFUME: '+ Add',
    },
};

export const SEARCH_DROPDOWN = {
    FRAGRANCES: 'Fragrances',
    NO_RESULTS: 'No fragrances found',
};

export const TARGET_PERFUME_CARD_TEXT = {
    TITLE: 'Your starter kit',
    SELECTED_COUNT: (count: number) => `${count}/1 SELECTED`,
    PLACEHOLDER_TEXT:
        'Select 1 favorite perfume above to find its closest matches.',
};

export const SEARCH_MODES_PANEL_TEXT = {
    TITLE: 'Choose matching mode',
    HINT: 'Switch between balanced profile, exact note clones, or similar accord vibe.',
    SEARCH_BUTTON_PLACEHOLDER: 'Find My Top 10 Perfumes',
    MODES: {
        BALANCED: {
            badgeText: 'DEFAULT',
            title: 'Balanced Match',
            description:
                'Harmony of shared pyramid notes and overall fragrance accords for the most natural recommendation.',
        },
        CLONES: {
            badgeText: 'FOCUS ON NOTES',
            title: 'Exact Clones',
            description:
                'Maximum overlap of top, heart, and base notes. Ideal for finding close dupes and direct alternatives.',
        },
        SIMILAR: {
            badgeText: 'FOCUS ON ACCORDS',
            title: 'Similar Vibe & Mood',
            description:
                'Prioritizes dominant accords, warmth, and atmosphere — even when specific ingredients differ.',
        },
    },
};

export const SEARCH_RESULTS_GRID_TEXT = {
    PERFUME_CARD: {
        MATCH_BADGE_TEXT: (score: number) => `${score}% Match`,
        VIBE_LABEL_PLACEHOLDER: 'FRAGRANCE VIBE',
        VIBE_DEFAULT_LABEL: 'Bright, free-spirited and quietly magnetic.',
    },
    GRID: {
        TAG: 'CURATED MATCHES FOR YOU',
        TITLE: 'Ten scents. A thousand possibilities.',
        SUBTITLE:
            'Your personal edit, ranked by the notes you love and the feeling they leave behind.',
    },
    EMPTY_GRID: {
        TAG: 'NO MATCHES FOUND',
        TITLE: 'No fragrances matched your criteria',
        SUBTITLE:
            'Try selecting a different starter perfume or switching to another matching mode.',
    },
};
