import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    BarChart3,
    Bookmark,
    Clock,
    PenLine,
    LogOut,
    Sparkles,
    Compass,
} from 'lucide-react';
import { useProfile } from '@/context/ProfileContext';
import { getPerfumeById } from '@/pages/perfume-catalog-page/perfumes.data';
import { ScentTasteChart } from './components/ScentTasteChart';
import { FavoritePerfumes } from './components/FavoritePerfumes';
import { EditProfileModal } from './components/EditProfileModal';
import { ROUTES } from '@/const/routes';
import styles from './ProfilePage.module.scss';

export const ProfilePage = () => {
    const navigate = useNavigate();
    const {
        user,
        isAuthenticated,
        logout,
        openAuthModal,
        loadDemoProfile,
        favoritePerfumes,
        history,
        clearHistory,
        analytics,
    } = useProfile();

    const [activeTab, setActiveTab] = useState<'analytics' | 'favorites' | 'history'>(
        'analytics'
    );
    const [isEditModalOpen, setIsEditModalOpen] = useState(false);

    // If guest (not logged in)
    if (!isAuthenticated || !user) {
        return (
            <div className={styles.container}>
                {/* Guest Hero Banner */}
                <section className={styles.guestHero}>
                    <span className={styles.eyebrow}>
                        PRIVATE SCENT CABINET &middot; GUEST SESSION
                    </span>
                    <h1 className={styles.guestTitle}>Unlock Your Olfactory Identity</h1>
                    <p className={styles.guestSubtitle}>
                        You are currently browsing as a guest. Create your private connoisseur
                        profile to build your personal perfume wardrobe, generate interactive
                        scent radar diagrams, and track your olfactory journey.
                    </p>

                    <div className={styles.guestActions}>
                        <button
                            type="button"
                            className={styles.guestPrimaryBtn}
                            onClick={openAuthModal}
                        >
                            Create Free Connoisseur Profile &rarr;
                        </button>
                        <button
                            type="button"
                            className={styles.guestSecondaryBtn}
                            onClick={loadDemoProfile}
                        >
                            Explore as Demo (Elena Vance)
                        </button>
                    </div>
                </section>

                {/* Guest Feature Highlights */}
                <div className={styles.guestFeaturesGrid}>
                    <div className={styles.guestFeatureCard}>
                        <div className={styles.featureIcon}>
                            <Bookmark size={18} />
                        </div>
                        <h3 className={styles.featureTitle}>Curated Wardrobe</h3>
                        <p className={styles.featureDesc}>
                            Save your beloved niche flacons, catalog your collection, and
                            organize by accords.
                        </p>
                    </div>

                    <div className={styles.guestFeatureCard}>
                        <div className={styles.featureIcon}>
                            <BarChart3 size={18} />
                        </div>
                        <h3 className={styles.featureTitle}>Visual Taste Radar</h3>
                        <p className={styles.featureDesc}>
                            Interactive family ratios and 6-axis radar diagrams calculated from
                            your preferences.
                        </p>
                    </div>

                    <div className={styles.guestFeatureCard}>
                        <div className={styles.featureIcon}>
                            <Compass size={18} />
                        </div>
                        <h3 className={styles.featureTitle}>Note Affinity Index</h3>
                        <p className={styles.featureDesc}>
                            Discover your highest-affinity notes — from dry cedar and vetiver to
                            sparkling citrus.
                        </p>
                    </div>
                </div>

                {/* Teaser Preview with live demo backdrop */}
                <div className={styles.guestTeaserWrapper}>
                    <div className={styles.teaserOverlay}>
                        <span className={styles.teaserBadge}>CONNOISSEUR PREVIEW</span>
                        <h3 className={styles.teaserTitle}>Your Personal Analytics Await</h3>
                        <p className={styles.teaserText}>
                            Register your account in seconds or click &ldquo;Explore as
                            Demo&rdquo; to test with full pre-loaded data.
                        </p>
                        <button
                            type="button"
                            className={styles.teaserBtn}
                            onClick={openAuthModal}
                        >
                            Sign In / Register &rarr;
                        </button>
                    </div>
                    <div className={styles.teaserBlur}>
                        <ScentTasteChart />
                    </div>
                </div>
            </div>
        );
    }

    const signaturePerfume = user.signatureScentId
        ? getPerfumeById(user.signatureScentId)
        : null;

    return (
        <div className={styles.container}>
            {/* Top User Profile Header Banner */}
            <section className={styles.profileHeader}>
                <div className={styles.headerMain}>
                    <div className={styles.avatarWrapper}>
                        <div className={styles.avatarCircle}>
                            <span>{user.initials || 'EV'}</span>
                        </div>
                        <span
                            className={styles.memberStatusDot}
                            title="Connoisseur Active"
                        />
                    </div>

                    <div className={styles.profileInfo}>
                        <div className={styles.nameRow}>
                            <h1 className={styles.userName}>{user.name}</h1>
                            <span className={styles.levelBadge}>{user.level}</span>
                        </div>
                        <p className={styles.userEmail}>
                            {user.email} &middot; Member since {user.memberSince}
                        </p>
                        <p className={styles.userBio}>{user.bio}</p>
                    </div>
                </div>

                <div className={styles.headerActions}>
                    <button
                        type="button"
                        className={styles.editBtn}
                        onClick={() => setIsEditModalOpen(true)}
                    >
                        <PenLine size={14} />
                        Edit Profile
                    </button>

                    <button
                        type="button"
                        className={styles.logoutBtn}
                        onClick={logout}
                        title="Sign Out to Guest Mode"
                    >
                        <LogOut
                            size={14}
                            style={{ marginRight: 5, verticalAlign: 'text-bottom' }}
                        />
                        Sign Out
                    </button>
                </div>
            </section>

            {/* 4 Summary Stat Cards */}
            <section className={styles.statsRow}>
                <div
                    className={styles.statCard}
                    role="button"
                    tabIndex={0}
                    onClick={() => setActiveTab('favorites')}
                    onKeyDown={(e) => {
                        if (e.key === 'Enter' || e.key === ' ') {
                            e.preventDefault();
                            setActiveTab('favorites');
                        }
                    }}
                >
                    <span className={styles.statEyebrow}>SAVED WARDROBE</span>
                    <div className={styles.statValue}>
                        {favoritePerfumes.length}{' '}
                        <span className={styles.statUnit}>scents</span>
                    </div>
                    <span className={styles.statSub}>Curated personal shelf &rarr;</span>
                </div>

                <div
                    className={styles.statCard}
                    role="button"
                    tabIndex={0}
                    onClick={() => setActiveTab('analytics')}
                    onKeyDown={(e) => {
                        if (e.key === 'Enter' || e.key === ' ') {
                            e.preventDefault();
                            setActiveTab('analytics');
                        }
                    }}
                >
                    <span className={styles.statEyebrow}>DOMINANT PROFILE</span>
                    <div className={styles.statValue}>
                        {analytics.hasData ? (
                            <>
                                {analytics.dominantFamily}{' '}
                                <span className={styles.statUnit}>
                                    {analytics.families.find(
                                        (f) => f.name === analytics.dominantFamily
                                    )?.percentage || 0}
                                    %
                                </span>
                            </>
                        ) : (
                            <span style={{ color: '#77736c' }}>&mdash;</span>
                        )}
                    </div>
                    <span className={styles.statSub}>
                        {analytics.hasData
                            ? 'Top olfactory preference \u2192'
                            : 'Save perfumes to unlock \u2192'}
                    </span>
                </div>

                <div
                    className={styles.statCard}
                    role="button"
                    tabIndex={0}
                    onClick={() => setActiveTab('analytics')}
                    onKeyDown={(e) => {
                        if (e.key === 'Enter' || e.key === ' ') {
                            e.preventDefault();
                            setActiveTab('analytics');
                        }
                    }}
                >
                    <span className={styles.statEyebrow}>TOP KEY NOTE</span>
                    <div className={styles.statValue}>
                        {analytics.hasData ? (
                            analytics.topNotes[0]?.name || '—'
                        ) : (
                            <span style={{ color: '#77736c' }}>&mdash;</span>
                        )}
                    </div>
                    <span className={styles.statSub}>
                        {analytics.hasData
                            ? `${analytics.topNotes[0]?.count || 1} selections in wardrobe \u2192`
                            : 'Awaiting your 1st scent \u2192'}
                    </span>
                </div>

                <div
                    className={styles.statCard}
                    role="button"
                    tabIndex={0}
                    onClick={() => {
                        if (signaturePerfume) {
                            navigate(ROUTES.PERFUME_DETAIL.FULL(signaturePerfume.id));
                        } else {
                            setIsEditModalOpen(true);
                        }
                    }}
                    onKeyDown={(e) => {
                        if (e.key === 'Enter' || e.key === ' ') {
                            e.preventDefault();
                            if (signaturePerfume) {
                                navigate(
                                    ROUTES.PERFUME_DETAIL.FULL(signaturePerfume.id)
                                );
                            } else {
                                setIsEditModalOpen(true);
                            }
                        }
                    }}
                >
                    <span className={styles.statEyebrow}>SIGNATURE FLACON</span>
                    <div className={styles.statValueSmall}>
                        {signaturePerfume ? (
                            signaturePerfume.name
                        ) : (
                            <span style={{ color: '#77736c', fontSize: '18px' }}>
                                None chosen
                            </span>
                        )}
                    </div>
                    <span className={styles.statSub}>
                        {signaturePerfume
                            ? `${signaturePerfume.brand} \u00B7 View perfume \u2192`
                            : 'Select in Edit Profile \u2192'}
                    </span>
                </div>
            </section>

            {/* Profile Navigation Tabs */}
            <div className={styles.tabNavigation}>
                <button
                    type="button"
                    className={`${styles.navTab} ${
                        activeTab === 'analytics' ? styles.navTabActive : ''
                    }`}
                    onClick={() => setActiveTab('analytics')}
                >
                    <BarChart3 size={15} className={styles.tabSvg} />
                    Taste Analytics &amp; Diagrams
                </button>

                <button
                    type="button"
                    className={`${styles.navTab} ${
                        activeTab === 'favorites' ? styles.navTabActive : ''
                    }`}
                    onClick={() => setActiveTab('favorites')}
                >
                    <Bookmark size={15} className={styles.tabSvg} />
                    Saved Fragrances ({favoritePerfumes.length})
                </button>

                <button
                    type="button"
                    className={`${styles.navTab} ${
                        activeTab === 'history' ? styles.navTabActive : ''
                    }`}
                    onClick={() => setActiveTab('history')}
                >
                    <Clock size={15} className={styles.tabSvg} />
                    Discovery &amp; Search Log ({history.length})
                </button>
            </div>

            {/* Tab Contents */}
            <div className={styles.tabBody}>
                {activeTab === 'analytics' &&
                    (analytics.hasData ? (
                        <ScentTasteChart />
                    ) : (
                        <div className={styles.emptyAnalytics}>
                            <div className={styles.emptyAnalyticsIcon}>
                                <Sparkles size={24} />
                            </div>
                            <span className={styles.eyebrow}>
                                CALIBRATING YOUR TASTE PROFILE
                            </span>
                            <h3 className={styles.emptyAnalyticsTitle}>
                                Your Olfactory Scent Wheel Awaits Your First Fragrance
                            </h3>
                            <p className={styles.emptyAnalyticsDesc}>
                                You haven&apos;t saved any perfumes to your wardrobe yet.
                                Explore the Encyclopedia, open any fragrance dossier, and
                                click &ldquo;Save to My Wardrobe&rdquo; (or quick-add from
                                the Saved Fragrances tab). Your live Scent Wheel, 6-axis
                                Radar Spectrum, and favorite notes will be calculated right
                                here in real time.
                            </p>
                            <div className={styles.emptyAnalyticsActions}>
                                <button
                                    type="button"
                                    className={styles.exploreBtn}
                                    onClick={() => navigate(ROUTES.PERFUME_CATALOG.FULL)}
                                >
                                    Explore Perfume Encyclopedia &rarr;
                                </button>
                                <button
                                    type="button"
                                    className={styles.demoSampleBtn}
                                    onClick={loadDemoProfile}
                                >
                                    Or Load Sample Data (Demo Mode)
                                </button>
                            </div>
                        </div>
                    ))}

                {activeTab === 'favorites' && <FavoritePerfumes />}

                {activeTab === 'history' && (
                    <section className={styles.historySection}>
                        <div className={styles.historyHeader}>
                            <div>
                                <span className={styles.eyebrow}>SEARCH TIMELINE</span>
                                <h3 className={styles.historyTitle}>
                                    Recent Fragrance Explorations
                                </h3>
                            </div>
                            {history.length > 0 && (
                                <button
                                    type="button"
                                    className={styles.clearHistoryBtn}
                                    onClick={clearHistory}
                                >
                                    Clear Log
                                </button>
                            )}
                        </div>

                        {history.length === 0 ? (
                            <div className={styles.emptyHistory}>
                                <p>No exploration history recorded yet.</p>
                                <button
                                    type="button"
                                    className={styles.exploreBtn}
                                    onClick={() => navigate(ROUTES.PERFUME_FINDER.FULL)}
                                >
                                    Launch Perfume Finder &rarr;
                                </button>
                            </div>
                        ) : (
                            <div className={styles.historyList}>
                                {history.map((item) => (
                                    <div key={item.id} className={styles.historyItem}>
                                        <div className={styles.historyLeft}>
                                            <span className={styles.historyMode}>
                                                {item.mode.toUpperCase()}
                                            </span>
                                            <div>
                                                <h4 className={styles.historyQuery}>
                                                    {item.query}
                                                </h4>
                                                <span className={styles.historyMeta}>
                                                    {item.date} &middot;{' '}
                                                    {item.resultCount} results
                                                </span>
                                            </div>
                                        </div>

                                        <button
                                            type="button"
                                            className={styles.relaunchBtn}
                                            onClick={() =>
                                                navigate(ROUTES.PERFUME_FINDER.FULL)
                                            }
                                        >
                                            Explore Similar &rarr;
                                        </button>
                                    </div>
                                ))}
                            </div>
                        )}
                    </section>
                )}
            </div>

            {/* Recommendation Footnote banner */}
            <section className={styles.curatedRecommendation}>
                <div className={styles.curatedContent}>
                    <span className={styles.eyebrow}>CURATED EDIT FOR YOU</span>
                    <h3 className={styles.curatedTitle}>
                        Have you discovered <em>Vetiver Extraordinaire</em>?
                    </h3>
                    <p className={styles.curatedDesc}>
                        With your affinity for earthy vetiver and sparkling bergamot,
                        Frédéric Malle’s masterwork aligns with your signature olfactory
                        harmony.
                    </p>
                </div>
                <button
                    type="button"
                    className={styles.curatedBtn}
                    onClick={() =>
                        navigate(ROUTES.PERFUME_DETAIL.FULL('vetiver-extraordinaire'))
                    }
                >
                    Explore Recommendation &rarr;
                </button>
            </section>

            {/* Edit Profile Modal */}
            <EditProfileModal
                isOpen={isEditModalOpen}
                onClose={() => setIsEditModalOpen(false)}
            />
        </div>
    );
};
