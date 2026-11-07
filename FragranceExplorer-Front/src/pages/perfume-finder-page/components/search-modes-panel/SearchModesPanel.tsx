import { Button } from '@/components/shared/button/Button';
import { Chip, type NoteChipVariant } from '@/components/shared/chip/Chip';
import { SEARCH_MODES_PANEL_TEXT } from '@/const/common';
import type { SearchMode } from '@/types/common/perfume';
import styles from './SearchModesPanel.module.scss';

export interface SearchModesPanelProps {
    selectedMode: SearchMode;
    onSelectMode: (mode: SearchMode) => void;
    onFindPerfumes: () => void;
    isDisabled?: boolean;
}

export interface ModeOption {
    mode: SearchMode;
    badgeText: string;
    chipVariant?: NoteChipVariant;
    title: string;
    description: string;
}

const MODES: ModeOption[] = [
    {
        mode: 'balanced',
        badgeText: SEARCH_MODES_PANEL_TEXT.MODES.BALANCED.badgeText,
        chipVariant: 'default',
        title: SEARCH_MODES_PANEL_TEXT.MODES.BALANCED.title,
        description: SEARCH_MODES_PANEL_TEXT.MODES.BALANCED.description,
    },
    {
        mode: 'clones',
        badgeText: SEARCH_MODES_PANEL_TEXT.MODES.CLONES.badgeText,
        chipVariant: 'fresh',
        title: SEARCH_MODES_PANEL_TEXT.MODES.CLONES.title,
        description: SEARCH_MODES_PANEL_TEXT.MODES.CLONES.description,
    },
    {
        mode: 'similar',
        badgeText: SEARCH_MODES_PANEL_TEXT.MODES.SIMILAR.badgeText,
        chipVariant: 'woody',
        title: SEARCH_MODES_PANEL_TEXT.MODES.SIMILAR.title,
        description: SEARCH_MODES_PANEL_TEXT.MODES.SIMILAR.description,
    },
];

export const SearchModesPanel = ({
    selectedMode = 'balanced',
    onSelectMode,
    onFindPerfumes,
    isDisabled = false,
}: SearchModesPanelProps) => {
    const handleModeChange = (mode: SearchMode) => {
        onSelectMode(mode);
    };

    return (
        <div className={styles.wrapper}>
            <div className={styles.header}>
                <h2 className={styles.title}>
                    {SEARCH_MODES_PANEL_TEXT.TITLE}
                </h2>
                <p className={styles.hint}>{SEARCH_MODES_PANEL_TEXT.HINT}</p>
            </div>

            <div className={styles.modesGrid}>
                {MODES.map((modeOption) => {
                    const isActive = selectedMode === modeOption.mode;
                    const badgeText = isActive
                        ? `${modeOption.badgeText} · ACTIVE`
                        : modeOption.badgeText;

                    return (
                        <Button
                            onClick={() => handleModeChange(modeOption.mode)}
                            className={`${styles.modeCard} ${isActive ? styles.active : ''}`}
                        >
                            <div className={styles.cardHeader}>
                                <Chip
                                    label={badgeText}
                                    variant={
                                        isActive
                                            ? 'gold'
                                            : modeOption.chipVariant ||
                                              'default'
                                    }
                                />
                                <span
                                    className={`${styles.radio} ${isActive ? styles.radioChecked : ''}`}
                                />
                            </div>

                            <h3 className={styles.modeTitle}>
                                {modeOption.title}
                            </h3>
                            <p className={styles.modeDescription}>
                                {modeOption.description}
                            </p>
                        </Button>
                    );
                })}
            </div>

            <Button
                buttonStyle="primary"
                onClick={onFindPerfumes}
                disabled={isDisabled}
                className={styles.findButton}
            >
                {SEARCH_MODES_PANEL_TEXT.SEARCH_BUTTON_PLACEHOLDER}
            </Button>
        </div>
    );
};
