export const ROUTES = {
    ROOT: '/',
    PERFUME_FINDER: {
        FULL: '/finder',
        PATH: 'finder',
    },
    PERFUME_CATALOG: {
        FULL: '/catalog',
        PATH: 'catalog',
    },
    PERFUME_DETAIL: {
        FULL: (id: string) => `/catalog/${id}`,
        PATH: 'catalog/:id',
    },
};
