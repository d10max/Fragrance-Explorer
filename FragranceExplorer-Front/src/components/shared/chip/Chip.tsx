import styles from './Chip.module.scss';

export type NoteChipVariant =
    | 'green'
    | 'gold'
    | 'default'
    | 'woody'
    | 'fresh'
    | 'spicy'
    | 'citrus';

interface NoteChipProps {
    label: string;
    variant?: NoteChipVariant;
    className?: string;
}

export const Chip = ({
    label,
    variant = 'default',
    className = '',
}: NoteChipProps) => {
    return (
        <span className={`${styles.chip} ${styles[variant]} ${className}`}>
            {label}
        </span>
    );
};
