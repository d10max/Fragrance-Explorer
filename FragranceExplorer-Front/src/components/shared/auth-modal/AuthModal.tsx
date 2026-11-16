import { useState, useEffect } from 'react';
import { Eye, EyeOff, X, Sparkles, AlertCircle } from 'lucide-react';
import { useProfile } from '@/context/ProfileContext';
import styles from './AuthModal.module.scss';

export const AuthModal = () => {
    const { isAuthModalOpen, closeAuthModal, register, login, loadDemoProfile } =
        useProfile();

    const [mode, setMode] = useState<'register' | 'login'>('register');
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [showPassword, setShowPassword] = useState(false);
    const [level, setLevel] = useState('Fragrance Enthusiast');
    const [bio, setBio] = useState('');
    const [error, setError] = useState('');

    useEffect(() => {
        if (!isAuthModalOpen) return;
        const handleKeyDown = (e: KeyboardEvent) => {
            if (e.key === 'Escape') {
                closeAuthModal();
            }
        };
        window.addEventListener('keydown', handleKeyDown);
        return () => window.removeEventListener('keydown', handleKeyDown);
    }, [isAuthModalOpen, closeAuthModal]);

    if (!isAuthModalOpen) return null;

    const handleClose = () => {
        setError('');
        setPassword('');
        setConfirmPassword('');
        closeAuthModal();
    };

    const handleSwitchMode = (newMode: 'register' | 'login') => {
        setMode(newMode);
        setError('');
        setPassword('');
        setConfirmPassword('');
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        setError('');

        if (mode === 'register') {
            if (!name.trim() || !email.trim()) {
                setError('Please provide your name and email address.');
                return;
            }
            if (password.length < 6) {
                setError('Password must be at least 6 characters long.');
                return;
            }
            if (password !== confirmPassword) {
                setError('Passwords do not match.');
                return;
            }

            const result = register({ name, email, password, level, bio });
            if (!result.success) {
                setError(result.error || 'Could not register account.');
                return;
            }

            setName('');
            setEmail('');
            setPassword('');
            setConfirmPassword('');
            setBio('');
        } else {
            if (!email.trim() || !password) {
                setError('Please provide your email and password.');
                return;
            }

            const success = login(email, password);
            if (!success) {
                setError(
                    'Invalid email or password. Please verify your credentials or use Demo Mode below.'
                );
                return;
            }

            setPassword('');
        }
    };

    return (
        <div
            className={styles.overlay}
            onClick={handleClose}
            role="dialog"
            aria-modal="true"
            aria-labelledby="auth-modal-title"
        >
            <div className={styles.modal} onClick={(e) => e.stopPropagation()}>
                {/* Header with Close */}
                <div className={styles.modalHeader}>
                    <div>
                        <span className={styles.eyebrow}>
                            FRAGRANCE EXPLORER &middot; MEMBERSHIP
                        </span>
                        <h3 id="auth-modal-title" className={styles.title}>
                            {mode === 'register'
                                ? 'Create Connoisseur Profile'
                                : 'Sign In to Your Wardrobe'}
                        </h3>
                    </div>
                    <button
                        type="button"
                        className={styles.closeBtn}
                        onClick={handleClose}
                        aria-label="Close authentication modal"
                    >
                        <X size={20} strokeWidth={1.75} />
                    </button>
                </div>

                {/* Tabs: Register / Login */}
                <div className={styles.tabBar}>
                    <button
                        type="button"
                        className={`${styles.tabBtn} ${
                            mode === 'register' ? styles.tabBtnActive : ''
                        }`}
                        onClick={() => handleSwitchMode('register')}
                    >
                        Create New Profile
                    </button>
                    <button
                        type="button"
                        className={`${styles.tabBtn} ${
                            mode === 'login' ? styles.tabBtnActive : ''
                        }`}
                        onClick={() => handleSwitchMode('login')}
                    >
                        Sign In
                    </button>
                </div>

                {/* Error Banner */}
                {error && (
                    <div className={styles.errorAlert} role="alert">
                        <AlertCircle
                            size={15}
                            style={{ marginRight: 6, verticalAlign: 'text-bottom' }}
                        />
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit} className={styles.form}>
                    {mode === 'register' ? (
                        <>
                            <div className={styles.fieldGroup}>
                                <label htmlFor="auth-name" className={styles.label}>
                                    Your Name
                                </label>
                                <input
                                    id="auth-name"
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
                                <label htmlFor="auth-reg-email" className={styles.label}>
                                    Email Address
                                </label>
                                <input
                                    id="auth-reg-email"
                                    type="email"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    required
                                    className={styles.input}
                                    placeholder="name@university.edu"
                                />
                            </div>

                            {/* Password Field */}
                            <div className={styles.fieldGroup}>
                                <div className={styles.labelRow}>
                                    <label htmlFor="auth-reg-pwd" className={styles.label}>
                                        Create Password
                                    </label>
                                    <button
                                        type="button"
                                        className={styles.togglePwdBtn}
                                        onClick={() => setShowPassword((prev) => !prev)}
                                    >
                                        {showPassword ? (
                                            <>
                                                <EyeOff size={13} style={{ marginRight: 4, verticalAlign: 'middle' }} />
                                                Hide
                                            </>
                                        ) : (
                                            <>
                                                <Eye size={13} style={{ marginRight: 4, verticalAlign: 'middle' }} />
                                                Show
                                            </>
                                        )}
                                    </button>
                                </div>
                                <input
                                    id="auth-reg-pwd"
                                    type={showPassword ? 'text' : 'password'}
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required
                                    minLength={6}
                                    className={styles.input}
                                    placeholder="At least 6 characters"
                                />
                            </div>

                            {/* Confirm Password Field */}
                            <div className={styles.fieldGroup}>
                                <label htmlFor="auth-reg-confirm" className={styles.label}>
                                    Confirm Password
                                </label>
                                <input
                                    id="auth-reg-confirm"
                                    type={showPassword ? 'text' : 'password'}
                                    value={confirmPassword}
                                    onChange={(e) => setConfirmPassword(e.target.value)}
                                    required
                                    minLength={6}
                                    className={styles.input}
                                    placeholder="Repeat your password"
                                />
                            </div>

                            <div className={styles.fieldGroup}>
                                <label htmlFor="auth-reg-level" className={styles.label}>
                                    Olfactory Experience Level
                                </label>
                                <select
                                    id="auth-reg-level"
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
                                <label htmlFor="auth-reg-bio" className={styles.label}>
                                    Fragrance Notes or Philosophy (Optional)
                                </label>
                                <input
                                    id="auth-reg-bio"
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
                                <label htmlFor="auth-login-email" className={styles.label}>
                                    Email Address
                                </label>
                                <input
                                    id="auth-login-email"
                                    type="email"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    required
                                    className={styles.input}
                                    placeholder="Enter your registered email"
                                    autoFocus
                                />
                            </div>

                            <div className={styles.fieldGroup}>
                                <div className={styles.labelRow}>
                                    <label htmlFor="auth-login-pwd" className={styles.label}>
                                        Password
                                    </label>
                                    <button
                                        type="button"
                                        className={styles.togglePwdBtn}
                                        onClick={() => setShowPassword((prev) => !prev)}
                                    >
                                        {showPassword ? (
                                            <>
                                                <EyeOff size={13} style={{ marginRight: 4, verticalAlign: 'middle' }} />
                                                Hide
                                            </>
                                        ) : (
                                            <>
                                                <Eye size={13} style={{ marginRight: 4, verticalAlign: 'middle' }} />
                                                Show
                                            </>
                                        )}
                                    </button>
                                </div>
                                <input
                                    id="auth-login-pwd"
                                    type={showPassword ? 'text' : 'password'}
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required
                                    className={styles.input}
                                    placeholder="Enter your password"
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
                        <span className={styles.demoLabel}>
                            <Sparkles size={11} style={{ marginRight: 4, verticalAlign: 'middle' }} />
                            EXAM / DEMO MODE
                        </span>
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
