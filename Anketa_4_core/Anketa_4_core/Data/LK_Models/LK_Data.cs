using Anketa_4_core.Data.AnketaModels;
namespace Anketa_4_core.Data.LK_Models
{
    public class LK_Data
    {
        /// <summary>
        /// Список профессиональных задач
        /// </summary>
        public List<LK_ProfTask> ProfTasks { get; set; }
        /// <summary>
        /// Стажировка
        /// </summary>
        public List<LK_Internship> Internships { get; set; }
        /// <summary>
        /// Замещение начальника
        /// </summary>
        public List<LK_ViceBoss> LK_ViceBoss { get; set; }
        /// <summary>
        /// Карта развивающих действий
        /// </summary>
        public LK_DevelopmentalActionMap DevelopmentalActionMaproperty { get; set; }
    }

    /// <summary>
    /// Проессиональная задача
    /// </summary>
    public class LK_ProfTask
    {
        /// <summary>
        /// Номер задачи
        /// </summary>                         
        public int Number { get; set; }
        /// <summary>
        /// Название задачи
        /// </summary>                              
        public string ProfTaskName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string AnalisEffect { get; set; }
        public string Difficult { get; set; }
        public string BossComment { get; set; }
        public int Status { get; set; }
    }

    /// <summary>
    /// Стажировка
    /// </summary>
    public class LK_Internship
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int Status { get; set; }
    }
    /// <summary>
    /// Замещение
    /// </summary>
    public class LK_ViceBoss
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Target { get; set; }
        /// <summary>
        /// Задачи замещения
        /// </summary>
        public List<LK_ViceBoss_Task> ViceBoss_Tasks { get; set; }
    }
    /// <summary>
    /// Задачи замещения
    /// </summary>
    public class LK_ViceBoss_Task
    {
        public string TaskName { get; set; }
        public string ReportAboutTask { get; set; }
        public string MarkAboutTask { get; set; }
        public int Status { get; set; }
    }
    /// <summary>
    /// Отчёт по прочитанной книге
    /// </summary>
    public class LK_ReportBook
    {
        public string BookName { get; set; }
        public string Author { get; set; }
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
        /// <summary>
        /// Что из предложенного на тренинге материала Вы применили на практике? Приведите примеры данных ситуаций.
        /// </summary>
        public string LiveSityations { get; set; }
        /// <summary>
        /// Какие сложности возникали в ходе применения нового материала?
        /// </summary>
        public string Difficult { get; set; }
        /// <summary>
        /// Какое влияние оказало участие в тренинге на ваши результаты деятельности
        /// </summary>
        public string InfluenceAsWorker { get; set; }
        /// <summary>
        /// Какое влияние оказало участие в тренинге на результаты деятельности вашего филиала
        /// </summary>
        public string InfluenceAsFilial { get; set; }
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
}
