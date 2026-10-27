import { Link } from 'react-router-dom';
import { ROUTES } from '@/const/routes';
import { COMMON_TEXT } from '@/const/common';
import styles from './Footer.module.scss';

export const Footer = () => {
    return (
        <footer className={styles.footer}>
            <div className={styles.brandGroup}>
                <Link to={ROUTES.ROOT} className={styles.logo}>
                    {COMMON_TEXT.APP_NAME}
                </Link>
                <span className={styles.tagline}>
                    The art of finding what feels like you.
                </span>
            </div>

            <div className={styles.copyright}>
                &copy; 2026 Fragrance-Explorer
            </div>

            <div className={styles.navLinks}>
                <a href="#our-approach" className={styles.link}>
                    Our approach
                </a>
                <a href="#privacy" className={styles.link}>
                    Privacy
                </a>
                <a href="#terms" className={styles.link}>
                    Terms
                </a>
            </div>
        </footer>
    );
};
