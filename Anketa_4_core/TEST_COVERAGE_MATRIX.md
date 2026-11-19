# Матрица покрытия функций Anketa 4

Статусы:
- **AUTO** — вызывается автоматическим тестом напрямую или через публичный метод, покрывающий внутреннюю логику.
- **MANUAL** — для функции подготовлен конкретный сценарий в `TEST_SCENARIOS.md`; автоматического теста пока нет.
- **BLOCKED** — функция-заглушка или ожидаемое поведение не реализовано; есть отдельный тест/сценарий, который фиксирует блокер.

## Сервисы

| Класс | Функция | Статус | Тесты |
|---|---|---:|---|
| LKCabinetService | ParseRoot | AUTO | LKCabinetServiceTests |
| LKCabinetService | LoadForUserAsync | AUTO | LKCabinetServiceTests |
| LKCabinetService | LoadForTestableAsync | AUTO | LKCabinetServiceTests |
| LKCabinetService | SaveForUserAsync | AUTO | LKCabinetServiceTests |
| LKCabinetService | SaveForTestableAsync | AUTO | LKCabinetServiceTests |
| LKCabinetService | ApplyPlanDefaults | AUTO (через загрузку) | LKCabinetServiceTests |
| LKCabinetService | SaveAsync / блокировка и транзакция | AUTO | LKCabinetServiceTests |
| LKPeriodRatingService | GetForTestableAsync | AUTO | ResultAndRatingServiceTests |
| LKPeriodRatingService | выбор последнего назначения | AUTO (через рейтинг) | ResultAndRatingServiceTests |
| LKPeriodRatingService | расчёт прогресса ИПП | AUTO (через рейтинг) | ResultAndRatingServiceTests |
| LKPeriodRatingService | расчёт завершённости тестов | AUTO (через рейтинг) | ResultAndRatingServiceTests |
| LKPeriodRatingService | расчёт качества компетенций | AUTO (через рейтинг) | ResultAndRatingServiceTests |
| ResultModuleService | SearchAsync | AUTO | ResultAndRatingServiceTests |
| ResultModuleService | DetailAsync | AUTO | ResultAndRatingServiceTests |
| ResultModuleService | EnrichAsync / Fill / MatchStatus | AUTO (через поиск и детали) | ResultAndRatingServiceTests |
| TestableResultsService | LoadAsync | AUTO | TestableResultsAndReportTests |
| TestableResultsService | DeleteAsync | AUTO | TestableResultsAndReportTests |
| TestableResultsService | Show360 / ShowMotivation / ShowKettel / ShowCompetence | AUTO (через LoadAsync) | TestableResultsAndReportTests |
| ReportGenerator | Generate | AUTO | TestableResultsAndReportTests |
| ReportGenerator | EnsureCompetenceSheets | AUTO (через Generate) | TestableResultsAndReportTests |
| ReportGenerator | WriteIdentity / Write360 / WriteMotivation / WriteKettel / WriteCompetence | AUTO (через Generate) | TestableResultsAndReportTests |

## Модели и расчёты

| Класс / метод | Статус | Тесты |
|---|---:|---|
| JSON_ResultModel(string) — корректный JSON round-trip | AUTO | JsonResultModelTests |
| JSON_ResultModel(string) — повреждённый JSON и безопасные значения по умолчанию | BLOCKED | пропущенный тест KNOWN DEFECT |
| Add_JSON_Test360_ToReport_Testable | AUTO | JsonResultModelTests |
| Add_JSON_Test360_ToReport_Respondents | AUTO | JsonResultModelTests |
| SaveCompetentionQuestion | AUTO | JsonResultModelTests |
| CalculateCompetentionYear | AUTO | JsonResultModelTests |
| JSON_Kettel.RecordAnswer | AUTO | JsonResultModelTests |
| JSON_Kettel.Calculate / расчёт категорий | AUTO | JsonResultModelTests |
| JSON_Motivation_Result(MotivationTest) — валидные поля | AUTO | JsonResultModelTests |
| JSON_Motivation_Result(MotivationTest) — null в необязательных полях | BLOCKED | пропущенный тест KNOWN DEFECT |

## Контроллеры личного кабинета и тестов

| Контроллер | Функция / действие | Статус | Тесты |
|---|---|---:|---|
| PersonalCabinetController | Index | AUTO | PersonalCabinetControllerTests |
| PersonalCabinetController | Data | AUTO | PersonalCabinetControllerTests |
| PersonalCabinetController | SaveSection | AUTO | PersonalCabinetControllerTests |
| PersonalCabinetController | Rating | AUTO | PersonalCabinetControllerTests |
| Test360Controller | StartTest GET | AUTO | Test360ControllerTests |
| Test360Controller | StartTest POST | AUTO | Test360ControllerTests |
| Test360Controller | SubmitTest | AUTO | Test360ControllerTests |
| Test360Controller | CheckAccess / повторная самооценка | AUTO (через действия) | Test360ControllerTests |
| MotivationController | Index GET | AUTO | MotivationControllerTests |
| MotivationController | Index POST | MANUAL | MOT-02–MOT-04 |
| MotivationController | MotivationTest | AUTO | MotivationControllerTests |
| MotivationController | Finish | MANUAL | MOT-08, MOT-14 |
| MotivationController | CheckAccess / Error | AUTO (через действия) | MotivationControllerTests |
| KettelController | Index GET/POST | MANUAL | KET-01–KET-04 |
| KettelController | Start GET/POST | AUTO | KettelControllerTests |
| KettelController | Question GET/POST | AUTO | KettelControllerTests |
| KettelController | Finish | AUTO | KettelControllerTests |
| KettelController | CheckAccess / IsCompleted / LockAccess | AUTO (через действия) | KettelControllerTests |
| CompetentionController | Index GET/POST | AUTO (частично) | CompetentionControllerTests |
| CompetentionController | Question GET/POST | AUTO | CompetentionControllerTests |
| CompetentionController | Finish | AUTO (через действия) | CompetentionControllerTests |
| CompetentionController | CheckAccess / IsCompleted / GetCompletedQuestionIds | AUTO (через действия) | CompetentionControllerTests |

## Администрирование и модерация

| Контроллер | Функция / действие | Статус | Тесты |
|---|---|---:|---|
| RegistrationController | Index | MANUAL | REG-01–REG-25 |
| RegistrationController | Register | AUTO | RegistrationControllerTests |
| RegistrationController | ParseRows / ReadCsvAsync / ValidateRole / ValidatePasswords | AUTO (через Register) | RegistrationControllerTests |
| RegistrationController | ValidateFilials / ValidateExistingTestables / ValidateNewTestables | AUTO (частично) | RegistrationControllerTests |
| RegistrationController | RegisterAll / rollback | AUTO (сценарии успеха и ошибок) | RegistrationControllerTests |
| TestableController | Index | MANUAL | TEST-01 |
| TestableController | List | AUTO | TestableControllerTests |
| TestableController | Create GET/POST | BLOCKED | TestableControllerTests фиксирует заглушку |
| TestableController | NextCode / GetNextCodeAsync | AUTO | TestableControllerTests |
| TestableController | Import GET/POST | AUTO (POST) | TestableControllerTests |
| TestableController | GetFreeTestableUsersAsync | AUTO (через Import) | TestableControllerTests |
| TestableController | Edit GET/POST | AUTO | TestableControllerTests |
| TestableController | Details | AUTO | TestableControllerTests |
| TestableController | DeleteTestResults | MANUAL | TEST-13–TEST-16 |
| TestableController | LKData | AUTO | TestableControllerTests |
| TestableController | SaveLKSection | AUTO | TestableControllerTests |
| TestableController | GenerateReportPdf | BLOCKED | TestableControllerTests фиксирует NoContent |
| TestableController | GenerateReportXlsx | BLOCKED | TestableControllerTests фиксирует NoContent |
| TestableController | SaveLKData | BLOCKED | TestableControllerTests фиксирует NoContent |
| TestableController | Delete GET/POST | AUTO (POST) | TestableControllerTests |
| TestableController | FillLookups / UpsertTestableUser / UpsertAccessForTestable | AUTO (через Edit/Import) | TestableControllerTests |
| PeriodController | Index / Details | AUTO | ModeratorControllerTests |
| PeriodController | Create GET | MANUAL | PERIOD-04–PERIOD-08 |
| PeriodController | Create POST | AUTO | ModeratorControllerTests |
| PeriodController | Edit GET | MANUAL | PERIOD-09–PERIOD-10 |
| PeriodController | Edit POST | AUTO | ModeratorControllerTests |
| NewsAdminController | Index | AUTO | ModeratorControllerTests |
| NewsAdminController | Create GET | MANUAL | NEWS-01–NEWS-06 |
| NewsAdminController | Create POST | AUTO | ModeratorControllerTests |
| NewsAdminController | Edit GET/POST | AUTO (POST) | ModeratorControllerTests |
| NewsAdminController | Delete GET/POST | AUTO | ModeratorControllerTests |
| ResultController | IndexModerator | AUTO | ModeratorControllerTests |
| ResultController | SearchResult | AUTO | ModeratorControllerTests |
| ResultController | GetTests360 / GetMotivationTest / GetKettelTest / GetCompetentionTest | AUTO | ModeratorControllerTests |

## Прочие контроллеры и helpers

| Класс | Функция | Статус | Сценарии |
|---|---|---:|---|
| ReportController | GetReport GET/POST | AUTO | ReportControllerTests |
| ReportController | DownloadAsync / проверка шаблона / формирование файла | AUTO | ReportControllerTests |
| ReportController | GenerateReport | AUTO через ReportGenerator и HTTP-обвязку | ReportControllerTests, TestableResultsAndReportTests |
| AccountController | Login / Logout / AccessDenied | MANUAL | AUTH-01, SYS-13 |
| UsersController | Index / Edit / Delete | MANUAL | SYS-12 |
| DefaultValuesController | действия заполнения справочников | MANUAL | SYS-01–SYS-06, SYS-14 |
| ValidateController | все endpoints проверки готовности | MANUAL | SYS-06–SYS-09 |
| ValidateHelper | CodeExist / AccessExist / MotivationTestReadyTesting и др. | MANUAL | SYS-06–SYS-09 |
| ValidationsAttributes | UniqueCode и другие атрибуты | MANUAL | SYS-06 |
| AccountHelper | получение пользователя и текущего назначения | MANUAL | SYS-10 |
| AnketaDefaultValues | методы начального заполнения справочников | MANUAL | SYS-01–SYS-05 |
| HTML.DropDownLists | DDL_Filials / DDL_Reservs / DDL_RespondentRoles / DDL_TestPeriods | MANUAL | SYS-11 |
| HomeController | Index / Privacy / Error | MANUAL | общий smoke test |

## Границы покрытия

- AUTO означает, что метод действительно исполняется автоматическим тестом; приватные методы покрываются через публичные действия.
- MANUAL означает, что тест-кейс описан, но пока требует запуска в веб-приложении с реальными ролями и браузером.
- BLOCKED означает незавершённую или известную дефектную функцию; её нельзя считать принятой.
- Перед выпуском нужно довести все MANUAL сценарии до автоматизации или приложить протокол ручного прогона.
