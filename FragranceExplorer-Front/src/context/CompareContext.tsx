import { createContext, useContext, useState, type ReactNode } from 'react';
import { useNavigate } from 'react-router-dom';
import { ROUTES } from '@/const/routes';

interface CompareContextType {
    compareIds: string[];
    addToCompare: (id: string) => void;
    removeFromCompare: (id: string) => void;
    clearCompare: () => void;
    setComparePair: (idA: string, idB: string) => void;
}

const CompareContext = createContext<CompareContextType | undefined>(undefined);

export const CompareProvider = ({ children }: { children: ReactNode }) => {
    const [compareIds, setCompareIds] = useState<string[]>([]);
    const navigate = useNavigate();

    const addToCompare = (id: string) => {
        if (compareIds.includes(id)) {
            // If already 2 items, navigate straight to compare page
            if (compareIds.length === 2) {
                navigate(
                    `${ROUTES.PERFUME_COMPARE.FULL}?scentA=${compareIds[0]}&scentB=${compareIds[1]}`
                );
            }
            return;
        }

        if (compareIds.length === 0) {
            setCompareIds([id]);
        } else if (compareIds.length === 1) {
            const firstId = compareIds[0];
            const nextPair = [firstId, id];
            setCompareIds(nextPair);
            navigate(
                `${ROUTES.PERFUME_COMPARE.FULL}?scentA=${firstId}&scentB=${id}`
            );
        } else {
            // Replace the second scent and open comparison
            const firstId = compareIds[0];
            setCompareIds([firstId, id]);
            navigate(
                `${ROUTES.PERFUME_COMPARE.FULL}?scentA=${firstId}&scentB=${id}`
            );
        }
    };

    const removeFromCompare = (id: string) => {
        setCompareIds((prev) => prev.filter((item) => item !== id));
    };

    const clearCompare = () => {
        setCompareIds([]);
    };

    const setComparePair = (idA: string, idB: string) => {
        setCompareIds([idA, idB]);
    };

    return (
        <CompareContext.Provider
            value={{
                compareIds,
                addToCompare,
                removeFromCompare,
                clearCompare,
                setComparePair,
            }}
        >
            {children}
        </CompareContext.Provider>
    );
};

export const useCompare = () => {
    const context = useContext(CompareContext);
    if (!context) {
        throw new Error('useCompare must be used within a CompareProvider');
    }
    return context;
};

