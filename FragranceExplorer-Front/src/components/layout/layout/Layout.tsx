import { Outlet } from 'react-router-dom';
import { Header } from '@/components/layout/header/Header';
import { Footer } from '@/components/layout/footer/Footer';

export const Layout = () => {
    return (
        <>
            <Header />
            <main>
                <Outlet />
            </main>
            <Footer />
        </>
    );
};
