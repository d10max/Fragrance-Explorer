export interface OlfactoryPyramidStage {
    timing: string;
    notes: string[];
}

export interface FragranceDossier {
    houseLocation: string;
    perfumer: string;
    releaseYear: number;
    concentration: string;
    olfactoryFamily: string;
    longevity: string;
    sillage: string;
    bestSeasons: string;
}

export interface PerfumeItem {
    id: string;
    brand: string;
    name: string;
    concentration: string;
    year: number;
    family: 'Woody' | 'Citrus' | 'Fresh' | 'Spicy';
    rating: number;
    keyNotes: string[];
    storyAndVibe: string;
    capColor?: string;
    liquidColor?: string;
    liquidHeight?: number;
    isSelected?: boolean;

    // Detailed properties for detail view
    matchPercentage?: number;
    reviewsCount?: number;
    families?: string[];
    about?: string;
    vibeQuote?: string;
    dossier?: FragranceDossier;
    pyramid?: {
        topNotes: OlfactoryPyramidStage;
        heartNotes: OlfactoryPyramidStage;
        baseNotes: OlfactoryPyramidStage;
    };
    similarPerfumeIds?: string[];
}

export const PERFUMES_COLLECTION: PerfumeItem[] = [
    // Ряд 1
    {
        id: 'bal-dafrique',
        brand: 'BYREDO',
        name: "Bal d'Afrique",
        concentration: 'Eau de Parfum',
        year: 2009,
        family: 'Woody',
        rating: 4.9,
        keyNotes: ['Bergamot', 'Vetiver', 'Moroccan Cedar'],
        storyAndVibe:
            'Warm, romantic vetiver inspired by late-1920s Paris and African avant-garde.',
        capColor: '#18181b',
        liquidColor: '#eedb88',
        liquidHeight: 38,
        isSelected: true,
        matchPercentage: 96,
        reviewsCount: 342,
        families: ['Woody', 'Citrus'],
        about:
            "Bal d'Afrique is one of Ben Gorham's earliest and most celebrated creations — a warm and romantic vetiver inspired by Paris in the late 1920s and its infatuation with African culture, art, music, and dance. Opening with sun-drenched bergamot, lemon, and African marigold, it melts into a velvety heart of violet and jasmine before resting on a clean, luminous base of Moroccan cedarwood and earthy vetiver.",
        vibeQuote:
            '“Golden hour warmth on clean linen — joyful, luminous, and effortlessly sophisticated from morning to midnight.”',
        dossier: {
            houseLocation: 'Byredo (Stockholm, Sweden)',
            perfumer: 'Jérôme Epinette',
            releaseYear: 2009,
            concentration: 'Eau de Parfum (EDP)',
            olfactoryFamily: 'Woody · Citrus · Warm Amber',
            longevity: '8 – 10 hours (Long-lasting)',
            sillage: 'Moderate, enveloping aura',
            bestSeasons: 'Spring, Summer evenings, Autumn',
        },
        pyramid: {
            topNotes: {
                timing: 'First 15–30 mins · Bright & effervescent opening',
                notes: [
                    'Bergamot',
                    'Amalfi Lemon',
                    'African Marigold',
                    'Blackcurrant',
                    'Neroli',
                ],
            },
            heartNotes: {
                timing: 'Hours 1–4 · Soft floral & aromatic core',
                notes: ['Violet', 'Cyclamen', 'Jasmine Petals'],
            },
            baseNotes: {
                timing: 'Hours 4–10 · Warm, woody & skin-close drydown',
                notes: [
                    'Vetiver',
                    'Moroccan Cedarwood',
                    'Black Amber',
                    'Clean Musk',
                ],
            },
        },
        similarPerfumeIds: ['santal-33', 'terre-d-hermes', 'philosykos'],
    },
    {
        id: 'santal-33',
        brand: 'LE LABO',
        name: 'Santal 33',
        concentration: 'Eau de Parfum',
        year: 2011,
        family: 'Woody',
        rating: 4.8,
        keyNotes: ['Sandalwood', 'Cardamom', 'Violet'],
        storyAndVibe:
            'An open fire, the soft drift of smoke, and sensuality after the light fades.',
        capColor: '#7a7770',
        liquidColor: '#f4f0e7',
        liquidHeight: 28,
        isSelected: false,
        matchPercentage: 92,
        reviewsCount: 512,
        families: ['Woody', 'Spicy'],
        about:
            'An iconic scent inspired by the spirit of the American West and freedom. Cardamom, iris, and violet crackle in the formula, yielding a smoky wood alloy (Australian sandalwood, cedarwood) with a subtle leather facet that creates intoxicating presence.',
        vibeQuote:
            '“An open fire in the desert night, leather boots, and soft cedar embers that linger endlessly.”',
        dossier: {
            houseLocation: 'Le Labo (New York, USA)',
            perfumer: 'Frank Voelkl',
            releaseYear: 2011,
            concentration: 'Eau de Parfum (EDP)',
            olfactoryFamily: 'Woody · Smoky · Leathery',
            longevity: '10 – 12 hours (Very long-lasting)',
            sillage: 'Heavy, unmistakable trail',
            bestSeasons: 'Autumn, Winter, Cool Evenings',
        },
        pyramid: {
            topNotes: {
                timing: 'First 15–30 mins · Spiced aromatic opening',
                notes: ['Cardamom', 'Iris', 'Violet'],
            },
            heartNotes: {
                timing: 'Hours 1–4 · Dry leather & smoky floral',
                notes: ['Papyrus', 'Ambrox', 'Cedarwood'],
            },
            baseNotes: {
                timing: 'Hours 4–12 · Deep sandalwood & leather',
                notes: ['Australian Sandalwood', 'Leather', 'Warm Amber'],
            },
        },
        similarPerfumeIds: ['bal-dafrique', 'hwyl', 'the-noir-29'],
    },
    {
        id: 'terre-d-hermes',
        brand: 'HERMÈS',
        name: "Terre d'Hermès",
        concentration: 'Eau de Toilette',
        year: 2006,
        family: 'Citrus',
        rating: 4.9,
        keyNotes: ['Grapefruit', 'Flint', 'Cedar'],
        storyAndVibe:
            'Sun-baked earth after rain — mineral, citrus-bright, and effortlessly noble.',
        capColor: '#18181b',
        liquidColor: '#e2a654',
        liquidHeight: 52,
        isSelected: false,
        matchPercentage: 94,
        reviewsCount: 428,
        families: ['Citrus', 'Woody'],
        about:
            "Terre d'Hermès narrates the relationship between man and earth, his humble and harmonious dialogue with the elements and nature. It connects man to his origins and to the source of his creative power with sparkling grapefruit and earthy mineral flint.",
        vibeQuote:
            '“Sun-baked earth after rain — mineral, citrus-bright, and effortlessly noble.”',
        dossier: {
            houseLocation: 'Hermès (Paris, France)',
            perfumer: 'Jean-Claude Ellena',
            releaseYear: 2006,
            concentration: 'Eau de Toilette (EDT)',
            olfactoryFamily: 'Woody · Citrus · Mineral',
            longevity: '7 – 9 hours (Moderate-long)',
            sillage: 'Moderate, sophisticated projection',
            bestSeasons: 'Spring, Summer, Autumn',
        },
        pyramid: {
            topNotes: {
                timing: 'First 15–30 mins · Sparkling bitter citrus',
                notes: ['Orange', 'Grapefruit', 'Shiso'],
            },
            heartNotes: {
                timing: 'Hours 1–4 · Mineral flint & spicy pepper',
                notes: ['Flint', 'Pink Pepper', 'Pelargonium'],
            },
            baseNotes: {
                timing: 'Hours 4–8 · Grounded cedar & vetiver',
                notes: ['Atlas Cedar', 'Vetiver', 'Patchouli', 'Benzoin'],
            },
        },
        similarPerfumeIds: ['bal-dafrique', 'paris-edimbourg', 'philosykos'],
    },

    // Ряд 2
    {
        id: 'philosykos',
        brand: 'DIPTYQUE',
        name: 'Philosykos',
        concentration: 'Eau de Parfum',
        year: 1996,
        family: 'Fresh',
        rating: 4.8,
        keyNotes: ['Fig Leaf', 'Fig Milk', 'White Cedar'],
        storyAndVibe:
            'A summer walk through a Greek grove of wild fig trees by the sea.',
        capColor: '#18181b',
        liquidColor: '#e8ece5',
        liquidHeight: 36,
        isSelected: false,
        matchPercentage: 91,
        reviewsCount: 289,
        families: ['Fresh', 'Woody'],
        about:
            'An ode to the entire fig tree: the green freshness of the leaves, the density of the white wood, and the milky sap of the ripe fruit. Inspired by memories of Grecian summers in Mount Pelion.',
        vibeQuote:
            '“A summer walk through a Greek grove of wild fig trees by the sea.”',
        dossier: {
            houseLocation: 'Diptyque (Paris, France)',
            perfumer: 'Olivia Giacobetti',
            releaseYear: 1996,
            concentration: 'Eau de Parfum (EDP)',
            olfactoryFamily: 'Fresh Green · Woody · Fruity',
            longevity: '7 – 8 hours',
            sillage: 'Intimate to moderate aura',
            bestSeasons: 'Spring, Summer',
        },
        pyramid: {
            topNotes: {
                timing: 'First 15–30 mins · Crisp green fig leaf',
                notes: ['Fig Leaf', 'Fig Fruit', 'Green Sap'],
            },
            heartNotes: {
                timing: 'Hours 1–4 · Creamy milky coconut accords',
                notes: ['Fig Milk', 'Coconut', 'Green Woods'],
            },
            baseNotes: {
                timing: 'Hours 4–8 · Soft cedar & solar bark',
                notes: ['White Cedar', 'Dry Wood', 'Clean Musk'],
            },
        },
        similarPerfumeIds: ['bal-dafrique', 'the-noir-29', 'paris-edimbourg'],
    },
    {
        id: 'paris-edimbourg',
        brand: 'CHANEL',
        name: 'Paris–Édimbourg',
        concentration: 'Eau de Toilette',
        year: 2021,
        family: 'Fresh',
        rating: 4.7,
        keyNotes: ['Juniper', 'Cypress', 'Vetiver'],
        storyAndVibe:
            'A tweed jacket in the Highlands. Bracing, green, and beautifully understated.',
        capColor: '#18181b',
        liquidColor: '#c9e2cf',
        liquidHeight: 44,
        isSelected: false,
        matchPercentage: 89,
        reviewsCount: 174,
        families: ['Fresh', 'Woody'],
        about:
            'Inspired by the Scottish Highlands, where Gabrielle Chanel loved to recharge in nature with the Duke of Westminster. Bracingly green and aromatic, distinguished by juniper berry, peat smoke, and gentle vetiver.',
        vibeQuote:
            '“A tweed jacket in the Highlands. Bracing, green, and beautifully understated.”',
        dossier: {
            houseLocation: 'Chanel (Paris, France)',
            perfumer: 'Olivier Polge',
            releaseYear: 2021,
            concentration: 'Eau de Toilette (EDT)',
            olfactoryFamily: 'Aromatic · Woody · Fresh',
            longevity: '6 – 8 hours',
            sillage: 'Refined, skin-close elegance',
            bestSeasons: 'Spring, Autumn, Cool Mornings',
        },
        pyramid: {
            topNotes: {
                timing: 'First 15–30 mins · Bracing juniper berry',
                notes: ['Juniper Berries', 'Cypress', 'Lavender'],
            },
            heartNotes: {
                timing: 'Hours 1–4 · Aromatic cedar & dry herbal',
                notes: ['Cedar', 'Peat Smoke', 'Thyme'],
            },
            baseNotes: {
                timing: 'Hours 4–7 · Earthy vetiver & vanilla hints',
                notes: ['Vetiver', 'Musk', 'Warm Amber'],
            },
        },
        similarPerfumeIds: ['terre-d-hermes', 'hwyl', 'philosykos'],
    },
    {
        id: 'the-noir-29',
        brand: 'LE LABO',
        name: 'Thé Noir 29',
        concentration: 'Eau de Parfum',
        year: 2015,
        family: 'Spicy',
        rating: 4.9,
        keyNotes: ['Black Tea', 'Fig', 'Tobacco'],
        storyAndVibe:
            'Luminous bergamot meets dark, cured black tea leaves and warm cedar.',
        capColor: '#7a7770',
        liquidColor: '#e4cdb3',
        liquidHeight: 36,
        isSelected: false,
        matchPercentage: 95,
        reviewsCount: 398,
        families: ['Spicy', 'Woody'],
        about:
            'An ode to the noble leaf and the craft that surrounds it. Thé Noir 29 combines lightness and depth, freshness and mystery through a continuous oscillation between the light of bergamot, fig, and bay leaves and the depth of cedarwood, vetiver, and musk.',
        vibeQuote:
            '“Luminous bergamot meets dark, cured black tea leaves and warm cedar.”',
        dossier: {
            houseLocation: 'Le Labo (New York, USA)',
            perfumer: 'Frank Voelkl',
            releaseYear: 2015,
            concentration: 'Eau de Parfum (EDP)',
            olfactoryFamily: 'Aromatic · Spicy · Woody',
            longevity: '9 – 11 hours',
            sillage: 'Alluring, sensual projection',
            bestSeasons: 'Autumn, Winter, Evenings',
        },
        pyramid: {
            topNotes: {
                timing: 'First 15–30 mins · Crisp tea & fresh fig',
                notes: ['Fig', 'Bay Leaf', 'Bergamot'],
            },
            heartNotes: {
                timing: 'Hours 1–4 · Smoked cedar & cured black tea',
                notes: ['Cedarwood', 'Vetiver', 'Black Tea'],
            },
            baseNotes: {
                timing: 'Hours 4–10 · Hay, dry tobacco & musk',
                notes: ['Tobacco Leaf', 'Hay', 'Musk'],
            },
        },
        similarPerfumeIds: ['santal-33', 'bal-dafrique', 'eau-duelle'],
    },

    // Ряд 3
    {
        id: 'gypsy-water',
        brand: 'BYREDO',
        name: 'Gypsy Water',
        concentration: 'Eau de Parfum',
        year: 2008,
        family: 'Woody',
        rating: 4.7,
        keyNotes: ['Pine Needle', 'Sandalwood', 'Vanilla'],
        storyAndVibe:
            'Fresh soil, deep forests, and campfires under a star-filled northern sky.',
        capColor: '#18181b',
        liquidColor: '#eee7d6',
        liquidHeight: 40,
        isSelected: false,
        matchPercentage: 90,
        reviewsCount: 310,
        families: ['Woody', 'Fresh'],
        about:
            'An ode to the beauty of Romany culture, its unique customs, intimate beliefs, and distinguished way of living. The scent wakens a dream of a colorful lifestyle made of innate nomadism.',
        vibeQuote:
            '“Fresh soil, deep forests, and campfires under a star-filled northern sky.”',
        dossier: {
            houseLocation: 'Byredo (Stockholm, Sweden)',
            perfumer: 'Jérôme Epinette',
            releaseYear: 2008,
            concentration: 'Eau de Parfum (EDP)',
            olfactoryFamily: 'Woody · Aromatic · Balsamic',
            longevity: '6 – 8 hours',
            sillage: 'Subtle, personal skin-scent',
            bestSeasons: 'Spring, Summer, Autumn',
        },
        pyramid: {
            topNotes: {
                timing: 'First 15–30 mins · Effervescent juniper & citrus',
                notes: ['Juniper', 'Lemon', 'Bergamot', 'Pepper'],
            },
            heartNotes: {
                timing: 'Hours 1–4 · Balsamic pine needles & incense',
                notes: ['Pine Needles', 'Incense', 'Orris Root'],
            },
            baseNotes: {
                timing: 'Hours 4–8 · Warm vanilla & creamy sandalwood',
                notes: ['Vanilla', 'Sandalwood', 'Amber'],
            },
        },
        similarPerfumeIds: ['bal-dafrique', 'santal-33', 'hwyl'],
    },
    {
        id: 'hwyl',
        brand: 'AESOP',
        name: 'Hwyl',
        concentration: 'Eau de Parfum',
        year: 2017,
        family: 'Woody',
        rating: 4.8,
        keyNotes: ['Hinoki', 'Frankincense', 'Oakmoss'],
        storyAndVibe:
            'Quiet solitude inside a centuries-old Japanese hinoki forest shrouded in mist.',
        capColor: '#18181b',
        liquidColor: '#be9364',
        liquidHeight: 48,
        isSelected: false,
        matchPercentage: 93,
        reviewsCount: 215,
        families: ['Woody', 'Spicy'],
        about:
            'An intriguing fragrance reminiscent of the verdant and eccentric shelter of an old-growth forest. Smoky notes open to reveal aromatic spices and fresh thyme, followed by green Cypress and warm vetiver.',
        vibeQuote:
            '“Quiet solitude inside a centuries-old Japanese hinoki forest shrouded in mist.”',
        dossier: {
            houseLocation: 'Aesop (Melbourne, Australia)',
            perfumer: 'Barnabé Fillion',
            releaseYear: 2017,
            concentration: 'Eau de Parfum (EDP)',
            olfactoryFamily: 'Woody · Smoky · Resinous',
            longevity: '8 – 10 hours',
            sillage: 'Contemplative, distinguished trail',
            bestSeasons: 'Autumn, Winter, Rainy Days',
        },
        pyramid: {
            topNotes: {
                timing: 'First 15–30 mins · Smoky spices & thyme',
                notes: ['Spices', 'Thyme', 'Smoke'],
            },
            heartNotes: {
                timing: 'Hours 1–4 · Ancient Cypress & hinoki wood',
                notes: ['Cypress', 'Hinoki', 'Geranium'],
            },
            baseNotes: {
                timing: 'Hours 4–9 · Earthy oakmoss & frankincense',
                notes: ['Vetiver', 'Frankincense', 'Oakmoss', 'Cedar'],
            },
        },
        similarPerfumeIds: ['santal-33', 'paris-edimbourg', 'the-noir-29'],
    },
    {
        id: 'eau-duelle',
        brand: 'DIPTYQUE',
        name: 'Eau Duelle',
        concentration: 'Eau de Parfum',
        year: 2010,
        family: 'Spicy',
        rating: 4.8,
        keyNotes: ['Bourbon Vanilla', 'Cardamom', 'Elemi'],
        storyAndVibe:
            'Spiced, smoky vanilla along the ancient incense routes — never overly sweet.',
        capColor: '#18181b',
        liquidColor: '#dfcbb2',
        liquidHeight: 42,
        isSelected: false,
        matchPercentage: 91,
        reviewsCount: 260,
        families: ['Spicy', 'Woody'],
        about:
            'An ode to travel and vanilla. Eau Duelle traces the spice routes from Goa to Venice, framing dark bourbon vanilla against bright green cardamom and resinous elemi with smoky calamus.',
        vibeQuote:
            '“Spiced, smoky vanilla along the ancient incense routes — never overly sweet.”',
        dossier: {
            houseLocation: 'Diptyque (Paris, France)',
            perfumer: 'Fabrice Pellegrin',
            releaseYear: 2010,
            concentration: 'Eau de Parfum (EDP)',
            olfactoryFamily: 'Spicy · Oriental · Warm Amber',
            longevity: '8 – 10 hours',
            sillage: 'Soft, seductive warmth',
            bestSeasons: 'Autumn, Winter',
        },
        pyramid: {
            topNotes: {
                timing: 'First 15–30 mins · Bright aromatic elemi & cardamom',
                notes: ['Cardamom', 'Elemi', 'Pink Pepper', 'Juniper'],
            },
            heartNotes: {
                timing: 'Hours 1–4 · Smoky calamus & saffron',
                notes: ['Frankincense', 'Calamus', 'Black Tea'],
            },
            baseNotes: {
                timing: 'Hours 4–9 · Bourbon vanilla & dark amber',
                notes: ['Bourbon Vanilla', 'Cypriol Oil', 'Amber'],
            },
        },
        similarPerfumeIds: ['the-noir-29', 'bal-dafrique', 'santal-33'],
    },
];

export const getPerfumeById = (id?: string): PerfumeItem => {
    if (!id) return PERFUMES_COLLECTION[0];
    const found = PERFUMES_COLLECTION.find((p) => p.id === id);
    if (found) return found;

    // Fallback to Bal d'Afrique if unknown
    return PERFUMES_COLLECTION[0];
};

export const getSimilarPerfumes = (perfume: PerfumeItem): PerfumeItem[] => {
    if (perfume.similarPerfumeIds && perfume.similarPerfumeIds.length > 0) {
        const matches = PERFUMES_COLLECTION.filter((p) =>
            perfume.similarPerfumeIds?.includes(p.id)
        );
        if (matches.length > 0) return matches.slice(0, 3);
    }
    return PERFUMES_COLLECTION.filter((p) => p.id !== perfume.id).slice(0, 3);
};
