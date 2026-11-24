import { Button } from '@/components/shared/button/Button';
import { COMMON_TEXT } from '@/const/common';
import type { PerfumeSearchItem } from '@/types/common/perfume';
import styles from './SearchDropdownItem.module.scss';

export interface SearchDropdownItemProps {
    key?: string | number;
    perfume: PerfumeSearchItem;
    onAdd: (perfume: PerfumeSearchItem) => void;
}

export const SearchDropdownItem = ({
    key,
    perfume,
    onAdd,
}: SearchDropdownItemProps) => {
    return (
        <div className={styles.item} key={key}>
            <div className={styles.itemInfo}>
                <img
                    src={perfume.imageUrl || ''}
                    alt={perfume.name}
                    className={styles.image}
                />
                <div className={styles.textInfo}>
                    <span className={styles.name}>{perfume.name}</span>
                    <span className={styles.brand}>{perfume.brandName}</span>
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
