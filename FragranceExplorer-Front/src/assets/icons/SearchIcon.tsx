import searchIcon from '@/assets/icons/searchIcon.png';

export const SearchIcon = ({ className = '' }: { className?: string }) => {
    return (
        <img
            src={searchIcon}
            alt="Search"
            width="16"
            height="16"
            className={className}
        />
    );
};
