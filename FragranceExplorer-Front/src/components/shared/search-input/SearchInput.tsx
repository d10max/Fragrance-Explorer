import { CloseIcon } from '@/assets/icons/CloseIcon';
import { SearchIcon } from '@/assets/icons/SearchIcon';
import { Button } from '@/components/shared/button/Button';
import styles from '@/components/shared/searchInput/SearchInput.module.scss';

export type SearchInputProps = {
    value: string;
    onChange: (value: string) => void;
    onSearch?: (value: string) => void;
    onClear?: () => void;
    placeholder?: string;
    className?: string;
    showSearchButton?: boolean;
    showClearButton?: boolean;
};

export const SearchInput = ({
    value,
    onChange,
    onSearch,
    onClear,
    placeholder = 'Search...',
    className = '',
    showSearchButton = true,
    showClearButton = true,
}: SearchInputProps) => {
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        onChange(e.target.value);
    };

    const handleClear = () => {
        onChange('');
        onClear?.();
    };

    const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === 'Enter') {
            onSearch?.(value);
        }
    };

    return (
        <div className={`${styles.wrapper} ${className}`}>
            {showSearchButton && (
                <Button isIcon={true} onClick={() => onSearch?.(value)}>
                    <SearchIcon />
                </Button>
            )}
            <input
                className={styles.input}
                type="text"
                value={value}
                onChange={handleChange}
                onKeyDown={handleKeyDown}
                placeholder={placeholder}
            />
            {showClearButton && (
                <Button
                    isIcon={true}
                    onClick={handleClear}
                    disabled={value.length === 0}
                >
                    <CloseIcon />
                </Button>
            )}
        </div>
    );
};
