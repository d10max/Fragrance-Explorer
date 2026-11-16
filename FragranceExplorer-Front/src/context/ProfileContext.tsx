import {
    createContext,
    useContext,
    useState,
    useEffect,
    useMemo,
    useCallback,
    type ReactNode,
} from 'react';
import {
    PERFUMES_COLLECTION,
    type PerfumeItem,
} from '@/pages/perfume-catalog-page/perfumes.data';

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

export interface AuthActionResult {
    success: boolean;
    error?: string;
}

interface RegisteredUserEntry {
    profile: UserProfile;
    passwordHash?: string;
    password?: string; // legacy support
}

export interface UpdateUserPayload extends Partial<UserProfile> {
    currentPassword?: string;
    newPassword?: string;
}

interface ProfileContextType {
    user: UserProfile | null;
    isAuthenticated: boolean;
    updateUser: (updates: UpdateUserPayload) => AuthActionResult;
    register: (data: {
        name: string;
        email: string;
        password?: string;
        level?: string;
        bio?: string;
    }) => AuthActionResult;
    login: (email: string, password?: string) => boolean;
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
    addHistoryEntry: (entry: Omit<DiscoveryHistoryItem, 'id' | 'date'>) => void;
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

const BASE_FAMILIES_META: Omit<FamilyStat, 'count' | 'percentage'>[] = [
    {
        name: 'Woody',
        color: '#c59b27',
        bgBadge: '#eee9e2',
        textBadge: '#8b7963',
        description: 'Cedarwood, Sandalwood, Vetiver & Dry Woods',
    },
    {
        name: 'Citrus',
        color: '#d4a32e',
        bgBadge: '#f5eed9',
        textBadge: '#b89737',
        description: 'Bergamot, Bitter Orange, Neroli & Zest',
    },
    {
        name: 'Fresh',
        color: '#77958a',
        bgBadge: '#eaf0ec',
        textBadge: '#77958a',
        description: 'Cardamom, Green Fig, Aquatic & Herbal Notes',
    },
    {
        name: 'Spicy',
        color: '#b67c67',
        bgBadge: '#f5e7e1',
        textBadge: '#b67c67',
        description: 'Pink Pepper, Tonka Bean, Saffron & Warm Amber',
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

// Deterministic client-side salted hash so passwords are never stored in raw plaintext
function hashPassword(password: string, customSalt = 'fe_global_salt_v1'): string {
    const input = `${customSalt.toLowerCase()}::fe_salt_v1::${password}`;
    let h1 = 0xdeadbeef ^ input.length;
    let h2 = 0x41c6ce57 ^ input.length;
    for (let i = 0; i < input.length; i++) {
        const ch = input.charCodeAt(i);
        h1 = Math.imul(h1 ^ ch, 2654435761);
        h2 = Math.imul(h2 ^ ch, 1597334677);
    }
    h1 = Math.imul(h1 ^ (h1 >>> 16), 2246822507) ^ Math.imul(h2 ^ (h2 >>> 13), 3266489909);
    h2 = Math.imul(h2 ^ (h2 >>> 16), 2246822507) ^ Math.imul(h1 ^ (h1 >>> 13), 3266489909);
    return `fe_h1_${(h2 >>> 0).toString(16).padStart(8, '0')}${(h1 >>> 0).toString(16).padStart(8, '0')}`;
}

function verifyStoredPassword(
    entry: RegisteredUserEntry,
    inputPassword: string,
    email: string
): boolean {
    if (entry.passwordHash) {
        return (
            entry.passwordHash === hashPassword(inputPassword) ||
            entry.passwordHash === hashPassword(inputPassword, email)
        );
    }
    if (entry.password !== undefined) {
        return entry.password === inputPassword;
    }
    return true;
}

function readStoredUser(): UserProfile | null {
    try {
        const saved = localStorage.getItem(CURRENT_USER_STORAGE_KEY);
        return saved ? (JSON.parse(saved) as UserProfile) : null;
    } catch (err) {
        console.warn('Failed to read user session:', err);
        return null;
    }
}

function readUserFavorites(email: string): string[] {
    try {
        const saved = localStorage.getItem(`fe_user_favorites_${email}`);
        if (saved) return JSON.parse(saved) as string[];
        return email === DEMO_USER.email ? DEMO_FAVORITES : [];
    } catch (err) {
        console.warn('Failed to read user favorites:', err);
        return [];
    }
}

function readUserHistory(email: string): DiscoveryHistoryItem[] {
    try {
        const saved = localStorage.getItem(`fe_user_history_${email}`);
        if (saved) return JSON.parse(saved) as DiscoveryHistoryItem[];
        return email === DEMO_USER.email ? DEMO_HISTORY : [];
    } catch (err) {
        console.warn('Failed to read user history:', err);
        return [];
    }
}

function readRegistry(): Record<string, RegisteredUserEntry> {
    try {
        const raw = localStorage.getItem(REGISTERED_USERS_KEY);
        return raw ? (JSON.parse(raw) as Record<string, RegisteredUserEntry>) : {};
    } catch (err) {
        console.warn('Failed to read user registry:', err);
        return {};
    }
}

function formatNowLabel(): string {
    const now = new Date();
    const hours = String(now.getHours()).padStart(2, '0');
    const mins = String(now.getMinutes()).padStart(2, '0');
    return `Today, ${hours}:${mins}`;
}

export const ProfileProvider = ({ children }: { children: ReactNode }) => {
    const [user, setUser] = useState<UserProfile | null>(() => readStoredUser());

    const [favoriteIds, setFavoriteIds] = useState<string[]>(() => {
        const initialUser = readStoredUser();
        return initialUser ? readUserFavorites(initialUser.email) : [];
    });

    const [history, setHistory] = useState<DiscoveryHistoryItem[]>(() => {
        const initialUser = readStoredUser();
        return initialUser ? readUserHistory(initialUser.email) : [];
    });

    const [isAuthModalOpen, setIsAuthModalOpen] = useState(false);

    // Persist active user session to localStorage
    useEffect(() => {
        try {
            if (user) {
                localStorage.setItem(CURRENT_USER_STORAGE_KEY, JSON.stringify(user));
            } else {
                localStorage.removeItem(CURRENT_USER_STORAGE_KEY);
            }
        } catch (err) {
            console.error('Session sync error:', err);
        }
    }, [user]);

    // Persist user favorites to localStorage
    useEffect(() => {
        try {
            if (user) {
                localStorage.setItem(
                    `fe_user_favorites_${user.email}`,
                    JSON.stringify(favoriteIds)
                );
            }
        } catch (err) {
            console.error('Favorites sync error:', err);
        }
    }, [favoriteIds, user]);

    // Persist user discovery history to localStorage
    useEffect(() => {
        try {
            if (user) {
                localStorage.setItem(
                    `fe_user_history_${user.email}`,
                    JSON.stringify(history)
                );
            }
        } catch (err) {
            console.error('History sync error:', err);
        }
    }, [history, user]);

    const openAuthModal = useCallback(() => setIsAuthModalOpen(true), []);
    const closeAuthModal = useCallback(() => setIsAuthModalOpen(false), []);

    const requireAuth = useCallback(
        (callback: () => void) => {
            if (user) {
                callback();
            } else {
                setIsAuthModalOpen(true);
            }
        },
        [user]
    );

    const addHistoryEntry = useCallback(
        (entry: Omit<DiscoveryHistoryItem, 'id' | 'date'>) => {
            if (!user) return;
            setHistory((prev) => {
                const deduplicated = prev.filter(
                    (item) => !(item.query === entry.query && item.mode === entry.mode)
                );
                const newItem: DiscoveryHistoryItem = {
                    ...entry,
                    id: `h-${Date.now()}-${Math.random().toString(36).slice(2, 6)}`,
                    date: formatNowLabel(),
                };
                return [newItem, ...deduplicated].slice(0, 25);
            });
        },
        [user]
    );

    const register = useCallback(
        (data: {
            name: string;
            email: string;
            password?: string;
            level?: string;
            bio?: string;
        }): AuthActionResult => {
            const normalizedEmail = data.email.trim().toLowerCase();
            if (!normalizedEmail || !data.name.trim()) {
                return {
                    success: false,
                    error: 'Please provide both your name and a valid email address.',
                };
            }

            const registry = readRegistry();
            if (normalizedEmail === DEMO_USER.email || registry[normalizedEmail]) {
                return {
                    success: false,
                    error: 'An account with this email already exists. Please switch to Sign In.',
                };
            }

            const currentDate = new Date();
            const monthNames = [
                'January', 'February', 'March', 'April', 'May', 'June',
                'July', 'August', 'September', 'October', 'November', 'December',
            ];
            const memberSince = `${monthNames[currentDate.getMonth()]} ${currentDate.getFullYear()}`;

            const newUser: UserProfile = {
                name: data.name.trim(),
                email: normalizedEmail,
                bio:
                    data.bio?.trim() ||
                    'Niche fragrance enthusiast crafting a personal olfactory wardrobe.',
                initials: calculateInitials(data.name),
                memberSince,
                signatureScentId: '',
                level: data.level || 'Fragrance Enthusiast',
            };

            const initialHistory: DiscoveryHistoryItem[] = [
                {
                    id: `h-${Date.now()}`,
                    query: 'Joined Fragrance Explorer Connoisseur Club',
                    mode: 'catalog',
                    date: formatNowLabel(),
                    resultCount: 128,
                },
            ];

            try {
                registry[newUser.email] = {
                    profile: newUser,
                    passwordHash: hashPassword(data.password || ''),
                };
                localStorage.setItem(REGISTERED_USERS_KEY, JSON.stringify(registry));
                localStorage.setItem(`fe_user_favorites_${newUser.email}`, JSON.stringify([]));
                localStorage.setItem(
                    `fe_user_history_${newUser.email}`,
                    JSON.stringify(initialHistory)
                );
            } catch (err) {
                console.error('Failed to save registered user:', err);
            }

            setUser(newUser);
            setFavoriteIds([]);
            setHistory(initialHistory);
            closeAuthModal();
            return { success: true };
        },
        [closeAuthModal]
    );

    const loadDemoProfile = useCallback(() => {
        const demoFavs = readUserFavorites(DEMO_USER.email);
        const demoHist = readUserHistory(DEMO_USER.email);
        setUser(DEMO_USER);
        setFavoriteIds(demoFavs.length > 0 ? demoFavs : DEMO_FAVORITES);
        setHistory(demoHist.length > 0 ? demoHist : DEMO_HISTORY);
        closeAuthModal();
    }, [closeAuthModal]);

    const login = useCallback(
        (email: string, password?: string): boolean => {
            const normalized = email.trim().toLowerCase();
            if (!password) return false;

            const registry = readRegistry();
            const entry = registry[normalized];
            if (entry) {
                const profile: UserProfile = entry.profile;
                if (!verifyStoredPassword(entry, password, normalized)) {
                    return false;
                }

                setUser(profile);
                setFavoriteIds(readUserFavorites(normalized));
                setHistory(readUserHistory(normalized));
                closeAuthModal();
                return true;
            }

            return false;
        },
        [closeAuthModal]
    );

    const logout = useCallback(() => {
        setUser(null);
        setFavoriteIds([]);
        setHistory([]);
    }, []);

    const updateUser = useCallback(
        (updates: UpdateUserPayload): AuthActionResult => {
            if (!user) {
                return { success: false, error: 'No active user session.' };
            }

            const { currentPassword, newPassword, ...profileUpdates } = updates;
            const oldEmail = user.email;
            const newEmail = profileUpdates.email
                ? profileUpdates.email.trim().toLowerCase()
                : oldEmail;

            const registry = readRegistry();
            const existingEntry = registry[oldEmail];

            if (newEmail !== oldEmail) {
                if (
                    (newEmail === DEMO_USER.email && oldEmail !== DEMO_USER.email) ||
                    registry[newEmail]
                ) {
                    return {
                        success: false,
                        error: 'This email address is already registered to another profile.',
                    };
                }
            }

            let updatedPasswordHash = existingEntry?.passwordHash;
            if (newPassword) {
                if (newPassword.length < 6) {
                    return {
                        success: false,
                        error: 'New password must be at least 6 characters long.',
                    };
                }
                if (
                    existingEntry &&
                    (existingEntry.passwordHash || existingEntry.password)
                ) {
                    if (
                        !currentPassword ||
                        !verifyStoredPassword(existingEntry, currentPassword, oldEmail)
                    ) {
                        return {
                            success: false,
                            error: 'Current password is incorrect.',
                        };
                    }
                }
                updatedPasswordHash = hashPassword(newPassword);
            }

            const updatedName = profileUpdates.name
                ? profileUpdates.name.trim()
                : user.name;
            const updatedUser: UserProfile = {
                ...user,
                ...profileUpdates,
                name: updatedName,
                email: newEmail,
                initials: calculateInitials(updatedName),
            };

            try {
                if (newEmail !== oldEmail) {
                    // Migrate favorites and history to new email key
                    localStorage.setItem(
                        `fe_user_favorites_${newEmail}`,
                        JSON.stringify(favoriteIds)
                    );
                    localStorage.setItem(
                        `fe_user_history_${newEmail}`,
                        JSON.stringify(history)
                    );
                    localStorage.removeItem(`fe_user_favorites_${oldEmail}`);
                    localStorage.removeItem(`fe_user_history_${oldEmail}`);

                    delete registry[oldEmail];
                    registry[newEmail] = {
                        profile: updatedUser,
                        ...(updatedPasswordHash
                            ? { passwordHash: updatedPasswordHash }
                            : {}),
                    };
                } else {
                    registry[oldEmail] = {
                        ...(existingEntry || {}),
                        profile: updatedUser,
                        ...(updatedPasswordHash
                            ? { passwordHash: updatedPasswordHash }
                            : {}),
                    };
                }
                localStorage.setItem(REGISTERED_USERS_KEY, JSON.stringify(registry));
            } catch (err) {
                console.error('Failed to persist profile update:', err);
            }

            setUser(updatedUser);
            return { success: true };
        },
        [user, favoriteIds, history]
    );

    const addFavorite = useCallback(
        (id: string) => {
            if (!user) {
                openAuthModal();
                return;
            }
            const perfume = PERFUMES_COLLECTION.find((p) => p.id === id);
            setFavoriteIds((prev) => (prev.includes(id) ? prev : [...prev, id]));
            if (perfume) {
                addHistoryEntry({
                    query: `Saved ${perfume.name} (${perfume.brand}) to Wardrobe`,
                    mode: 'catalog',
                    resultCount: 1,
                });
            }
        },
        [user, openAuthModal, addHistoryEntry]
    );

    const removeFavorite = useCallback(
        (id: string) => {
            if (!user) return;
            setFavoriteIds((prev) => prev.filter((item) => item !== id));
        },
        [user]
    );

    const toggleFavorite = useCallback(
        (id: string) => {
            if (!user) {
                openAuthModal();
                return;
            }
            const isCurrentlyFav = favoriteIds.includes(id);
            if (isCurrentlyFav) {
                removeFavorite(id);
            } else {
                addFavorite(id);
            }
        },
        [user, favoriteIds, openAuthModal, removeFavorite, addFavorite]
    );

    const isFavorite = useCallback(
        (id: string) => Boolean(user && favoriteIds.includes(id)),
        [user, favoriteIds]
    );

    const clearHistory = useCallback(() => {
        setHistory([]);
        if (user) {
            try {
                localStorage.removeItem(`fe_user_history_${user.email}`);
            } catch (err) {
                console.warn('Failed to clear history from storage:', err);
            }
        }
    }, [user]);

    const favoritePerfumes: PerfumeItem[] = useMemo(
        () =>
            favoriteIds
                .map((id) => PERFUMES_COLLECTION.find((p) => p.id === id))
                .filter((p): p is PerfumeItem => Boolean(p)),
        [favoriteIds]
    );

    const analytics: TasteAnalytics = useMemo(() => {
        if (favoritePerfumes.length === 0) {
            return {
                hasData: false,
                totalFavorites: 0,
                dominantFamily: '—',
                dominantNote: '—',
                vibePersona: 'Olfactory Novice',
                vibeDescription:
                    'Your signature scent archetype and visual taste radar will activate as you discover and save fragrances to your wardrobe.',
                families: BASE_FAMILIES_META.map((meta) => ({
                    ...meta,
                    count: 0,
                    percentage: 0,
                })),
                topNotes: [],
                radarData: [
                    { label: 'Woody & Earthy', value: 0, maxValue: 100 },
                    { label: 'Citrus & Bright', value: 0, maxValue: 100 },
                    { label: 'Fresh Green', value: 0, maxValue: 100 },
                    { label: 'Resinous Amber', value: 0, maxValue: 100 },
                    { label: 'Spicy Warmth', value: 0, maxValue: 100 },
                    { label: 'Soft Floral', value: 0, maxValue: 100 },
                ],
                exploredCount: history.length,
                comparisonsCount: 0,
            };
        }

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
        const spicyPct =
            familyCounts.Spicy > 0
                ? Math.max(1, 100 - (woodyPct + citrusPct + freshPct))
                : 0;

        const pctMap: Record<'Woody' | 'Citrus' | 'Fresh' | 'Spicy', number> = {
            Woody: woodyPct,
            Citrus: citrusPct,
            Fresh: freshPct,
            Spicy: spicyPct,
        };

        const families: FamilyStat[] = BASE_FAMILIES_META.map((meta) => ({
            ...meta,
            count: familyCounts[meta.name],
            percentage: pctMap[meta.name],
        }));

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
                name.includes('Cedar') ||
                name.includes('Vetiver') ||
                name.includes('Sandalwood') ||
                name.includes('Wood') ||
                name.includes('Oud')
                    ? 'Woody'
                    : name.includes('Bergamot') ||
                      name.includes('Orange') ||
                      name.includes('Lemon') ||
                      name.includes('Grapefruit') ||
                      name.includes('Mandarin')
                    ? 'Citrus'
                    : name.includes('Pepper') ||
                      name.includes('Saffron') ||
                      name.includes('Cardamom') ||
                      name.includes('Vanilla') ||
                      name.includes('Tonka')
                    ? 'Spicy'
                    : 'Fresh',
        }));

        const sortedFamilies = [...families].sort((a, b) => b.count - a.count);
        const dominant = sortedFamilies[0];

        const radarData: RadarDimension[] = [
            {
                label: 'Woody & Earthy',
                value: Math.min(100, Math.round(woodyPct * 0.9) + (woodyPct > 0 ? 15 : 8)),
                maxValue: 100,
            },
            {
                label: 'Citrus & Bright',
                value: Math.min(100, Math.round(citrusPct * 0.9) + (citrusPct > 0 ? 15 : 8)),
                maxValue: 100,
            },
            {
                label: 'Fresh Green',
                value: Math.min(100, Math.round(freshPct * 0.9) + (freshPct > 0 ? 15 : 8)),
                maxValue: 100,
            },
            {
                label: 'Resinous Amber',
                value: Math.min(100, Math.round((woodyPct + spicyPct) * 0.5) + 12),
                maxValue: 100,
            },
            {
                label: 'Spicy Warmth',
                value: Math.min(100, Math.round(spicyPct * 0.9) + (spicyPct > 0 ? 15 : 8)),
                maxValue: 100,
            },
            {
                label: 'Soft Floral',
                value: Math.min(100, Math.round((freshPct + citrusPct) * 0.35) + 16),
                maxValue: 100,
            },
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
            vibeDescription: `Your personal edit is led by ${dominant.name.toLowerCase()} accords (${dominant.percentage}%), featuring distinct notes of ${topNotes
                .slice(0, 3)
                .map((n) => n.name)
                .join(', ')}.`,
            families,
            topNotes,
            radarData,
            exploredCount: Math.max(history.length, totalCount * 3),
            comparisonsCount: Math.min(totalCount * 2, 8),
        };
    }, [favoritePerfumes, history.length]);

    const contextValue = useMemo<ProfileContextType>(
        () => ({
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
            addHistoryEntry,
            clearHistory,
            isAuthModalOpen,
            openAuthModal,
            closeAuthModal,
            requireAuth,
        }),
        [
            user,
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
            addHistoryEntry,
            clearHistory,
            isAuthModalOpen,
            openAuthModal,
            closeAuthModal,
            requireAuth,
        ]
    );

    return (
        <ProfileContext.Provider value={contextValue}>
            {children}
        </ProfileContext.Provider>
    );
};

// eslint-disable-next-line react-refresh/only-export-components
export const useProfile = () => {
    const context = useContext(ProfileContext);
    if (!context) {
        throw new Error('useProfile must be used within a ProfileProvider');
    }
    return context;
};
