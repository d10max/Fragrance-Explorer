export interface HouseItem {
    id: string;
    name: string;
    location: string;
    count: string;
}

export const HOUSES_DATA: HouseItem[] = [
    // Топ-6 за замовчуванням
    { id: 'byredo', name: 'BYREDO', location: 'Stockholm · Est. 2006', count: '24 perfumes' },
    { id: 'le-labo', name: 'LE LABO', location: 'New York · Est. 2006', count: '19 perfumes' },
    { id: 'diptyque', name: 'DIPTYQUE', location: 'Paris · Est. 1961', count: '32 perfumes' },
    { id: 'hermes', name: 'HERMÈS', location: 'Paris · Est. 1837', count: '28 perfumes' },
    { id: 'chanel', name: 'CHANEL', location: 'Paris · Est. 1910', count: '21 perfumes' },
    { id: 'aesop', name: 'AESOP', location: 'Melbourne · Est. 1987', count: '11 perfumes' },

    // Додаткові всесвітньо відомі нішеві та люксові доми
    { id: 'tom-ford', name: 'TOM FORD', location: 'New York · Est. 2005', count: '38 perfumes' },
    { id: 'mfk', name: 'MAISON FRANCIS KURKDJIAN', location: 'Paris · Est. 2009', count: '22 perfumes' },
    { id: 'creed', name: 'CREED', location: 'London · Est. 1760', count: '29 perfumes' },
    { id: 'guerlain', name: 'GUERLAIN', location: 'Paris · Est. 1828', count: '45 perfumes' },
    { id: 'jo-malone', name: 'JO MALONE', location: 'London · Est. 1990', count: '34 perfumes' },
    { id: 'kilian', name: 'KILIAN PARIS', location: 'Paris · Est. 2007', count: '26 perfumes' },
    { id: 'pdm', name: 'PARFUMS DE MARLY', location: 'Paris · Est. 2009', count: '18 perfumes' },
    { id: 'frederic-malle', name: 'ÉDITIONS DE PARFUMS', location: 'Paris · Est. 2000', count: '25 perfumes' },
    { id: 'serge-lutens', name: 'SERGE LUTENS', location: 'Paris · Est. 2000', count: '30 perfumes' },
    { id: 'acqua-di-parma', name: 'ACQUA DI PARMA', location: 'Parma · Est. 1916', count: '27 perfumes' },
    { id: 'penhaligons', name: 'PENHALIGON’S', location: 'London · Est. 1870', count: '31 perfumes' },
    { id: 'nishane', name: 'NISHANE', location: 'Istanbul · Est. 2012', count: '16 perfumes' },
    { id: 'amouage', name: 'AMOUAGE', location: 'Muscat · Est. 1983', count: '33 perfumes' },
    { id: 'margiela', name: 'MAISON MARGIELA', location: 'Paris · Est. 2010', count: '20 perfumes' },
    { id: 'xerjoff', name: 'XERJOFF', location: 'Turin · Est. 2003', count: '35 perfumes' },
    { id: 'roja', name: 'ROJA PARFUMS', location: 'London · Est. 2011', count: '15 perfumes' },
    { id: 'vilhelm', name: 'VILHELM PARFUMERIE', location: 'New York · Est. 2015', count: '14 perfumes' },
    { id: 'bdk', name: 'BDK PARFUMS', location: 'Paris · Est. 2016', count: '17 perfumes' },
    { id: 'memo', name: 'MEMO PARIS', location: 'Paris · Est. 2007', count: '23 perfumes' },
    { id: 'initio', name: 'INITIO PARFUMS PRIVÉS', location: 'Paris · Est. 2015', count: '16 perfumes' },
    { id: 'matiere-premiere', name: 'MATIERE PREMIERE', location: 'Paris · Est. 2019', count: '12 perfumes' },
    { id: 'diur', name: 'DIOR', location: 'Paris · Est. 1947', count: '40 perfumes' },
    { id: 'saint-laurent', name: 'YVES SAINT LAURENT', location: 'Paris · Est. 1961', count: '36 perfumes' },
    { id: 'cartier', name: 'CARTIER', location: 'Paris · Est. 1847', count: '18 perfumes' },
    { id: 'prada', name: 'PRADA', location: 'Milan · Est. 1913', count: '22 perfumes' },
    { id: 'giorgio-armani', name: 'GIORGIO ARMANI', location: 'Milan · Est. 1975', count: '35 perfumes' },
    { id: 'givenchy', name: 'GIVENCHY', location: 'Paris · Est. 1952', count: '29 perfumes' },
    { id: 'santa-maria-novella', name: 'SANTA MARIA NOVELLA', location: 'Florence · Est. 1221', count: '15 perfumes' },
    { id: 'l-artisan', name: 'L’ARTISAN PARFUMEUR', location: 'Paris · Est. 1976', count: '28 perfumes' },
    { id: 'floraiku', name: 'FLORAIKU', location: 'Paris · Est. 2017', count: '13 perfumes' },
    { id: 'ex-nihilo', name: 'EX NIHILO', location: 'Paris · Est. 2013', count: '19 perfumes' },
    { id: 'clive-christian', name: 'CLIVE CHRISTIAN', location: 'London · Est. 1999', count: '14 perfumes' },
    { id: 'atkinsons', name: 'ATKINSONS', location: 'London · Est. 1799', count: '21 perfumes' },
    { id: 'ormonde-jayne', name: 'ORMONDE JAYNE', location: 'London · Est. 2002', count: '18 perfumes' },
    { id: 'parle-moi', name: 'PARLE MOI DE PARFUM', location: 'Paris · Est. 2016', count: '12 perfumes' },
    { id: 'ds-durga', name: 'D.S. & DURGA', location: 'New York · Est. 2008', count: '17 perfumes' },
    { id: 'fueguia', name: 'FUEGUIA 1833', location: 'Buenos Aires · Est. 2010', count: '22 perfumes' },
    { id: 'boadicea', name: 'BOADICEA THE VICTORIOUS', location: 'London · Est. 2008', count: '25 perfumes' },
    { id: 'sospiro', name: 'SOSPIRO', location: 'Turin · Est. 2011', count: '16 perfumes' },
    { id: 'nasomatto', name: 'NASOMATTO', location: 'Amsterdam · Est. 2007', count: '11 perfumes' },
    { id: 'zoologist', name: 'ZOOLOGIST PERFUMES', location: 'Toronto · Est. 2013', count: '15 perfumes' },
    { id: 'essential-parfums', name: 'ESSENTIAL PARFUMS', location: 'Paris · Est. 2018', count: '10 perfumes' },
];
