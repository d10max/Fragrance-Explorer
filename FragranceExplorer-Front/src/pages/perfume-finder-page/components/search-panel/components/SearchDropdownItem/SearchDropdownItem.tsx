import { Button } from '@/components/shared/button/Button';
import { COMMON_TEXT } from '@/const/common';
import type { Perfume } from '@/types/common/perfume';
import styles from './SearchDropdownItem.module.scss';

export interface SearchDropdownItemProps {
    perfume: Perfume;
    onAdd: (perfume: Perfume) => void;
}

export const SearchDropdownItem = ({
    perfume,
    onAdd,
}: SearchDropdownItemProps) => {
    return (
        <div className={styles.item}>
            <div className={styles.itemInfo}>
                <img
                    src={perfume.imageUrl || ''}
                    alt={perfume.name}
                    className={styles.image}
                />
                <div className={styles.textInfo}>
                    <span className={styles.name}>{perfume.name}</span>
                    <span className={styles.brand}>{perfume.brand}</span>
                </div>
            </div>

            <Button
                buttonStyle="secondary"
                onClick={() => onAdd(perfume)}
                className={styles.addButton}
            >
                {COMMON_TEXT.PLACEHOLDERS.BUTTON_ADD_PERFUME}
            </Button>
        </div>
    );
};
