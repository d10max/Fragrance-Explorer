import { ROUTES } from '@/const/routes';
import { Link, NavLink } from 'react-router-dom';
import { COMMON_TEXT } from '@/const/common.ts';
import { useProfile } from '@/context/ProfileContext';
import styles from '@/components/layout/header/Header.module.scss';

export const Header = () => {
    const { user, isAuthenticated, openAuthModal } = useProfile();

    return (
        <header className={styles.header}>
            <Link to={ROUTES.ROOT} className={styles.logo}>
                {COMMON_TEXT.APP_NAME}
            </Link>

            <nav className={styles.nav}>
                <NavLink
                    to={ROUTES.PERFUME_FINDER.FULL}
                    className={({ isActive }) =>
                        isActive
                            ? `${styles.link} ${styles.active}`
                            : styles.link
                    }
                >
                    {COMMON_TEXT.TABS.PERFUME_FINDER}
                </NavLink>
                <NavLink
                    to={ROUTES.PERFUME_CATALOG.FULL}
                    className={({ isActive }) =>
                        isActive
                            ? `${styles.link} ${styles.active}`
                            : styles.link
                    }
                >
                    {COMMON_TEXT.TABS.PERFUME_CATALOG}
                </NavLink>
                <NavLink
                    to={ROUTES.PROFILE.FULL}
                    className={({ isActive }) =>
                        isActive
                            ? `${styles.link} ${styles.active}`
                            : styles.link
                    }
                >
                    {COMMON_TEXT.TABS.PROFILE}
                </NavLink>
            </nav>

            <div className={styles.actions}>
                {isAuthenticated && user ? (
                    <NavLink
                        to={ROUTES.PROFILE.FULL}
                        className={({ isActive }) =>
                            isActive
                                ? `${styles.profileButton} ${styles.profileButtonActive}`
                                : styles.profileButton
                        }
                        title="Open Fragrance Profile"
                    >
                        <div className={styles.avatarPill}>
                            <div className={styles.avatarBubble}>
                                <span>{user.initials}</span>
                            </div>
                            <div className={styles.profileTextGroup}>
                                <span className={styles.profileUserName}>
                                    {user.name}
                                </span>
                                <span className={styles.profileRoleLabel}>
                                    {user.level}
                                </span>
                            </div>
                        </div>
                    </NavLink>
                ) : (
                    <button
                        type="button"
                        className={styles.guestJoinButton}
                        onClick={openAuthModal}
                        title="Sign In or Create Free Connoisseur Profile"
                    >
                        <svg
                            width="14"
                            height="14"
                            viewBox="0 0 24 24"
                            fill="none"
                            stroke="currentColor"
                            strokeWidth="2"
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            className={styles.guestIcon}
                        >
                            <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
                            <circle cx="12" cy="7" r="4" />
                        </svg>
                        <span>Sign In / Join</span>
                    </button>
                )}
            </div>
        </header>
    );
};
