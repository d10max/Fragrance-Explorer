import styles from '@/components/shared/button/Button.module.scss';

export type ButtonProps = {
    children: React.ReactNode;
    buttonStyle?: 'primary' | 'secondary';
    onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void;
    className?: string;
    disabled?: boolean;
    isIcon?: boolean;
};

export const Button = ({
    children,
    buttonStyle,
    onClick,
    className = '',
    disabled = false,
    isIcon = false,
}: ButtonProps) => {
    const buttonClassName = [
        styles.button,
        buttonStyle ? styles[buttonStyle] : '',
        isIcon ? styles.iconOnly : '',
        className,
    ]
        .filter(Boolean)
        .join(' ');

    return (
        <button
            className={buttonClassName}
            onClick={onClick}
            disabled={disabled}
        >
            {children}
        </button>
    );
};
