import { Outlet } from 'react-router-dom';
import { Header } from '@/components/layout/header/Header';
import { Footer } from '@/components/layout/footer/Footer';
import { CompareProvider } from '@/context/CompareContext';
import { CompareTray } from '@/components/shared/compare-tray/CompareTray';

export const Layout = () => {
    return (
        <CompareProvider>
            <Header />
            <main>
                <Outlet />
            </main>
            <CompareTray />
            <Footer />
        </CompareProvider>
    );
};
