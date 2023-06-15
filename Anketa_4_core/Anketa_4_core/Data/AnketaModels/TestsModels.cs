using System.ComponentModel.DataAnnotations;

namespace Anketa_4_core.Data.AnketaModels
{
    /// <summary>
    /// Тип теста: 360градусов, Кеттел, Мотивация, Мотивация 2
    /// </summary>
    public class TestType
    {
        [Key]
        public int Id { get; set; }
        public string TypeName { get; set; }
    }

    public class RespondentRole
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
    }

    #region Компетенции

    public class Comp_Blocks
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string BlockName { get; set; }
        public string WordInReport { get; set; }
    }

    public class Comp_Questions
    {
        [Key]
        public int ID { get; set; }
        public int YearTraining { get; set; }
        public int QuestionNumber { get; set; }               
        public string QuestionText { get; set; }
    }

    public class Comp_Answers
    {
        [Key]
        public int ID { get; set; }
        public string AnswerText { get; set; }
        public string AnswerNumber { get; set; }
        public int AnswerKey { get; set; }
    }

    public class Comp_Level
    {
        [Key]
        public int ID { get; set; }
        public string LevelName { get; set; }
        public int BallFrom { get; set; }
        public int BallTo { get; set; }
        public int LevelNumber { get; set; }
        public int Year { get; set; }
    }

    public class Comp_TestableAnswers
    {
        [Key]
        public int ID { get; set; }
        public AccessForTestables AccessID { get; set; }
        public Comp_Answers AnswerID { get; set; }
        public int MarkAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime DateTimeAnswer { get; set; }

    }

    #endregion

    #region Кеттел

    public class KettelMain
    {
        [Key]
        public int ID { get; set; }

    }

    #endregion
}
