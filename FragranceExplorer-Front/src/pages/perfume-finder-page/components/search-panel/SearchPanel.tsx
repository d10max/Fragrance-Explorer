import { SearchInput } from '@/components/shared/search-input/SearchInput';
import type { PerfumeSearchItem } from '@/types/common/perfume';
import { COMMON_TEXT } from '@/const/common';
import { useState } from 'react';
import { SearchDropdown } from './components/SearchDropdown/SearchDropdown';
import styles from './SearchPanel.module.scss';
import { usePerfumeSearch } from '@/hooks/usePerfumeSearch';

export interface SearchPanelProps {
    onSelectPerfume: (perfume: PerfumeSearchItem) => void;
    isDropdownOpen?: boolean;
}

export const SearchPanel = ({
    onSelectPerfume,
    isDropdownOpen = true,
}: SearchPanelProps) => {
    const [searchQuery, setSearchQuery] = useState('');
    const { results } = usePerfumeSearch(searchQuery);

    const handleInputChange = (value: string) => {
        setSearchQuery(value);
    };

    const handleClear = () => {
        setSearchQuery('');
    };

    const handleSelect = (perfume: PerfumeSearchItem) => {
        onSelectPerfume(perfume);
        handleClear();
    };

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
                perfumes={results}
                onAddPerfume={handleSelect}
                isOpen={isDropdownOpen}
            />
        </div>
    );
};
