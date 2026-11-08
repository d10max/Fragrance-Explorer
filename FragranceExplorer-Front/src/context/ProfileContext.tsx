import { createContext, useContext, useState, useEffect, type ReactNode } from 'react';
import { PERFUMES_COLLECTION, type PerfumeItem } from '@/pages/perfume-catalog-page/perfumes.data';

export interface UserProfile {
    name: string;
    email: string;
    bio: string;
    initials: string;
    memberSince: string;
    signatureScentId: string;
    level: string;
}

export interface FamilyStat {
    name: 'Woody' | 'Citrus' | 'Fresh' | 'Spicy';
    count: number;
    percentage: number;
    color: string;
    bgBadge: string;
    textBadge: string;
    description: string;
}

export interface NoteStat {
    name: string;
    count: number;
    percentage: number;
    category: string;
}

export interface RadarDimension {
    label: string;
    value: number; // 0 - 100
    maxValue: number;
}

export interface TasteAnalytics {
    hasData: boolean;
    totalFavorites: number;
    dominantFamily: string;
    dominantNote: string;
    vibePersona: string;
    vibeDescription: string;
    families: FamilyStat[];
    topNotes: NoteStat[];
    radarData: RadarDimension[];
    exploredCount: number;
    comparisonsCount: number;
}

export interface DiscoveryHistoryItem {
    id: string;
    query: string;
    mode: 'balanced' | 'clones' | 'similar' | 'catalog';
    date: string;
    resultCount: number;
}

interface ProfileContextType {
    user: UserProfile | null;
    isAuthenticated: boolean;
    updateUser: (updates: Partial<UserProfile>) => void;
    register: (data: { name: string; email: string; level?: string; bio?: string }) => void;
    login: (email: string) => boolean;
    logout: () => void;
    loadDemoProfile: () => void;
    favoriteIds: string[];
    favoritePerfumes: PerfumeItem[];
    addFavorite: (id: string) => void;
    removeFavorite: (id: string) => void;
    toggleFavorite: (id: string) => void;
    isFavorite: (id: string) => boolean;
    analytics: TasteAnalytics;
    history: DiscoveryHistoryItem[];
    clearHistory: () => void;
    isAuthModalOpen: boolean;
    openAuthModal: () => void;
    closeAuthModal: () => void;
    requireAuth: (callback: () => void) => void;
}

const DEMO_USER: UserProfile = {
    name: 'Elena Vance',
    email: 'elena.vance@fragrance.io',
    bio: 'Niche fragrance connoisseur & olfactory minimalist. Devoted to warm woods, sunlit bergamot, and smoky vetivers.',
    initials: 'EV',
    memberSince: 'October 2024',
    signatureScentId: 'bal-dafrique',
    level: 'Fragrance Connoisseur',
};

const DEMO_FAVORITES: string[] = [
    'bal-dafrique',
    'santal-33',
    'gypsy-water',
    'terre-d-hermes',
    'vetiver-extraordinaire',
    'aventus',
];

const DEMO_HISTORY: DiscoveryHistoryItem[] = [
    {
        id: 'h-1',
        query: "Bal d'Afrique (BYREDO)",
        mode: 'balanced',
        date: 'Today, 18:42',
        resultCount: 10,
    },
    {
        id: 'h-2',
        query: 'Woody & Vetiver search',
        mode: 'catalog',
        date: 'Yesterday, 14:15',
        resultCount: 24,
    },
    {
        id: 'h-3',
        query: 'Santal 33 vs Terre d’Hermès',
        mode: 'similar',
        date: 'Nov 04, 2026',
        resultCount: 2,
    },
    {
        id: 'h-4',
        query: 'Exact note clones for Aventus',
        mode: 'clones',
        date: 'Nov 02, 2026',
        resultCount: 8,
    },
];

const CURRENT_USER_STORAGE_KEY = 'fe_current_user_session';
const REGISTERED_USERS_KEY = 'fe_registered_users_registry';

const ProfileContext = createContext<ProfileContextType | undefined>(undefined);

function calculateInitials(name: string): string {
    const parts = name.trim().split(/\s+/);
    if (parts.length === 0 || !parts[0]) return 'FE';
    if (parts.length === 1) return parts[0].substring(0, 2).toUpperCase();
    return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
}

export const ProfileProvider = ({ children }: { children: ReactNode }) => {
    // Current user state (null if guest)
    const [user, setUser] = useState<UserProfile | null>(() => {
        try {
            const saved = localStorage.getItem(CURRENT_USER_STORAGE_KEY);
            return saved ? JSON.parse(saved) : null;
        } catch {
            return null;
        }
    });

    const [favoriteIds, setFavoriteIds] = useState<string[]>(() => {
        try {
            if (!user) return [];
            const key = `fe_user_favorites_${user.email}`;
            const saved = localStorage.getItem(key);
            return saved ? JSON.parse(saved) : [];
        } catch {
            return [];
        }
    });

    const [history, setHistory] = useState<DiscoveryHistoryItem[]>(() => {
        try {
            if (!user) return [];
            const key = `fe_user_history_${user.email}`;
            const saved = localStorage.getItem(key);
            return saved ? JSON.parse(saved) : [];
        } catch {
            return [];
        }
    });

    const [isAuthModalOpen, setIsAuthModalOpen] = useState(false);

    // Sync session and user favorites
    useEffect(() => {
        try {
            if (user) {
                localStorage.setItem(CURRENT_USER_STORAGE_KEY, JSON.stringify(user));
                // Load favorites for this specific user
                const key = `fe_user_favorites_${user.email}`;
                const savedFavs = localStorage.getItem(key);
                if (savedFavs) {
                    setFavoriteIds(JSON.parse(savedFavs));
                } else if (user.email === DEMO_USER.email) {
                    setFavoriteIds(DEMO_FAVORITES);
                } else {
                    setFavoriteIds([]);
                }

                // Load history for this specific user
                const histKey = `fe_user_history_${user.email}`;
                const savedHist = localStorage.getItem(histKey);
                if (savedHist) {
                    setHistory(JSON.parse(savedHist));
                } else if (user.email === DEMO_USER.email) {
                    setHistory(DEMO_HISTORY);
                } else {
                    setHistory([]);
                }
            } else {
                localStorage.removeItem(CURRENT_USER_STORAGE_KEY);
                setFavoriteIds([]);
                setHistory([]);
            }
        } catch (e) {
            console.error('Session sync error:', e);
        }
    }, [user?.email]);

    // Save favorites whenever they change for the logged-in user
    useEffect(() => {
        try {
            if (user) {
                const key = `fe_user_favorites_${user.email}`;
                localStorage.setItem(key, JSON.stringify(favoriteIds));
            }
        } catch (e) {
            console.error('Favorites sync error:', e);
        }
    }, [favoriteIds, user?.email]);

    const openAuthModal = () => setIsAuthModalOpen(true);
    const closeAuthModal = () => setIsAuthModalOpen(false);

    const requireAuth = (callback: () => void) => {
        if (user) {
            callback();
        } else {
            setIsAuthModalOpen(true);
        }
    };

    const register = (data: {
        name: string;
        email: string;
        level?: string;
        bio?: string;
    }) => {
        const currentDate = new Date();
        const monthNames = [
            'January', 'February', 'March', 'April', 'May', 'June',
            'July', 'August', 'September', 'October', 'November', 'December'
        ];
        const memberSince = `${monthNames[currentDate.getMonth()]} ${currentDate.getFullYear()}`;

        const newUser: UserProfile = {
            name: data.name.trim(),
            email: data.email.trim().toLowerCase(),
            bio:
                data.bio?.trim() ||
                'Niche fragrance enthusiast crafting a personal olfactory wardrobe.',
            initials: calculateInitials(data.name),
            memberSince,
            signatureScentId: '',
            level: data.level || 'Fragrance Enthusiast',
        };

        // Save into local registry
        try {
            const registryRaw = localStorage.getItem(REGISTERED_USERS_KEY);
            const registry: Record<string, UserProfile> = registryRaw
                ? JSON.parse(registryRaw)
                : {};
            registry[newUser.email] = newUser;
            localStorage.setItem(REGISTERED_USERS_KEY, JSON.stringify(registry));
        } catch {}

        setUser(newUser);
        setFavoriteIds([]);
        setHistory([
            {
                id: `h-${Date.now()}`,
                query: 'Welcome to Fragrance Explorer',
                mode: 'catalog',
                date: 'Just now',
                resultCount: 1,
            },
        ]);
        closeAuthModal();
    };

    const login = (email: string): boolean => {
        const normalized = email.trim().toLowerCase();
        if (normalized === DEMO_USER.email) {
            loadDemoProfile();
            closeAuthModal();
            return true;
        }

        try {
            const registryRaw = localStorage.getItem(REGISTERED_USERS_KEY);
            if (registryRaw) {
                const registry: Record<string, UserProfile> = JSON.parse(registryRaw);
                if (registry[normalized]) {
                    setUser(registry[normalized]);
                    closeAuthModal();
                    return true;
                }
            }
        } catch {}

        // Fallback: create account for convenience if first time
        register({
            name: email.split('@')[0].replace(/[._-]/g, ' '),
            email: normalized,
        });
        return true;
    };

    const logout = () => {
        setUser(null);
        setFavoriteIds([]);
        setHistory([]);
    };

    const loadDemoProfile = () => {
        setUser(DEMO_USER);
        setFavoriteIds(DEMO_FAVORITES);
        setHistory(DEMO_HISTORY);
        closeAuthModal();
    };

    const updateUser = (updates: Partial<UserProfile>) => {
        if (!user) return;
        setUser((prev) => {
            if (!prev) return null;
            const updated = { ...prev, ...updates };
            if (updates.name && (!updates.initials || updates.initials === prev.initials)) {
                updated.initials = calculateInitials(updates.name);
            }
            return updated;
        });
    };

    const addFavorite = (id: string) => {
        if (!user) {
            openAuthModal();
            return;
        }
        setFavoriteIds((prev) => (prev.includes(id) ? prev : [...prev, id]));
    };

    const removeFavorite = (id: string) => {
        if (!user) return;
        setFavoriteIds((prev) => prev.filter((item) => item !== id));
    };

    const toggleFavorite = (id: string) => {
        if (!user) {
            openAuthModal();
            return;
        }
        setFavoriteIds((prev) =>
            prev.includes(id) ? prev.filter((item) => item !== id) : [...prev, id]
        );
    };

    const isFavorite = (id: string) => Boolean(user && favoriteIds.includes(id));

    const clearHistory = () => {
        setHistory([]);
        if (user) {
            try {
                localStorage.removeItem(`fe_user_history_${user.email}`);
            } catch {}
        }
    };

    // Calculate real perfume items from favoriteIds
    const favoritePerfumes: PerfumeItem[] = favoriteIds
        .map((id) => PERFUMES_COLLECTION.find((p) => p.id === id))
        .filter((p): p is PerfumeItem => Boolean(p));

    // Dynamic taste analysis strictly based on user's saved perfumes
    const analytics: TasteAnalytics = (() => {
        // If user has 0 saved perfumes (e.g. freshly registered)
        if (favoritePerfumes.length === 0) {
            return {
                hasData: false,
                totalFavorites: 0,
                dominantFamily: '—',
                dominantNote: '—',
                vibePersona: 'Olfactory Novice',
                vibeDescription:
                    'Your signature scent archetype and visual taste radar will activate as you discover and save fragrances to your wardrobe.',
                families: [
                    {
                        name: 'Woody',
                        count: 0,
                        percentage: 0,
                        color: '#c59b27',
                        bgBadge: '#eee9e2',
                        textBadge: '#8b7963',
                        description: 'Cedarwood, Sandalwood, Vetiver & Dry Woods',
                    },
                    {
                        name: 'Citrus',
                        count: 0,
                        percentage: 0,
                        color: '#d4a32e',
                        bgBadge: '#f5eed9',
                        textBadge: '#b89737',
                        description: 'Bergamot, Bitter Orange, Neroli & Zest',
                    },
                    {
                        name: 'Fresh',
                        count: 0,
                        percentage: 0,
                        color: '#77958a',
                        bgBadge: '#eaf0ec',
                        textBadge: '#77958a',
                        description: 'Cardamom, Green Fig, Aquatic & Herbal Notes',
                    },
                    {
                        name: 'Spicy',
                        count: 0,
                        percentage: 0,
                        color: '#b67c67',
                        bgBadge: '#f5e7e1',
                        textBadge: '#b67c67',
                        description: 'Pink Pepper, Tonka Bean, Saffron & Warm Amber',
                    },
                ],
                topNotes: [],
                radarData: [
                    { label: 'Woody & Earthy', value: 0, maxValue: 100 },
                    { label: 'Citrus & Bright', value: 0, maxValue: 100 },
                    { label: 'Fresh Green', value: 0, maxValue: 100 },
                    { label: 'Resinous Amber', value: 0, maxValue: 100 },
                    { label: 'Spicy Warmth', value: 0, maxValue: 100 },
                    { label: 'Soft Floral', value: 0, maxValue: 100 },
                ],
                exploredCount: 0,
                comparisonsCount: 0,
            };
        }

        // Real calculations when user has 1+ saved perfumes
        const familyCounts: Record<'Woody' | 'Citrus' | 'Fresh' | 'Spicy', number> = {
            Woody: 0,
            Citrus: 0,
            Fresh: 0,
            Spicy: 0,
        };

        favoritePerfumes.forEach((p) => {
            if (p.family in familyCounts) {
                familyCounts[p.family as keyof typeof familyCounts]++;
            } else {
                familyCounts.Woody++;
            }
        });

        const totalCount = favoritePerfumes.length;
        const woodyPct = Math.round((familyCounts.Woody / totalCount) * 100);
        const citrusPct = Math.round((familyCounts.Citrus / totalCount) * 100);
        const freshPct = Math.round((familyCounts.Fresh / totalCount) * 100);
        const spicyPct = Math.max(0, 100 - (woodyPct + citrusPct + freshPct));

        const families: FamilyStat[] = [
            {
                name: 'Woody',
                count: familyCounts.Woody,
                percentage: woodyPct,
                color: '#c59b27',
                bgBadge: '#eee9e2',
                textBadge: '#8b7963',
                description: 'Cedarwood, Sandalwood, Vetiver & Dry Woods',
            },
            {
                name: 'Citrus',
                count: familyCounts.Citrus,
                percentage: citrusPct,
                color: '#d4a32e',
                bgBadge: '#f5eed9',
                textBadge: '#b89737',
                description: 'Bergamot, Bitter Orange, Neroli & Zest',
            },
            {
                name: 'Fresh',
                count: familyCounts.Fresh,
                percentage: freshPct,
                color: '#77958a',
                bgBadge: '#eaf0ec',
                textBadge: '#77958a',
                description: 'Cardamom, Green Fig, Aquatic & Herbal Notes',
            },
            {
                name: 'Spicy',
                count: familyCounts.Spicy,
                percentage: spicyPct,
                color: '#b67c67',
                bgBadge: '#f5e7e1',
                textBadge: '#b67c67',
                description: 'Pink Pepper, Tonka Bean, Saffron & Warm Amber',
            },
        ];

        // Notes frequency strictly from real saved perfumes
        const noteCounts: Record<string, number> = {};
        favoritePerfumes.forEach((p) => {
            p.keyNotes.forEach((n) => {
                noteCounts[n] = (noteCounts[n] || 0) + 1;
            });
        });

        const sortedNotes = Object.entries(noteCounts)
            .sort((a, b) => b[1] - a[1])
            .slice(0, 6);

        const topNotes: NoteStat[] = sortedNotes.map(([name, count]) => ({
            name,
            count,
            percentage: Math.min(100, Math.round((count / totalCount) * 100)),
            category:
                name.includes('Cedar') || name.includes('Vetiver') || name.includes('Sandalwood')
                    ? 'Woody'
                    : name.includes('Bergamot') || name.includes('Orange') || name.includes('Lemon')
                    ? 'Citrus'
                    : name.includes('Pepper') || name.includes('Saffron')
                    ? 'Spicy'
                    : 'Fresh',
        }));

        // Sort families to find dominant one
        const sortedFamilies = [...families].sort((a, b) => b.count - a.count);
        const dominant = sortedFamilies[0];

        // Radar chart dimensions derived from their real preferences
        const radarData: RadarDimension[] = [
            { label: 'Woody & Earthy', value: Math.min(100, Math.round(woodyPct * 0.9) + (woodyPct > 0 ? 15 : 5)), maxValue: 100 },
            { label: 'Citrus & Bright', value: Math.min(100, Math.round(citrusPct * 0.9) + (citrusPct > 0 ? 15 : 5)), maxValue: 100 },
            { label: 'Fresh Green', value: Math.min(100, Math.round(freshPct * 0.9) + (freshPct > 0 ? 15 : 5)), maxValue: 100 },
            { label: 'Resinous Amber', value: Math.min(100, Math.round((woodyPct + spicyPct) * 0.5) + 10), maxValue: 100 },
            { label: 'Spicy Warmth', value: Math.min(100, Math.round(spicyPct * 0.9) + (spicyPct > 0 ? 15 : 5)), maxValue: 100 },
            { label: 'Soft Floral', value: 20, maxValue: 100 },
        ];

        return {
            hasData: true,
            totalFavorites: totalCount,
            dominantFamily: dominant.percentage > 0 ? dominant.name : '—',
            dominantNote: topNotes[0]?.name || '—',
            vibePersona:
                woodyPct >= 50
                    ? 'The Aristocratic Minimalist'
                    : citrusPct >= 40
                    ? 'The Sunlit Mediterranean Virtuoso'
                    : freshPct >= 40
                    ? 'The Crisp Botanical Purist'
                    : 'The Modern Eclectic Connoisseur',
            vibeDescription: `Your personal edit is led by ${dominant.name.toLowerCase()} accords (${dominant.percentage}%), featuring distinct notes of ${topNotes.slice(0, 3).map((n) => n.name).join(', ')}.`,
            families,
            topNotes,
            radarData,
            exploredCount: totalCount * 4 + 8,
            comparisonsCount: Math.min(totalCount * 2, 8),
        };
    })();

    return (
        <ProfileContext.Provider
            value={{
                user,
                isAuthenticated: Boolean(user),
                updateUser,
                register,
                login,
                logout,
                loadDemoProfile,
                favoriteIds,
                favoritePerfumes,
                addFavorite,
                removeFavorite,
                toggleFavorite,
                isFavorite,
                analytics,
                history,
                clearHistory,
                isAuthModalOpen,
                openAuthModal,
                closeAuthModal,
                requireAuth,
            }}
        >
            {children}
        </ProfileContext.Provider>
    );
};

export const useProfile = () => {
    const context = useContext(ProfileContext);
    if (!context) {
        throw new Error('useProfile must be used within a ProfileProvider');
    }
    return context;
};
