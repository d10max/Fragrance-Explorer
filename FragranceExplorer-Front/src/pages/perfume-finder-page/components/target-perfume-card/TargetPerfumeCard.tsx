import type { Perfume } from '@/types/common/perfume';
import { Button } from '@/components/shared/button/Button';
import { CloseIcon } from '@/assets/icons/CloseIcon';
import { TARGET_PERFUME_CARD_TEXT } from '@/const/common';
import styles from './TargetPerfumeCard.module.scss';

interface TargetPerfumeCardProps {
    selectedPerfume: Perfume | null;
    onRemove: () => void;
}

export const TargetPerfumeCard: React.FC<TargetPerfumeCardProps> = ({
    selectedPerfume,
    onRemove,
}) => {
    const isSelected = Boolean(selectedPerfume);

    return (
        <div className={styles.wrapper}>
            <div className={styles.header}>
                <div className={styles.titleBox}>
                    <h2 className={styles.title}>
                        {TARGET_PERFUME_CARD_TEXT.TITLE}
                    </h2>
                    <span className={styles.counter}>
                        {isSelected
                            ? TARGET_PERFUME_CARD_TEXT.SELECTED_COUNT(1)
                            : TARGET_PERFUME_CARD_TEXT.SELECTED_COUNT(0)}
                    </span>
                </div>
            </div>

            <div className={styles.cardContainer}>
                {selectedPerfume ? (
                    <div className={styles.card}>
                        <Button
                            isIcon={true}
                            className={styles.closeButton}
                            onClick={onRemove}
                        >
                            <CloseIcon />
                        </Button>

                        <img
                            src={selectedPerfume.imageUrl ?? ''}
                            alt={selectedPerfume.name}
                            className={styles.image}
                        />
                        <span className={styles.brand}>
                            {selectedPerfume.brand}
                        </span>
                        <h3 className={styles.name}>{selectedPerfume.name}</h3>
                    </div>
                ) : (
                    <div className={styles.placeholder}>
                        <p>{TARGET_PERFUME_CARD_TEXT.PLACEHOLDER_TEXT}</p>
                    </div>
                )}
            </div>
        </div>
    );
};
