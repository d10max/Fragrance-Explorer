# FragranceExplorer.Seeder

Консольний інструмент для початкового наповнення (сідингу) бази даних PostgreSQL проєкту **Fragrance Explorer** даними парфумерних домів (`house_catalog`) та парфумів (`perfumes_actual`).

---

## Що робить Seeder

1. **Автоматично застосовує міграції EF Core** (`Database.MigrateAsync()`) — створює базу даних `fragrance_explorer` та всі таблиці, якщо їх ще немає.
2. **Автоматично знаходить датасети** у `FragranceExplorer.BLL/Parser/` (якщо `.jsonl`-файли відсутні, автоматично розпаковує відповідні `.zip`-архіви).
3. **Сідить парфумерні доми (`Brands`)** потоком батчами по `BatchSize` (за замовчуванням `1000`) та індексує їх у пам'яті за нормалізованими `Name`, `DisplayName`, `Slug` та `BrandAliases`.
4. **Сідить довідники (`Notes`, `Accords`) та парфуми (`Perfumes`, `PerfumeNotes`, `PerfumeAccords`)** у двофазному потоковому режимі:
   - **Фаза А:** зберігає нові ноти, акорди та дефолтні бренди (якщо парфум посилається на бренд, відсутній у `house_catalog`).
   - **Фаза Б:** зберігає парфуми та їхні зв'язки з нотами й акордами без зайвого навантаження на оперативну пам'ять (`AutoDetectChangesEnabled = false` + `ChangeTracker.Clear()`).
5. **Виводить детальну статистику** виконання (`DATABASE SEEDING SUMMARY`).

---

## Початкове налаштування (один раз на новому ПК)

Проєкти `FragranceExplorer.WebAPI` та `FragranceExplorer.Seeder` використовують спільний `UserSecretsId`. Щоб задати рядок підключення до локального PostgreSQL, виконай з папки `FragranceExplorer-Back`:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=fragrance_explorer;Username=postgres;Password=postgres" --project FragranceExplorer.Seeder
```
*(Якщо твій локальний PostgreSQL має інший пароль або порт — підстав свої значення).*

---

## Команди запуску

Усі команди виконуються з папки **`FragranceExplorer-Back`**:

### 1. Стандартний запуск (ідемпотентний сід)
```powershell
dotnet run --project FragranceExplorer.Seeder
```
- **Що робить:** перевіряє, чи є дані в таблицях `Brands` та `Perfumes`. Якщо таблиці порожні — накатує міграції та засіває всі доми й парфуми. Якщо дані вже є — пропускає сід і нічого не дублює.

### 2. Повний перезапуск з нуля (`--reseed`)
```powershell
dotnet run --project FragranceExplorer.Seeder -- --reseed
```
- **Що робить:** виконує швидке очищення всіх таблиць (`TRUNCATE TABLE ... RESTART IDENTITY CASCADE`), скидає лічильники `Id` і заново заливає весь каталог домів, нот, акордів та парфумів.

### 3. Сід тільки парфумерних домів (`--houses-only`)
```powershell
dotnet run --project FragranceExplorer.Seeder -- --houses-only
```
- **Що робить:** засіває виключно таблицю `Brands` із файлу `house_catalog.jsonl`, пропускаючи парфуми. Можна комбінувати з `--reseed` (`dotnet run --project FragranceExplorer.Seeder -- --reseed --houses-only`).

### 4. Сід тільки парфумів, нот і акордів (`--perfumes-only`)
```powershell
dotnet run --project FragranceExplorer.Seeder -- --perfumes-only
```
- **Що робить:** підвантажує вже наявні в БД бренди в кеш і засіває тільки парфуми (`Perfumes`), ноти (`Notes`, `PerfumeNotes`) та акорди (`Accords`, `PerfumeAccords`) із файлу `perfumes_actual.jsonl`.

---

## Конфігурація (`appsettings.json`)

Налаштування сідера знаходяться у файлі `FragranceExplorer.Seeder/appsettings.json`:

```json
{
  "Seeder": {
    "DatasetsPath": "../FragranceExplorer.BLL/Parser",
    "HousesFile": "house_catalog.jsonl",
    "PerfumesFile": "perfumes_actual.jsonl",
    "BatchSize": 1000
  }
}
```

- **`DatasetsPath`** — шлях до папки з датасетами.
- **`HousesFile`** — назва файлу датасету парфумерних домів.
- **`PerfumesFile`** — назва файлу датасету парфумів.
- **`BatchSize`** — кількість записів, що зберігаються за одну транзакцію `SaveChangesAsync()`.
