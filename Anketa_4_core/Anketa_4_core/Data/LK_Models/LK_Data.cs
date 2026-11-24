using Anketa_4_core.Data.AnketaModels;
namespace Anketa_4_core.Data.LK_Models
{
    public class LK_Data
    {
        /// <summary>
        /// Список профессиональных задач
        /// </summary>
        public List<LK_ProfTask> ProfTasks { get; set; } = new();
        /// <summary>
        /// Стажировка
        /// </summary>
        public List<LK_Internship> Internships { get; set; } = new();
        /// <summary>
        /// Замещение начальника
        /// </summary>
        public List<LK_ViceBoss> LK_ViceBoss { get; set; } = new();
        /// <summary>
        /// Отчёты по прочитанным книгам (ранее класс существовал отдельно от корневого JSON)
        /// </summary>
        public List<LK_ReportBook> ReportBooks { get; set; } = new();
        /// <summary>
        /// Отчёты по тренинговым программам
        /// </summary>
        public List<LK_TrainingProgramm> TrainingProgramms { get; set; } = new();
        /// <summary>
        /// Карта развивающих действий
        /// </summary>
        public LK_DevelopmentalActionMap DevelopmentalActionMaproperty { get; set; } = new();

        /// <summary>Курсы повышения квалификации.</summary>
        public List<LK_AdvancedTrainingCourse> AdvancedTrainingCourses { get; set; } = new();
        /// <summary>Языковая подготовка.</summary>
        public List<LK_LanguageTraining> LanguageTrainings { get; set; } = new();
        /// <summary>Техническая учёба и преподавательская деятельность.</summary>
        public List<LK_TeachingActivity> TeachingActivities { get; set; } = new();
        /// <summary>Кураторство и наставничество.</summary>
        public List<LK_Mentorship> Mentorships { get; set; } = new();
        /// <summary>Дистанционные курсы, вебинары и практические кейсы.</summary>
        public List<LK_DistanceLearning> DistanceLearning { get; set; } = new();
        /// <summary>Профессиональная переподготовка.</summary>
        public List<LK_ProfessionalRetraining> ProfessionalRetrainings { get; set; } = new();
        /// <summary>Проектная деятельность.</summary>
        public List<LK_ProjectActivity> ProjectActivities { get; set; } = new();
        /// <summary>Конференции и конкурсы.</summary>
        public List<LK_ConferenceCompetition> ConferencesAndCompetitions { get; set; } = new();
        /// <summary>Психологическое консультирование. Не хранить содержание личных консультаций.</summary>
        public List<LK_PsychologicalConsultation> PsychologicalConsultations { get; set; } = new();
        /// <summary>Практическое задание «Мотивация персонала».</summary>
        public LK_PersonnelMotivation PersonnelMotivation { get; set; } = new();
    }

    /// <summary>
    /// Проессиональная задача
    /// </summary>
    public class LK_ProfTask
    {
        /// <summary>Номер задачи по ИПП.</summary>
        public int Number { get; set; }
        /// <summary>Название профессиональной задачи.</summary>
        public string ProfTaskName { get; set; } = string.Empty;
        public string CompetenceZone { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string AnalisEffect { get; set; } = string.Empty;
        public string WhatCouldBeDoneDifferently { get; set; } = string.Empty;
        public string Difficult { get; set; } = string.Empty;
        public string BossComment { get; set; } = string.Empty;
        public int Status { get; set; }
    }

    /// <summary>
    /// Стажировка
    /// </summary>
    public class LK_Internship
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<LK_InternshipTask> Tasks { get; set; } = new();
        public int Status { get; set; }
    }

    /// <summary>Строка отчётной формы №3: период, цель, мероприятия, отчёт и оценка руководителя.</summary>
    public class LK_InternshipTask
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Goal { get; set; } = string.Empty;
        public string Activities { get; set; } = string.Empty;
        public string Report { get; set; } = string.Empty;
        public string SupervisorAssessment { get; set; } = string.Empty;
    }
    /// <summary>
    /// Замещение
    /// </summary>
    public class LK_ViceBoss
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Position { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public List<LK_ViceBoss_Task> ViceBoss_Tasks { get; set; } = new();
        public string GeneralResult { get; set; } = string.Empty;
        public string SupervisorComment { get; set; } = string.Empty;
        public int Status { get; set; }
    }
    /// <summary>
    /// Задачи замещения
    /// </summary>
    public class LK_ViceBoss_Task
    {
        public string TaskName { get; set; } = string.Empty;
        public string ReportAboutTask { get; set; } = string.Empty;
        public string MarkAboutTask { get; set; } = string.Empty;
        public int Status { get; set; }
    }
    /// <summary>
    /// Отчёт по прочитанной книге
    /// </summary>
    public class LK_ReportBook
    {
        public string BookCategory { get; set; } = string.Empty;
        public string BookName { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        /// <summary>
        /// Ценность книги
        /// </summary>              
        public string Gold { get; set; }
        /// <summary>
        /// Практические приёмы
        /// </summary>           
        public string PractiсeSkills { get; set; }
        public int Status { get; set; }
    }
    /// <summary>
    /// Треннинговая программа
    /// </summary>
    public class LK_TrainingProgramm
    {
        public string Theme { get; set; } = string.Empty;
        public string Organizer { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Format { get; set; } = string.Empty;
        public int DurationHours { get; set; }
        public string LiveSityations { get; set; } = string.Empty;
        public string Difficult { get; set; } = string.Empty;
        public string InfluenceAsWorker { get; set; } = string.Empty;
        public string InfluenceAsFilial { get; set; } = string.Empty;
        public int Status { get; set; }
    }

    #region Карта развивающих действий
    /// <summary>
    /// Карта развивающих действий
    /// </summary>
    public class LK_DevelopmentalActionMap
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        /// <summary>
        /// Профессиональное развитие
        /// </summary>
        public List<LK_Map_ProfessionalDevelopment> ProfessionalDevelopment { get; set; }
        /// <summary>
        /// Профессиональное развитие. Таблица 19
        /// </summary>
        public List<LK_Map_ProfessionalDevelopment_T19> ProfessionalDevelopment_T19 { get; set; }
        /// <summary>
        /// Профессиональное развитие. Таблица 20
        /// </summary>
        public List<LK_Map_ProfessionalDevelopment_T20> ProfessionalDevelopment_T20 { get; set; }
        /// <summary>
        /// Ориентация на результат. Таблица 9
        /// </summary>
        public List<LK_Map_ResultOrientation_T09> ResultOrientation_T09 { get; set; }
        /// <summary>
        /// Ориентация на результат. Таблица 10
        /// </summary>
        public List<LK_Map_ResultOrientation_T10> ResultOrientation_T10 { get; set; }
        /// <summary>
        /// Ориентация на результат. Таблица 11
        /// </summary>
        public List<LK_Map_ResultOrientation_T11> LK_Map_ResultOrientation_T11 { get; set; }
        /// <summary>Планируемые действия по развитию компетенций КРД; пока не связаны автоматически с тестом компетенций.</summary>
        public List<LK_Map_CompetenceDevelopment> CompetenceDevelopment { get; set; } = new();
    }

    /// <summary>
    /// Профессиональное развитие
    /// </summary>
    public class LK_Map_ProfessionalDevelopment
    {
        /// <summary>
        /// Комментарий к таблице 19
        /// </summary>
        public string CommentToT19 { get; set; }
        /// <summary>
        /// Рекоемндация к таблице 19
        /// </summary>
        public string RecomendationToT19 { get; set; }
        /// <summary>
        /// Результат беседы к таблице 19
        /// </summary>
        public string DialogResultToT19 { get; set; }
    }

    /// <summary>
    /// Профессиональное развитие. Таблица 19
    /// </summary>
    public class LK_Map_ProfessionalDevelopment_T19
    {
        public string Header { get; set; }
        public string Description { get; set; }
    }
    /// <summary>
    /// Профессиональное развитие. Таблица 20
    /// </summary>
    public class LK_Map_ProfessionalDevelopment_T20
    {
        /// <summary>
        /// Тема обучающего мероприятия
        /// </summary>
        public string Theme { get; set; }
        /// <summary>
        /// Формат обучающего мероприятия
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// Дата проведения мероприятия
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Обратная связь участников мероприятия. Интерес
        /// </summary>
        public string Interes { get; set; }
        /// <summary>
        /// Обратная связь участников мероприятия. Практическая полезность
        /// </summary>
        public string PracticalUse { get; set; }
        /// <summary>
        /// Обратная связь участников мероприятия. Качество преподнесения информации
        /// </summary>
        public string Quality { get; set; }
        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Рекомендация
        /// </summary>
        public string Recomendation { get; set; }

        /// <summary>
        /// Вывод. Что было эффективным?
        /// </summary>
        public string Effect { get; set; }
        /// <summary>
        /// Вывод. Что можно было сделать лучше
        /// </summary>
        public string WhatBetter { get; set; }
    }
    /// <summary>
    /// Ориентация на результат. Таблица 9
    /// </summary>
    public class LK_Map_ResultOrientation_T09
    {
        /// <summary>
        /// Вид деятельности поддразделея
        /// </summary>
        public string TypeOfActivity { get; set; }
        /// <summary>
        /// Процессы
        /// </summary>
        public string Processes { get; set; }
        /// <summary>
        /// Существующие регламентирующие документы
        /// </summary>
        public string OrderDocument { get; set; }
        /// <summary>
        /// Необходимые корректировки в существующих регламентирующих документах
        /// </summary>
        public string AdjustmentOfOrderDocument { get; set; }
        /// <summary>
        /// Недоступные регламентирующих документах
        /// </summary>
        public string MissindOrderDocument { get; set; }
        /// <summary>
        /// Комментарии, рекомендации. предложения непосредственного руководителя
        /// </summary>
        public string BossRecomendation { get; set; }
    }
    /// <summary>
    /// Ориентация на результат. Таблица 10
    /// </summary>
    public class LK_Map_ResultOrientation_T10
    {
        /// <summary>
        /// Процессы / старые документы
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Нерегламентированные процессы/виды деятельности подразделения/ Устаревшие документы
        /// </summary>
        public string NonOrderedProcess_OldDocument { get; set; }
        /// <summary>
        /// Поэтапный план разработки регламентирующих документов. Задачи
        /// </summary>
        public string DevelopmentPlanOf_Order_Old_Document_Task { get; set; }
        /// <summary>
        /// Поэтапный план разработки регламентирующих документов. Сроки
        /// </summary>
        public string DevelopmentPlanOfOrder_Old_Document_Date { get; set; }
        /// <summary>
        /// Отметка о выполнении
        /// </summary>
        public string MarkOfComplete { get; set; }
    }
    /// <summary>
    /// Ориентация на результат. Таблица 11
    /// </summary>
    public class LK_Map_ResultOrientation_T11
    {
        /// <summary>
        /// Задача
        /// </summary>
        public string Task { get; set; }
        /// <summary>
        /// Планируемый результат
        /// </summary>
        public string PlanResult_S_M_A_R_T { get; set; }
        /// <summary>
        /// Фактический результат
        /// </summary>
        public string FactResult { get; set; }
        /// <summary>
        /// Отклонения от фактического результата
        /// </summary>
        public string DeviationOfFactResult { get; set; }
        /// <summary>
        /// Причины отклонений
        /// </summary>
        public string DeviationReason { get; set; }
        /// <summary>
        /// Меры по предотвращению отклонений    
        /// </summary>
        public string MeasuresOfDeviationOfFactResult { get; set; }
        /// <summary>
        /// Обратная связь Наставника
        /// </summary>
        public string FeedBack { get; set; }
        /// <summary>
        /// Выводы
        /// </summary>
        public string Conclusion { get; set; }
    }
    #endregion

    /// <summary>Курс повышения квалификации.</summary>
    public class LK_AdvancedTrainingCourse
    {
        public string ProgramName { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int DurationHours { get; set; }
        public string AcquiredSkills { get; set; } = string.Empty;
        public string ApplicationInWork { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string Certificate { get; set; } = string.Empty;
        public int Status { get; set; }
    }

    /// <summary>Языковая подготовка.</summary>
    public class LK_LanguageTraining
    {
        public string Language { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string CurrentLevel { get; set; } = string.Empty;
        public string TargetLevel { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public int Status { get; set; }
    }

    /// <summary>Техническая учёба и преподавательская деятельность.</summary>
    public class LK_TeachingActivity
    {
        public string ActivityType { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int DurationHours { get; set; }
        public string PracticalUse { get; set; } = string.Empty;
        public string Effect { get; set; } = string.Empty;
        public string WhatBetter { get; set; } = string.Empty;
        public string WhatHelped { get; set; } = string.Empty;
        public string WhatHindered { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public int Status { get; set; }
    }

    /// <summary>Кураторство и наставничество.</summary>
    public class LK_Mentorship
    {
        public string MenteePosition { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Goal { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Activities { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string SupervisorFeedback { get; set; } = string.Empty;
        public int Status { get; set; }
    }

    /// <summary>Дистанционные курсы, вебинары и практические кейсы.</summary>
    public class LK_DistanceLearning
    {
        public bool IsPlanned { get; set; }
        public string ActivityType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int DurationHours { get; set; }
        public string MainIdeas { get; set; } = string.Empty;
        public string ApplicationInWork { get; set; } = string.Empty;
        public string Certificate { get; set; } = string.Empty;
        public int Status { get; set; }
    }

    /// <summary>Профессиональная переподготовка.</summary>
    public class LK_ProfessionalRetraining
    {
        public string ProgramName { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Qualification { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string Certificate { get; set; } = string.Empty;
        public int Status { get; set; }
    }

    /// <summary>Проектная деятельность.</summary>
    public class LK_ProjectActivity
    {
        public string ProjectName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Goal { get; set; } = string.Empty;
        public string Tasks { get; set; } = string.Empty;
        public string PracticalUse { get; set; } = string.Empty;
        public string Effect { get; set; } = string.Empty;
        public string WhatBetter { get; set; } = string.Empty;
        public string WhatHelped { get; set; } = string.Empty;
        public string WhatHindered { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string Difficulties { get; set; } = string.Empty;
        public int Status { get; set; }
    }

    /// <summary>Участие в конференциях и конкурсах.</summary>
    public class LK_ConferenceCompetition
    {
        public string EventType { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public string Organizer { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Role { get; set; } = string.Empty;
        public string PracticalUse { get; set; } = string.Empty;
        public string Effect { get; set; } = string.Empty;
        public string WhatBetter { get; set; } = string.Empty;
        public string WhatHelped { get; set; } = string.Empty;
        public string WhatHindered { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public int Status { get; set; }
    }

    /// <summary>
    /// Учёт участия в психологическом консультировании.
    /// Содержимое консультаций и личные сведения в личном кабинете не сохраняются.
    /// </summary>
    public class LK_PsychologicalConsultation
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int DurationMinutes { get; set; }
        public string Format { get; set; } = string.Empty;
        public string GeneralResult { get; set; } = string.Empty;
        public int Status { get; set; }
    }

    /// <summary>Пять характеристик работы и по два конкретных действия для каждой.</summary>
    public class LK_PersonnelMotivation
    {
        public string WorkSignificanceAction1 { get; set; } = string.Empty;
        public string WorkSignificanceAction2 { get; set; } = string.Empty;
        public string WorkVarietyAction1 { get; set; } = string.Empty;
        public string WorkVarietyAction2 { get; set; } = string.Empty;
        public string WorkIdentityAction1 { get; set; } = string.Empty;
        public string WorkIdentityAction2 { get; set; } = string.Empty;
        public string WorkAutonomyAction1 { get; set; } = string.Empty;
        public string WorkAutonomyAction2 { get; set; } = string.Empty;
        public string FeedbackAction1 { get; set; } = string.Empty;
        public string FeedbackAction2 { get; set; } = string.Empty;
        public string AdditionalComment { get; set; } = string.Empty;
    }

    /// <summary>План развития одной компетенции в КРД.</summary>
    public class LK_Map_CompetenceDevelopment
    {
        public string CompetenceName { get; set; } = string.Empty;
        public string CurrentLevel { get; set; } = string.Empty;
        public string TargetLevel { get; set; } = string.Empty;
        public string DevelopmentActions { get; set; } = string.Empty;
        public DateTime Deadline { get; set; }
        public string ExpectedResult { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
