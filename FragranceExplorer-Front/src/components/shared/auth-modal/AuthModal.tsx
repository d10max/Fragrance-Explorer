import { useState } from 'react';
import { useProfile } from '@/context/ProfileContext';
import styles from './AuthModal.module.scss';

export const AuthModal = () => {
    const { isAuthModalOpen, closeAuthModal, register, login, loadDemoProfile } =
        useProfile();

    const [mode, setMode] = useState<'register' | 'login'>('register');
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [level, setLevel] = useState('Fragrance Enthusiast');
    const [bio, setBio] = useState('');

    if (!isAuthModalOpen) return null;

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (mode === 'register') {
            if (!name.trim() || !email.trim()) return;
            register({ name, email, level, bio });
        } else {
            if (!email.trim()) return;
            login(email);
        }
    };

    return (
        <div className={styles.overlay} onClick={closeAuthModal}>
            <div className={styles.modal} onClick={(e) => e.stopPropagation()}>
                {/* Header with Close */}
                <div className={styles.modalHeader}>
                    <div>
                        <span className={styles.eyebrow}>
                            FRAGRANCE EXPLORER &middot; MEMBERSHIP
                        </span>
                        <h3 className={styles.title}>
                            {mode === 'register'
                                ? 'Create Connoisseur Profile'
                                : 'Sign In to Your Wardrobe'}
                        </h3>
                    </div>
                    <button
                        type="button"
                        className={styles.closeBtn}
                        onClick={closeAuthModal}
                    >
                        &times;
                    </button>
                </div>

                {/* Tabs: Register / Login */}
                <div className={styles.tabBar}>
                    <button
                        type="button"
                        className={`${styles.tabBtn} ${
                            mode === 'register' ? styles.tabBtnActive : ''
                        }`}
                        onClick={() => setMode('register')}
                    >
                        Create New Profile
                    </button>
                    <button
                        type="button"
                        className={`${styles.tabBtn} ${
                            mode === 'login' ? styles.tabBtnActive : ''
                        }`}
                        onClick={() => setMode('login')}
                    >
                        Sign In
                    </button>
                </div>

                <form onSubmit={handleSubmit} className={styles.form}>
                    {mode === 'register' ? (
                        <>
                            <div className={styles.fieldGroup}>
                                <label className={styles.label}>Your Name</label>
                                <input
                                    type="text"
                                    value={name}
                                    onChange={(e) => setName(e.target.value)}
                                    required
                                    className={styles.input}
                                    placeholder="e.g. Max Koval"
                                    autoFocus
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
                                    placeholder="name@university.edu"
                                />
                            </div>

                            <div className={styles.fieldGroup}>
                                <label className={styles.label}>
                                    Olfactory Experience Level
                                </label>
                                <select
                                    value={level}
                                    onChange={(e) => setLevel(e.target.value)}
                                    className={styles.select}
                                >
                                    <option value="Fragrance Enthusiast">
                                        Fragrance Enthusiast
                                    </option>
                                    <option value="Niche Perfume Collector">
                                        Niche Perfume Collector
                                    </option>
                                    <option value="Olfactory Minimalist">
                                        Olfactory Minimalist
                                    </option>
                                    <option value="Artisanal Nose in Training">
                                        Artisanal Nose in Training
                                    </option>
                                    <option value="Fragrance Connoisseur">
                                        Fragrance Connoisseur
                                    </option>
                                </select>
                            </div>

                            <div className={styles.fieldGroup}>
                                <label className={styles.label}>
                                    Fragrance Notes or Philosophy (Optional)
                                </label>
                                <input
                                    type="text"
                                    value={bio}
                                    onChange={(e) => setBio(e.target.value)}
                                    className={styles.input}
                                    placeholder="e.g. In search of fresh vetivers and dry cedarwood."
                                />
                            </div>

                            <button type="submit" className={styles.submitBtn}>
                                Create Profile &amp; Unlock Wardrobe &rarr;
                            </button>
                        </>
                    ) : (
                        <>
                            <div className={styles.fieldGroup}>
                                <label className={styles.label}>Email Address</label>
                                <input
                                    type="email"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    required
                                    className={styles.input}
                                    placeholder="Enter your registered email"
                                    autoFocus
                                />
                            </div>

                            <button type="submit" className={styles.submitBtn}>
                                Sign In &rarr;
                            </button>
                        </>
                    )}
                </form>

                {/* Quick Demo Access Bar */}
                <div className={styles.demoBar}>
                    <div className={styles.demoInfo}>
                        <span className={styles.demoLabel}>EXAM / DEMO MODE</span>
                        <p className={styles.demoHint}>
                            Want to test with full pre-loaded data &amp; charts immediately?
                        </p>
                    </div>
                    <button
                        type="button"
                        className={styles.demoBtn}
                        onClick={loadDemoProfile}
                    >
                        Load Demo: Elena Vance
                    </button>
                </div>
            </div>
        </div>
    );
};
