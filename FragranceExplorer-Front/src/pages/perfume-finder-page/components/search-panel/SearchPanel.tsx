import { SearchInput } from '@/components/shared/search-input/SearchInput';
import type { Perfume } from '@/types/common/perfume';
import { searchPerfumes } from '@/utils/searchPerfumes';
import { COMMON_TEXT } from '@/const/common';
import { useMemo, useState } from 'react';
import { SearchDropdown } from './components/SearchDropdown/SearchDropdown';
import styles from './SearchPanel.module.scss';

export interface SearchPanelProps {
    onSelectPerfume: (perfume: Perfume) => void;
    perfumes: Perfume[];
    isDropdownOpen?: boolean;
}

export const SearchPanel = ({
    onSelectPerfume,
    perfumes,
    isDropdownOpen = true,
}: SearchPanelProps) => {
    const [searchQuery, setSearchQuery] = useState('');

    const handleInputChange = (value: string) => {
        setSearchQuery(value);
    };

    const handleClear = () => {
        setSearchQuery('');
    };

    const filteredPerfumes = useMemo(() => {
        return searchPerfumes(perfumes, searchQuery);
    }, [perfumes, searchQuery]);

    return (
        <div className={styles.container}>
            <SearchInput
                value={searchQuery}
                onChange={handleInputChange}
                onClear={handleClear}
                placeholder={COMMON_TEXT.PLACEHOLDERS.SEARCH_INPUT}
                showSearchButton={false}
            />
            <SearchDropdown
                perfumes={filteredPerfumes}
                onAddPerfume={(perfume) => {
                    onSelectPerfume(perfume);
                    handleClear();
                }}
                isOpen={isDropdownOpen}
            />
        </div>
    );
};
