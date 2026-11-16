# Звіт по розробці модуля «Профіль користувача та Ольфакторна аналітика смаків» і використанню сторонніх бібліотек

**Виконавець:** Андрій (`bloody2310` / локальні коміти `kozor`)  
**Модуль:** Особистий кабінет користувача (`/profile`), система автентифікації та гостьового доступу (`AuthModal`, `ProfileContext`), персональний гардероб ароматів (`FavoritePerfumes`) та візуальна аналітика смаків (`ScentTasteChart`).

---

## 1. Звіт про використання сторонніх бібліотек та фреймворків

Відповідно до вимог лабораторної роботи щодо використання сторонніх бібліотек/фреймворків, у модулі профілю та аналітики смаків інтегровано дві спеціалізовані React-бібліотеки: **`Recharts`** та **`Lucide React`**.

### 1.1. Бібліотека візуалізації даних `Recharts` (`v2.x` / `v3.x`)
* **Призначення:** Побудова адаптивних інтерактивних діаграм на основі збережених ароматів користувача у компоненті [`ScentTasteChart.tsx`](../FragranceExplorer-Front/src/pages/profile-page/components/ScentTasteChart.tsx).
* **Обґрунтування вибору (порівняння з альтернативами):**
  - Порівняно з імперативними бібліотеками (`Chart.js`, чистий `D3.js`), **`Recharts`** надає декларативну компонентну архітектуру React (`<PieChart>`, `<RadarChart>`, `<ResponsiveContainer>`), що ідеально поєднується зі станом `ProfileContext`.
  - Порівняно з ручною побудовою SVG-дуг, `Recharts` автоматично обчислює тригонометричні координати секторів і полігонів, підтримує плавні переходи, адаптивні контейнери та кастомні спливаючі підказки (`<Tooltip>`) без зміщення координат при наведенні курсору.
* **Використані компоненти `Recharts` у проекті:**
  1. **`PieChart`, `Pie`, `Cell`** — реалізація кругової діаграми **Scent Wheel Ratio (Olfactory Families Distribution)** з внутрішнім радіусом (`innerRadius={64}`, `outerRadius={92}`), відступами між секторами (`paddingAngle={2}`) та інтерактивним виділенням активної ольфакторної родини (*Woody, Citrus, Fresh, Spicy*).
  2. **`RadarChart`, `PolarGrid`, `PolarAngleAxis`, `PolarRadiusAxis`, `Radar`, `Tooltip`, `ResponsiveContainer`** — реалізація 6-осьової радарної діаграми **Taste Equilibrium (Olfactory Radar Spectrum)** за 6 вимірами (*Woody & Earthy, Citrus & Bright, Fresh Green, Resinous Amber, Spicy Warmth, Soft Floral*).

#### Приклад інтеграції `Recharts` у `ScentTasteChart.tsx`:
```tsx
<ResponsiveContainer width="100%" height={290}>
    <RadarChart cx="50%" cy="50%" outerRadius="68%" data={radarChartData}>
        <PolarGrid stroke="#e5e0d7" strokeDasharray="3 3" />
        <PolarAngleAxis dataKey="axisLabel" tick={{ fill: '#77736c', fontSize: 10.5 }} />
        <PolarRadiusAxis angle={90} domain={[0, 100]} tick={false} axisLine={false} />
        <Radar
            name="Olfactory Affinity"
            dataKey="value"
            stroke="#c59b27"
            strokeWidth={2.5}
            fill="#c59b27"
            fillOpacity={0.22}
        />
        <Tooltip formatter={(value) => [`${value}% Affinity`, 'Score']} />
    </RadarChart>
</ResponsiveContainer>
```

### 1.2. Бібліотека векторної іконографії `lucide-react`
* **Призначення:** Уніфікована система мінімалістичних векторних іконок у стилі *quiet luxury* для навігаційних вкладок, карток гардеробу, кнопок збереження та модальних вікон.
* **Використані компоненти:** `Bookmark`, `BarChart3`, `Clock`, `PenLine`, `LogOut`, `Eye`, `EyeOff`, `Sparkles`, `Compass`, `Plus`, `Check`, `AlertCircle`, `X`.
* **Переваги:** Підтримка Tree-shaking (у фінальний бандл потрапляють лише імпортовані іконки), єдина товщина ліній (`strokeWidth`), масштабування через пропси `size` та повна типізація для TypeScript.

---

## 2. Опис реалізованого функціоналу модуля

### 2.1. Кнопка профілю у Header (`Header.tsx`)
* **Авторизований режим:** відображає аватарку з ініціалами користувача, ім'я та статус знавця (*Connoisseur / Enthusiast*). Підтримує активний стан при переході на `/profile` та золотий hover-ефект.
* **Гостьовий режим:** відображає кнопку `Sign In / Join`.

### 2.2. Гостьовий режим та автентифікація (`AuthModal.tsx` та `ProfileContext.tsx`)
* Новий відвідувач за замовчуванням перебуває в **Guest Mode**:
  - Може переглядати енциклопедію та шукати парфуми, але при спробі зберегти аромат у гардероб відкривається модальне вікно авторизації.
* **Модальне вікно реєстрації та входу (`AuthModal.tsx`):**
  - Вкладка **Create New Profile**: введення імені, email (з перевіркою на дублікати в реєстрі), пароля (мінімум 6 символів) та підтвердження пароля, перемикач видимості пароля (`Eye` / `EyeOff`), вибір рівня знавця та ольфакторного маніфесту.
  - Вкладка **Sign In**: вхід за зареєстрованим email та паролем.
  - Кнопка **Exam / Demo Mode (Elena Vance)**: миттєвий перехід у демонстраційний профіль із передзаповненим гардеробом (6 нішевих ароматів) та активними діаграмами.
* **Безпека та цілісність даних (`ProfileContext.tsx`):**
  - Паролі хешуються детермінованою функцією з сіллю (`hashPassword(password, email)`) перед збереженням у `localStorage`.
  - При редагуванні профілю (`updateUser`) зміни синхронізуються в реєстрі користувачів (`fe_registered_users_registry`). У разі зміни email гардероб та історія автоматично мігрують на новий ключ.

### 2.3. Сторінка профілю (`/profile` — `ProfilePage.tsx`)
* **Гостьовий екран:** вітальний банер, картки переваг створення профілю та прев'ю діаграм.
* **Шапка профілю:** аватар з ініціалами, ім'я, бейдж рівня, дата реєстрації, біографія, модальне вікно редагування (`EditProfileModal.tsx`) та кнопка виходу (`Sign Out`).
* **4 інтерактивні статистичні картки:**
  1. *Saved Wardrobe* — кількість збережених ароматів.
  2. *Dominant Profile* — домінуюча ольфакторна родина та її частка у відсотках.
  3. *Top Key Note* — найчастіша нота серед збережених композицій.
  4. *Signature Flacon* — обраний сигнатурний аромат із переходом на його досьє.
* **3 вкладки кабінету:**
  - **Taste Analytics & Diagrams:** діаграми `Recharts` (кругова та радарна) + прогрес-бари *Top Selected Notes & Ingredients*. Якщо гардероб порожній — відображається екран калібрування.
  - **Saved Fragrances (`FavoritePerfumes.tsx`):** персональна полиця ароматів із фільтрацією за родинами, пошуком, швидким додаванням аромату (`+ Quick add fragrance...`), видаленням із гардеробу та додаванням до порівняння (`+ Compare`).
  - **Discovery & Search Log:** журнал активності користувача зі збереженням у `localStorage`.

---

## 3. Перелік створених та модифікованих файлів і комітів

### Основні файли модуля:
* `FragranceExplorer-Front/src/context/ProfileContext.tsx` — глобальний контекст профілю, автентифікації, хешування паролів, гардеробу, історії та розрахунку ольфакторної аналітики.
* `FragranceExplorer-Front/src/components/shared/auth-modal/AuthModal.tsx` (+ `.module.scss`) — модальне вікно реєстрації, входу та демо-режиму.
* `FragranceExplorer-Front/src/pages/profile-page/ProfilePage.tsx` (+ `.module.scss`) — головна сторінка профілю користувача.
* `FragranceExplorer-Front/src/pages/profile-page/components/ScentTasteChart.tsx` (+ `.module.scss`) — візуалізація смаків на базі бібліотеки `Recharts`.
* `FragranceExplorer-Front/src/pages/profile-page/components/FavoritePerfumes.tsx` (+ `.module.scss`) — управління збереженими парфумами.
* `FragranceExplorer-Front/src/pages/profile-page/components/EditProfileModal.tsx` (+ `.module.scss`) — редагування профілю та вибір сигнатурного аромату.
* `FragranceExplorer-Front/src/components/layout/header/Header.tsx` (+ `.module.scss`) — інтеграція кнопки профілю в навігаційну панель.
* `FragranceExplorer-Front/src/pages/perfume-detail-page/PerfumeDetailPage.tsx` — кнопка збереження аромату в гардероб (`Save to My Wardrobe`) та логування пошуку схожих композицій.

### Історія комітів у репозиторії:
1. `c4dce32` (*автор у git config ноутбука: `kozor`*) — `feat: implement user profile, guest mode, auth modal and taste analytics diagrams`
2. `8410368` (*автор у git config ноутбука: `kozor`*) — `feat(auth): add password and confirmation fields to registration and login with validation`
3. `83a95da` (*автор: `bloody2310`*) — `Merge pull request #20 from d10max/feature/user-profile-and-taste-analytics`
4. Нове оновлення (*автор: `bloody2310`*) — інтеграція бібліотек `recharts` та `lucide-react`, виправлення багів профілю/реєстрації та розширений звіт.
