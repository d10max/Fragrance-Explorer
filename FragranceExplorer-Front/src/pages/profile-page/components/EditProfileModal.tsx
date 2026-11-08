import { useState } from 'react';
import { useProfile } from '@/context/ProfileContext';
import { PERFUMES_COLLECTION } from '@/pages/perfume-catalog-page/perfumes.data';
import styles from './EditProfileModal.module.scss';

interface EditProfileModalProps {
    isOpen: boolean;
    onClose: () => void;
}

export const EditProfileModal = ({ isOpen, onClose }: EditProfileModalProps) => {
    const { user, updateUser } = useProfile();

    const [name, setName] = useState(user?.name || '');
    const [email, setEmail] = useState(user?.email || '');
    const [bio, setBio] = useState(user?.bio || '');
    const [level, setLevel] = useState(user?.level || 'Fragrance Enthusiast');
    const [signatureScentId, setSignatureScentId] = useState(
        user?.signatureScentId || 'bal-dafrique'
    );
    const [isSaved, setIsSaved] = useState(false);

    if (!isOpen || !user) return null;

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        updateUser({
            name,
            email,
            bio,
            level,
            signatureScentId,
        });
        setIsSaved(true);
        setTimeout(() => {
            setIsSaved(false);
            onClose();
        }, 600);
    };

    return (
        <div className={styles.overlay} onClick={onClose}>
            <div className={styles.modal} onClick={(e) => e.stopPropagation()}>
                <div className={styles.modalHeader}>
                    <div>
                        <span className={styles.eyebrow}>CONNOISSEUR SETTINGS</span>
                        <h3 className={styles.title}>Edit Fragrance Profile</h3>
                    </div>
                    <button
                        type="button"
                        className={styles.closeBtn}
                        onClick={onClose}
                    >
                        &times;
                    </button>
                </div>

                <form onSubmit={handleSubmit} className={styles.form}>
                    <div className={styles.fieldGroup}>
                        <label className={styles.label}>Full Name</label>
                        <input
                            type="text"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            required
                            className={styles.input}
                            placeholder="e.g. Elena Vance"
                        />
                    </div>

                    <div className={styles.fieldGroup}>
                        <label className={styles.label}>Email Address</label>
                        <input
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                            className={styles.input}
                            placeholder="elena.vance@fragrance.io"
                        />
                    </div>

                    <div className={styles.fieldGroup}>
                        <label className={styles.label}>Connoisseur Level / Title</label>
                        <select
                            value={level}
                            onChange={(e) => setLevel(e.target.value)}
                            className={styles.select}
                        >
                            <option value="Fragrance Connoisseur">Fragrance Connoisseur</option>
                            <option value="Niche Perfume Collector">Niche Perfume Collector</option>
                            <option value="Olfactory Minimalist">Olfactory Minimalist</option>
                            <option value="Artisanal Nose in Training">Artisanal Nose in Training</option>
                            <option value="Fragrance Enthusiast">Fragrance Enthusiast</option>
                        </select>
                    </div>

                    <div className={styles.fieldGroup}>
                        <label className={styles.label}>Signature Scent</label>
                        <select
                            value={signatureScentId}
                            onChange={(e) => setSignatureScentId(e.target.value)}
                            className={styles.select}
                        >
                            {PERFUMES_COLLECTION.slice(0, 20).map((perfume) => (
                                <option key={perfume.id} value={perfume.id}>
                                    {perfume.brand} — {perfume.name} ({perfume.family})
                                </option>
                            ))}
                        </select>
                    </div>

                    <div className={styles.fieldGroup}>
                        <label className={styles.label}>Personal Olfactory Manifesto (Bio)</label>
                        <textarea
                            value={bio}
                            onChange={(e) => setBio(e.target.value)}
                            rows={3}
                            className={styles.textarea}
                            placeholder="Describe your signature accords, favorite notes, and perfume philosophy..."
                        />
                    </div>

                    <div className={styles.modalFooter}>
                        <button
                            type="button"
                            className={styles.cancelBtn}
                            onClick={onClose}
                        >
                            Cancel
                        </button>
                        <button
                            type="submit"
                            className={styles.saveBtn}
                        >
                            {isSaved ? 'Saved! ✓' : 'Save Changes'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};
