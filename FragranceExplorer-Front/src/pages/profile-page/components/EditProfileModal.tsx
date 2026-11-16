import { useState, useEffect, useRef } from 'react';
import { X, Check, AlertCircle } from 'lucide-react';
import { useProfile, type UserProfile } from '@/context/ProfileContext';
import { PERFUMES_COLLECTION } from '@/pages/perfume-catalog-page/perfumes.data';
import styles from './EditProfileModal.module.scss';

interface EditProfileModalProps {
    isOpen: boolean;
    onClose: () => void;
}

interface EditProfileFormProps {
    user: UserProfile;
    onClose: () => void;
}

const EditProfileForm = ({ user, onClose }: EditProfileFormProps) => {
    const { updateUser } = useProfile();

    const [name, setName] = useState(user.name);
    const [email, setEmail] = useState(user.email);
    const [bio, setBio] = useState(user.bio);
    const [level, setLevel] = useState(user.level || 'Fragrance Enthusiast');
    const [signatureScentId, setSignatureScentId] = useState(
        user.signatureScentId || ''
    );
    const [error, setError] = useState('');
    const [isSaved, setIsSaved] = useState(false);
    const timerRef = useRef<number | null>(null);

    useEffect(() => {
        const handleKeyDown = (e: KeyboardEvent) => {
            if (e.key === 'Escape') {
                onClose();
            }
        };
        window.addEventListener('keydown', handleKeyDown);
        return () => {
            window.removeEventListener('keydown', handleKeyDown);
            if (timerRef.current !== null) {
                window.clearTimeout(timerRef.current);
            }
        };
    }, [onClose]);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        setError('');

        if (!name.trim() || !email.trim()) {
            setError('Name and email address cannot be empty.');
            return;
        }

        const result = updateUser({
            name: name.trim(),
            email: email.trim().toLowerCase(),
            bio: bio.trim(),
            level,
            signatureScentId,
        });

        if (!result.success) {
            setError(result.error || 'Could not update profile.');
            return;
        }

        setIsSaved(true);
        timerRef.current = window.setTimeout(() => {
            setIsSaved(false);
            onClose();
        }, 450);
    };

    return (
        <div
            className={styles.overlay}
            onClick={onClose}
            role="dialog"
            aria-modal="true"
            aria-labelledby="edit-profile-modal-title"
        >
            <div className={styles.modal} onClick={(e) => e.stopPropagation()}>
                <div className={styles.modalHeader}>
                    <div>
                        <span className={styles.eyebrow}>CONNOISSEUR SETTINGS</span>
                        <h3 id="edit-profile-modal-title" className={styles.title}>
                            Edit Fragrance Profile
                        </h3>
                    </div>
                    <button
                        type="button"
                        className={styles.closeBtn}
                        onClick={onClose}
                        aria-label="Close edit profile modal"
                    >
                        <X size={20} strokeWidth={1.75} />
                    </button>
                </div>

                <form onSubmit={handleSubmit} className={styles.form}>
                    {error && (
                        <div
                            style={{
                                padding: '10px 14px',
                                borderRadius: '6px',
                                backgroundColor: '#fdf2f2',
                                border: '1px solid #f5c2c2',
                                color: '#9b2c2c',
                                fontSize: '12px',
                                display: 'flex',
                                alignItems: 'center',
                                gap: '6px',
                            }}
                            role="alert"
                        >
                            <AlertCircle size={15} />
                            <span>{error}</span>
                        </div>
                    )}

                    <div className={styles.fieldGroup}>
                        <label htmlFor="edit-profile-name" className={styles.label}>
                            Full Name
                        </label>
                        <input
                            id="edit-profile-name"
                            type="text"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            required
                            className={styles.input}
                            placeholder="e.g. Elena Vance"
                        />
                    </div>

                    <div className={styles.fieldGroup}>
                        <label htmlFor="edit-profile-email" className={styles.label}>
                            Email Address
                        </label>
                        <input
                            id="edit-profile-email"
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                            className={styles.input}
                            placeholder="elena.vance@fragrance.io"
                        />
                    </div>

                    <div className={styles.fieldGroup}>
                        <label htmlFor="edit-profile-level" className={styles.label}>
                            Connoisseur Level / Title
                        </label>
                        <select
                            id="edit-profile-level"
                            value={level}
                            onChange={(e) => setLevel(e.target.value)}
                            className={styles.select}
                        >
                            <option value="Fragrance Connoisseur">Fragrance Connoisseur</option>
                            <option value="Niche Perfume Collector">Niche Perfume Collector</option>
                            <option value="Olfactory Minimalist">Olfactory Minimalist</option>
                            <option value="Artisanal Nose in Training">
                                Artisanal Nose in Training
                            </option>
                            <option value="Fragrance Enthusiast">Fragrance Enthusiast</option>
                        </select>
                    </div>

                    <div className={styles.fieldGroup}>
                        <label htmlFor="edit-profile-signature" className={styles.label}>
                            Signature Scent
                        </label>
                        <select
                            id="edit-profile-signature"
                            value={signatureScentId}
                            onChange={(e) => setSignatureScentId(e.target.value)}
                            className={styles.select}
                        >
                            <option value="">— None chosen yet —</option>
                            {PERFUMES_COLLECTION.map((perfume) => (
                                <option key={perfume.id} value={perfume.id}>
                                    {perfume.brand} — {perfume.name} ({perfume.family})
                                </option>
                            ))}
                        </select>
                    </div>

                    <div className={styles.fieldGroup}>
                        <label htmlFor="edit-profile-bio" className={styles.label}>
                            Personal Olfactory Manifesto (Bio)
                        </label>
                        <textarea
                            id="edit-profile-bio"
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
                        <button type="submit" className={styles.saveBtn}>
                            {isSaved ? (
                                <>
                                    <Check
                                        size={14}
                                        style={{ marginRight: 5, verticalAlign: 'middle' }}
                                    />
                                    Saved!
                                </>
                            ) : (
                                'Save Changes'
                            )}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export const EditProfileModal = ({ isOpen, onClose }: EditProfileModalProps) => {
    const { user } = useProfile();

    if (!isOpen || !user) return null;

    return <EditProfileForm user={user} onClose={onClose} />;
};
