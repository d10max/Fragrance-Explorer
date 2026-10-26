import styles from '@/components/shared/noteChip/NoteChip.module.scss';

export type NoteChipVariant =
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

export const NoteChip = ({
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
