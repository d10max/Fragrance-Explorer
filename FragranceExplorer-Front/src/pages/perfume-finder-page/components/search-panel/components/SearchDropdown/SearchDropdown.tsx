import type { Perfume } from '@/types/common/perfume';
import { SearchDropdownItem } from '../SearchDropdownItem/SearchDropdownItem';
import { SEARCH_DROPDOWN } from '@/const/common';
import styles from './SearchDropdown.module.scss';

export interface SearchDropdownProps {
    perfumes: Perfume[];
    onAddPerfume: (perfume: Perfume) => void;
    isOpen: boolean;
}

export const SearchDropdown = ({
    perfumes,
    onAddPerfume,
    isOpen,
}: SearchDropdownProps) => {
    const resultsCountText = `${perfumes.length} ${perfumes.length === 1 ? 'result' : 'results'}`;

    return (
        isOpen && (
            <div className={styles.dropdown}>
                <div className={styles.header}>
                    <span className={styles.title}>
                        {SEARCH_DROPDOWN.FRAGRANCES}
                    </span>
                    <span className={styles.resultsCount}>
                        {resultsCountText}
                    </span>
                </div>

                <div className={styles.list}>
                    {perfumes.length > 0 ? (
                        perfumes.map((perfume) => (
                            <SearchDropdownItem
                                perfume={perfume}
                                onAdd={onAddPerfume}
                            />
                        ))
                    ) : (
                        <div className={styles.emptyState}>
                            {SEARCH_DROPDOWN.NO_RESULTS}
                        </div>
                    )}
                </div>
            </div>
        )
    );
};
