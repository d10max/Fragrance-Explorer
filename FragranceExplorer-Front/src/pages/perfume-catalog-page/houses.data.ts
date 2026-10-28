export interface HouseDnaItem {
    family: 'Woody' | 'Spicy' | 'Fresh' | 'Citrus';
    notes: string;
    percentage: number;
}

export interface HouseItem {
    id: string;
    name: string;
    displayName?: string;
    location: string;
    count: string;
    archiveCount?: number;
    initials?: string;
    originLabel?: string;
    establishedLabel?: string;
    dossierEyebrow?: string;
    dnaBadge?: string;
    rating?: number;
    reviewsLabel?: string;
    description?: string;
    olfactoryDna?: HouseDnaItem[];
    familyCounts?: {
        All: number;
        Woody?: number;
        Spicy?: number;
        Fresh?: number;
        Citrus?: number;
    };
    similarHouseIds?: string[];
}

export const HOUSES_DATA: HouseItem[] = [
    // Топ-6 за замовчуванням
    {
        id: 'byredo',
        name: 'BYREDO',
        displayName: 'Byredo',
        location: 'Stockholm · Est. 2006',
        count: '24 perfumes',
        archiveCount: 24,
        initials: 'BY',
        originLabel: 'STOCKHOLM · PARIS',
        establishedLabel: 'EST. 2006',
        dossierEyebrow: 'FRAGRANCE HOUSE DOSSIER · MODERN LUXURY PERFUMERY',
        dnaBadge: 'Woody & Floral DNA',
        rating: 4.8,
        reviewsLabel: '3.1k reviews',
        description:
            'Founded in Stockholm in 2006 by Ben Gorham, Byredo translates personal memories and human emotions into contemporary olfactory experiences. Known for minimalist Nordic aesthetics and luminous, clean compositions crafted from rare raw materials.',
        olfactoryDna: [
            { family: 'Woody', notes: 'Moroccan Cedar · Vetiver · Pine', percentage: 85 },
            { family: 'Fresh', notes: 'Bergamot · Juniper · Violet', percentage: 76 },
            { family: 'Spicy', notes: 'Incense · Cardamom · Amber', percentage: 64 },
        ],
        familyCounts: { All: 24, Woody: 10, Fresh: 7, Citrus: 4, Spicy: 3 },
        similarHouseIds: ['le-labo', 'diptyque', 'aesop', 'hermes', 'chanel', 'ds-durga'],
    },
    {
        id: 'le-labo',
        name: 'LE LABO',
        displayName: 'Le Labo',
        location: 'New York · Est. 2006',
        count: '19 perfumes',
        archiveCount: 19,
        initials: 'LL',
        originLabel: 'GRASSE · NEW YORK',
        establishedLabel: 'EST. 2006',
        dossierEyebrow: 'FRAGRANCE HOUSE DOSSIER · NICHE ARTISANAL PERFUMERY',
        dnaBadge: 'Woody & Spicy DNA',
        rating: 4.8,
        reviewsLabel: '2.4k reviews',
        description:
            "Founded in New York's Nolita in 2006 by Fabrice Penot and Edouard Roschi, Le Labo crafts slow, soulful perfumery rooted in Grasse raw materials. Each creation is named after its principal note followed by the total number of ingredients in the formula — celebrating raw textures, smoky woods, and skin-close musks.",
        olfactoryDna: [
            { family: 'Woody', notes: 'Sandalwood · Cedar · Vetiver', percentage: 88 },
            { family: 'Spicy', notes: 'Black Tea · Cardamom · Cumin', percentage: 74 },
            { family: 'Fresh', notes: 'Bergamot · Matcha · Fig Leaf', percentage: 62 },
        ],
        familyCounts: { All: 19, Woody: 8, Spicy: 5, Fresh: 4, Citrus: 2 },
        similarHouseIds: ['byredo', 'diptyque', 'aesop', 'hermes', 'chanel', 'ds-durga'],
    },
    {
        id: 'diptyque',
        name: 'DIPTYQUE',
        displayName: 'Diptyque',
        location: 'Paris · Est. 1961',
        count: '32 perfumes',
        archiveCount: 32,
        initials: 'DP',
        originLabel: 'BOULEVARD SAINT-GERMAIN · PARIS',
        establishedLabel: 'EST. 1961',
        dossierEyebrow: 'FRAGRANCE HOUSE DOSSIER · POETIC BOTANICAL PERFUMERY',
        dnaBadge: 'Botanical & Woody DNA',
        rating: 4.9,
        reviewsLabel: '2.9k reviews',
        description:
            'Born at 34 Boulevard Saint-Germain in 1961 from the creative friendship of three artists, Diptyque turns travel memories and Mediterranean nature into poetic olfactory landscapes — from sun-warmed fig groves to spiced vanilla routes.',
        olfactoryDna: [
            { family: 'Fresh', notes: 'Fig Leaf · Green Sap · Rose', percentage: 86 },
            { family: 'Woody', notes: 'White Cedar · Sandalwood · Tam Dao', percentage: 79 },
            { family: 'Spicy', notes: 'Bourbon Vanilla · Elemi · Pink Pepper', percentage: 68 },
        ],
        familyCounts: { All: 32, Fresh: 12, Woody: 10, Spicy: 6, Citrus: 4 },
        similarHouseIds: ['byredo', 'le-labo', 'aesop', 'hermes', 'chanel', 'ds-durga'],
    },
    {
        id: 'hermes',
        name: 'HERMÈS',
        displayName: 'Hermès',
        location: 'Paris · Est. 1837',
        count: '28 perfumes',
        archiveCount: 28,
        initials: 'HM',
        originLabel: 'FAUBOURG SAINT-HONORÉ · PARIS',
        establishedLabel: 'EST. 1837',
        dossierEyebrow: 'FRAGRANCE HOUSE DOSSIER · HERITAGE HAUTE PARFUMERIE',
        dnaBadge: 'Citrus & Mineral DNA',
        rating: 4.9,
        reviewsLabel: '3.6k reviews',
        description:
            'Guided by legendary in-house perfumers Jean-Claude Ellena and Christine Nagel, Hermès perfumery is an art of watercolor transparency, mineral earth, and noble leather — crafting timeless signatures that bridge nature and craftsmanship.',
        olfactoryDna: [
            { family: 'Citrus', notes: 'Grapefruit · Bitter Orange · Shiso', percentage: 90 },
            { family: 'Woody', notes: 'Atlas Cedar · Vetiver · Flint', percentage: 82 },
            { family: 'Fresh', notes: 'Green Mango · Lotus · Rhubarb', percentage: 67 },
        ],
        familyCounts: { All: 28, Citrus: 11, Woody: 9, Fresh: 5, Spicy: 3 },
        similarHouseIds: ['chanel', 'diptyque', 'le-labo', 'byredo', 'aesop', 'ds-durga'],
    },
    {
        id: 'chanel',
        name: 'CHANEL',
        displayName: 'Chanel',
        location: 'Paris · Est. 1910',
        count: '21 perfumes',
        archiveCount: 21,
        initials: 'CH',
        originLabel: 'RUE CAMBON · PARIS',
        establishedLabel: 'EST. 1910',
        dossierEyebrow: 'FRAGRANCE HOUSE DOSSIER · ICONIC FRENCH PARFUMERIE',
        dnaBadge: 'Aromatic & Woody DNA',
        rating: 4.8,
        reviewsLabel: '4.2k reviews',
        description:
            'From the couture salons of Rue Cambon to the flower fields of Grasse, Chanel defines French olfactory elegance. Blending crisp aromatics, luminous iris, and velvety woods with unmistakable architectural precision.',
        olfactoryDna: [
            { family: 'Fresh', notes: 'Juniper · Cypress · Lavender', percentage: 84 },
            { family: 'Woody', notes: 'Vetiver · New Caledonian Sandalwood', percentage: 80 },
            { family: 'Citrus', notes: 'Sicilian Lemon · Bergamot · Neroli', percentage: 71 },
        ],
        familyCounts: { All: 21, Fresh: 8, Woody: 6, Citrus: 4, Spicy: 3 },
        similarHouseIds: ['hermes', 'diptyque', 'byredo', 'le-labo', 'aesop', 'ds-durga'],
    },
    {
        id: 'aesop',
        name: 'AESOP',
        displayName: 'Aesop',
        location: 'Melbourne · Est. 1987',
        count: '11 perfumes',
        archiveCount: 11,
        initials: 'AE',
        originLabel: 'MELBOURNE · AUSTRALIA',
        establishedLabel: 'EST. 1987',
        dossierEyebrow: 'FRAGRANCE HOUSE DOSSIER · BOTANICAL & ARCHITECTURAL SCENTS',
        dnaBadge: 'Woody & Resinous DNA',
        rating: 4.8,
        reviewsLabel: '1.8k reviews',
        description:
            'Established in Melbourne in 1987, Aesop approaches perfumery as contemplative architecture. Created with Barnabé Fillion and Céline Barel, each scent pairs ancient botanicals, hinoki forests, and sacred resins with modern restraint.',
        olfactoryDna: [
            { family: 'Woody', notes: 'Hinoki · Cypress · Vetiver', percentage: 92 },
            { family: 'Spicy', notes: 'Frankincense · Clove · Cardamom', percentage: 78 },
            { family: 'Fresh', notes: 'Yuzu · Basil · Shiso Leaf', percentage: 65 },
        ],
        familyCounts: { All: 11, Woody: 5, Spicy: 3, Fresh: 2, Citrus: 1 },
        similarHouseIds: ['le-labo', 'diptyque', 'byredo', 'ds-durga', 'hermes', 'chanel'],
    },

    // Додаткові всесвітньо відомі нішеві та люксові доми
    { id: 'tom-ford', name: 'TOM FORD', location: 'New York · Est. 2005', count: '38 perfumes' },
    { id: 'mfk', name: 'MAISON FRANCIS KURKDJIAN', location: 'Paris · Est. 2009', count: '22 perfumes' },
    { id: 'creed', name: 'CREED', location: 'London · Est. 1760', count: '29 perfumes' },
    { id: 'guerlain', name: 'GUERLAIN', location: 'Paris · Est. 1828', count: '45 perfumes' },
    { id: 'jo-malone', name: 'JO MALONE', location: 'London · Est. 1990', count: '34 perfumes' },
    { id: 'kilian', name: 'KILIAN PARIS', location: 'Paris · Est. 2007', count: '26 perfumes' },
    { id: 'pdm', name: 'PARFUMS DE MARLY', location: 'Paris · Est. 2009', count: '18 perfumes' },
    { id: 'frederic-malle', name: 'ÉDITIONS DE PARFUMS', location: 'Paris · Est. 2000', count: '25 perfumes' },
    { id: 'serge-lutens', name: 'SERGE LUTENS', location: 'Paris · Est. 2000', count: '30 perfumes' },
    { id: 'acqua-di-parma', name: 'ACQUA DI PARMA', location: 'Parma · Est. 1916', count: '27 perfumes' },
    { id: 'penhaligons', name: 'PENHALIGON’S', location: 'London · Est. 1870', count: '31 perfumes' },
    { id: 'nishane', name: 'NISHANE', location: 'Istanbul · Est. 2012', count: '16 perfumes' },
    { id: 'amouage', name: 'AMOUAGE', location: 'Muscat · Est. 1983', count: '33 perfumes' },
    { id: 'margiela', name: 'MAISON MARGIELA', location: 'Paris · Est. 2010', count: '20 perfumes' },
    { id: 'xerjoff', name: 'XERJOFF', location: 'Turin · Est. 2003', count: '35 perfumes' },
    { id: 'roja', name: 'ROJA PARFUMS', location: 'London · Est. 2011', count: '15 perfumes' },
    { id: 'vilhelm', name: 'VILHELM PARFUMERIE', location: 'New York · Est. 2015', count: '14 perfumes' },
    { id: 'bdk', name: 'BDK PARFUMS', location: 'Paris · Est. 2016', count: '17 perfumes' },
    { id: 'memo', name: 'MEMO PARIS', location: 'Paris · Est. 2007', count: '23 perfumes' },
    { id: 'initio', name: 'INITIO PARFUMS PRIVÉS', location: 'Paris · Est. 2015', count: '16 perfumes' },
    { id: 'matiere-premiere', name: 'MATIERE PREMIERE', location: 'Paris · Est. 2019', count: '12 perfumes' },
    { id: 'diur', name: 'DIOR', location: 'Paris · Est. 1947', count: '40 perfumes' },
    { id: 'saint-laurent', name: 'YVES SAINT LAURENT', location: 'Paris · Est. 1961', count: '36 perfumes' },
    { id: 'cartier', name: 'CARTIER', location: 'Paris · Est. 1847', count: '18 perfumes' },
    { id: 'prada', name: 'PRADA', location: 'Milan · Est. 1913', count: '22 perfumes' },
    { id: 'giorgio-armani', name: 'GIORGIO ARMANI', location: 'Milan · Est. 1975', count: '35 perfumes' },
    { id: 'givenchy', name: 'GIVENCHY', location: 'Paris · Est. 1952', count: '29 perfumes' },
    { id: 'santa-maria-novella', name: 'SANTA MARIA NOVELLA', location: 'Florence · Est. 1221', count: '15 perfumes' },
    { id: 'l-artisan', name: 'L’ARTISAN PARFUMEUR', location: 'Paris · Est. 1976', count: '28 perfumes' },
    { id: 'floraiku', name: 'FLORAIKU', location: 'Paris · Est. 2017', count: '13 perfumes' },
    { id: 'ex-nihilo', name: 'EX NIHILO', location: 'Paris · Est. 2013', count: '19 perfumes' },
    { id: 'clive-christian', name: 'CLIVE CHRISTIAN', location: 'London · Est. 1999', count: '14 perfumes' },
    { id: 'atkinsons', name: 'ATKINSONS', location: 'London · Est. 1799', count: '21 perfumes' },
    { id: 'ormonde-jayne', name: 'ORMONDE JAYNE', location: 'London · Est. 2002', count: '18 perfumes' },
    { id: 'parle-moi', name: 'PARLE MOI DE PARFUM', location: 'Paris · Est. 2016', count: '12 perfumes' },
    {
        id: 'ds-durga',
        name: 'DS & DURGA',
        displayName: 'D.S. & Durga',
        location: 'Brooklyn · Est. 2007',
        count: '18 perfumes',
        archiveCount: 18,
        initials: 'DS',
        originLabel: 'BROOKLYN · NEW YORK',
        establishedLabel: 'EST. 2007',
        dossierEyebrow: 'FRAGRANCE HOUSE DOSSIER · ARTISANAL FRONTIER PERFUMERY',
        dnaBadge: 'Woody & Aromatic DNA',
        rating: 4.7,
        reviewsLabel: '1.4k reviews',
        description:
            'Founded in Brooklyn by self-taught perfumer David Seth Moltz and architect Kavi Moltz, D.S. & Durga buildsinvisible sonic and narrative worlds in fragrance — evoking American folkloric landscapes, resinous woods, and desert botanicals.',
        olfactoryDna: [
            { family: 'Woody', notes: 'Cedar · Sandalwood · Birch Tar', percentage: 84 },
            { family: 'Spicy', notes: 'Coriander · Amber · Cardamom', percentage: 72 },
            { family: 'Fresh', notes: 'Fig · Sagebrush · Vetiver', percentage: 66 },
        ],
        familyCounts: { All: 18, Woody: 8, Spicy: 5, Fresh: 3, Citrus: 2 },
        similarHouseIds: ['le-labo', 'byredo', 'diptyque', 'aesop', 'hermes', 'chanel'],
    },
    { id: 'fueguia', name: 'FUEGUIA 1833', location: 'Buenos Aires · Est. 2010', count: '22 perfumes' },
    { id: 'boadicea', name: 'BOADICEA THE VICTORIOUS', location: 'London · Est. 2008', count: '25 perfumes' },
    { id: 'sospiro', name: 'SOSPIRO', location: 'Turin · Est. 2011', count: '16 perfumes' },
    { id: 'nasomatto', name: 'NASOMATTO', location: 'Amsterdam · Est. 2007', count: '11 perfumes' },
    { id: 'zoologist', name: 'ZOOLOGIST PERFUMES', location: 'Toronto · Est. 2013', count: '15 perfumes' },
    { id: 'essential-parfums', name: 'ESSENTIAL PARFUMS', location: 'Paris · Est. 2018', count: '10 perfumes' },
];

const toTitleCase = (str: string) =>
    str
        .toLowerCase()
        .split(' ')
        .map((word) => (word.length > 0 ? word[0].toUpperCase() + word.slice(1) : word))
        .join(' ');

export const getHouseByIdOrName = (identifier?: string): HouseItem => {
    if (!identifier) return HOUSES_DATA[1]; // Default Le Labo

    const normalized = decodeURIComponent(identifier).trim().toLowerCase();
    const found =
        HOUSES_DATA.find((h) => h.id.toLowerCase() === normalized) ||
        HOUSES_DATA.find((h) => h.name.toLowerCase() === normalized) ||
        HOUSES_DATA.find((h) => h.displayName?.toLowerCase() === normalized);

    const base = found || HOUSES_DATA[1];
    const [cityPart = 'Paris', estPart = 'Est. 2006'] = base.location.split('·').map((s) => s.trim());
    const parsedCount = parseInt(base.count, 10) || 19;
    const words = base.name.replace(/[^A-Za-zÀ-ÿ\s]/g, '').trim().split(/\s+/);
    const computedInitials =
        words.length >= 2
            ? `${words[0][0]}${words[1][0]}`.toUpperCase()
            : base.name.slice(0, 2).toUpperCase();

    return {
        ...base,
        displayName: base.displayName || toTitleCase(base.name),
        archiveCount: base.archiveCount || parsedCount,
        initials: base.initials || computedInitials,
        originLabel: base.originLabel || `GRASSE · ${cityPart.toUpperCase()}`,
        establishedLabel: base.establishedLabel || estPart.toUpperCase(),
        dossierEyebrow:
            base.dossierEyebrow || 'FRAGRANCE HOUSE DOSSIER · NICHE ARTISANAL PERFUMERY',
        dnaBadge: base.dnaBadge || 'Woody & Spicy DNA',
        rating: base.rating || 4.8,
        reviewsLabel: base.reviewsLabel || '1.9k reviews',
        description:
            base.description ||
            `Founded in ${cityPart} (${estPart}), ${base.displayName || toTitleCase(base.name)
            } crafts distinctive, slow perfumery rooted in noble raw materials — celebrating refined woods, aromatic botanicals, and skin-close accords.`,
        olfactoryDna: base.olfactoryDna || [
            { family: 'Woody', notes: 'Sandalwood · Cedar · Vetiver', percentage: 86 },
            { family: 'Spicy', notes: 'Cardamom · Black Tea · Amber', percentage: 73 },
            { family: 'Fresh', notes: 'Bergamot · Fig Leaf · Neroli', percentage: 64 },
        ],
        familyCounts: base.familyCounts || {
            All: parsedCount,
            Woody: Math.max(2, Math.round(parsedCount * 0.42)),
            Spicy: Math.max(1, Math.round(parsedCount * 0.26)),
            Fresh: Math.max(1, Math.round(parsedCount * 0.21)),
            Citrus: Math.max(1, Math.round(parsedCount * 0.11)),
        },
    };
};

export const getSimilarHouses = (currentHouse: HouseItem): HouseItem[] => {
    const defaultSimilarIds = ['byredo', 'diptyque', 'aesop', 'hermes', 'chanel', 'ds-durga'];
    const targetIds = (currentHouse.similarHouseIds || defaultSimilarIds).filter(
        (id) => id !== currentHouse.id
    );

    const resolved: HouseItem[] = [];
    for (const id of targetIds) {
        const match = HOUSES_DATA.find((h) => h.id === id);
        if (match && resolved.length < 6) {
            resolved.push(match);
        }
    }

    if (resolved.length < 6) {
        for (const h of HOUSES_DATA) {
            if (h.id !== currentHouse.id && !resolved.some((r) => r.id === h.id)) {
                resolved.push(h);
                if (resolved.length === 6) break;
            }
        }
    }

    return resolved;
};
