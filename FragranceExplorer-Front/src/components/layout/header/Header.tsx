import { ROUTES } from '@/const/routes';
import { Link, NavLink } from 'react-router-dom';
import { COMMON_TEXT } from '@/const/common.ts';
import styles from '@/components/layout/header/Header.module.scss';

export const Header = () => {
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
            </nav>
        </header>
    );
};
