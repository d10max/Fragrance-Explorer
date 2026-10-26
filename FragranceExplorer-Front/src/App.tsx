import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import { Layout } from '@/components/layout/layout/Layout';
import { PerfumeFinderPage } from '@/pages/perfume-finder-page/PerfumeFinderPage';
import { PerfumeCatalogPage } from '@/pages/perfume-catalog-page/PerfumeCatalogPage';
import { ROUTES } from './const/routes';

export const App = () => {
    return (
        <BrowserRouter>
            <Routes>
                <Route path={ROUTES.ROOT} element={<Layout />}>
                    <Route
                        index
                        element={
                            <Navigate to={ROUTES.PERFUME_FINDER.FULL} replace />
                        }
                    />
                    <Route
                        index
                        path={ROUTES.PERFUME_FINDER.PATH}
                        element={<PerfumeFinderPage />}
                    />
                    <Route
                        path={ROUTES.PERFUME_CATALOG.PATH}
                        element={<PerfumeCatalogPage />}
                    />
                </Route>
            </Routes>
        </BrowserRouter>
    );
};
