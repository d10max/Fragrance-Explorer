import { Outlet } from 'react-router-dom';
import { Header } from '@/components/layout/header/Header';
import { Footer } from '@/components/layout/footer/Footer';
import { CompareProvider } from '@/context/CompareContext';
import { ProfileProvider } from '@/context/ProfileContext';
import { CompareTray } from '@/components/shared/compare-tray/CompareTray';
import { AuthModal } from '@/components/shared/auth-modal/AuthModal';
import { FEATURES } from '@/const/common';

export const Layout = () => {
    return (
        <ProfileProvider>
            <CompareProvider>
                <Header />
                <main>
                    <Outlet />
                </main>
                <CompareTray />
                <Footer />
                {FEATURES.ENABLE_PROFILE && <AuthModal />}
            </CompareProvider>
        </ProfileProvider>
    );
};
