import { useLocation, useNavigate } from 'react-router-dom';
import { useCompare } from '@/context/CompareContext';
import { getPerfumeById } from '@/pages/perfume-catalog-page/perfumes.data';
import { PerfumeBottle } from '@/pages/perfume-catalog-page/components/PerfumeBottle';
import { ROUTES } from '@/const/routes';
import styles from './CompareTray.module.scss';

export const CompareTray = () => {
    const { compareIds, removeFromCompare, clearCompare } = useCompare();
    const location = useLocation();
    const navigate = useNavigate();

    // Hide floating tray when empty or already on the comparison page
    if (compareIds.length === 0 || location.pathname.startsWith(ROUTES.PERFUME_COMPARE.FULL)) {
        return null;
    }

    const firstPerfume = getPerfumeById(compareIds[0]);
    const secondPerfume = compareIds[1] ? getPerfumeById(compareIds[1]) : null;

    return (
        <aside className={styles.tray} aria-label="Fragrance comparison tray">
            <div className={styles.trayHeader}>
                <span className={styles.trayEyebrow}>
                    SCENT COMPARISON ({compareIds.length} / 2)
                </span>
                <button
                    type="button"
                    className={styles.clearBtn}
                    onClick={clearCompare}
                    aria-label="Clear comparison"
                >
                    ✕
                </button>
            </div>

            <div className={styles.slotsList}>
                {/* Slot 1 */}
                <div className={styles.slotFilled}>
                    <div className={styles.miniBottle}>
                        <PerfumeBottle
                            capColor={firstPerfume.capColor}
                            liquidColor={firstPerfume.liquidColor}
                            liquidHeight={firstPerfume.liquidHeight}
                            width={28}
                            height={42}
                        />
                    </div>
                    <div className={styles.slotInfo}>
                        <span className={styles.slotBrand}>{firstPerfume.brand}</span>
                        <span className={styles.slotName}>{firstPerfume.name}</span>
                    </div>
                    <button
                        type="button"
                        className={styles.removeSlotBtn}
                        onClick={() => removeFromCompare(firstPerfume.id)}
                        title="Remove fragrance"
                    >
                        ✕
                    </button>
                </div>

                {/* Slot 2 */}
                {secondPerfume ? (
                    <div className={styles.slotFilled}>
                        <div className={styles.miniBottle}>
                            <PerfumeBottle
                                capColor={secondPerfume.capColor}
                                liquidColor={secondPerfume.liquidColor}
                                liquidHeight={secondPerfume.liquidHeight}
                                width={28}
                                height={42}
                            />
                        </div>
                        <div className={styles.slotInfo}>
                            <span className={styles.slotBrand}>{secondPerfume.brand}</span>
                            <span className={styles.slotName}>{secondPerfume.name}</span>
                        </div>
                        <button
                            type="button"
                            className={styles.removeSlotBtn}
                            onClick={() => removeFromCompare(secondPerfume.id)}
                            title="Remove fragrance"
                        >
                            ✕
                        </button>
                    </div>
                ) : (
                    <div className={styles.slotEmpty}>
                        <span className={styles.slotEmptyIcon}>+</span>
                        <div className={styles.slotEmptyTextGroup}>
                            <span className={styles.slotEmptyTitle}>
                                Waiting for 2nd fragrance
                            </span>
                            <span className={styles.slotEmptyHint}>
                                Open another scent &amp; click &ldquo;Compare&rdquo;
                            </span>
                        </div>
                    </div>
                )}
            </div>

            {secondPerfume ? (
                <button
                    type="button"
                    className={styles.compareNowBtn}
                    onClick={() =>
                        navigate(
                            `${ROUTES.PERFUME_COMPARE.FULL}?scentA=${firstPerfume.id}&scentB=${secondPerfume.id}`
                        )
                    }
                >
                    Compare Both Scents &rarr;
                </button>
            ) : (
                location.pathname !== ROUTES.PERFUME_CATALOG.FULL && (
                    <button
                        type="button"
                        className={styles.browseCatalogBtn}
                        onClick={() => navigate(ROUTES.PERFUME_CATALOG.FULL)}
                    >
                        Browse Catalog for 2nd Scent &rarr;
                    </button>
                )
            )}
        </aside>
    );
};

