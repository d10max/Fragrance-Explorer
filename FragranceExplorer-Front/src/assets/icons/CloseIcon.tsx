import closeIcon from '@/assets/icons/closeIcon.png';

export const CloseIcon = ({ className = '' }: { className?: string }) => {
    return (
        <img
            src={closeIcon}
            alt="Close"
            width="8"
            height="8"
            className={className}
        />
    );
};
