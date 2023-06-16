using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

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
        public AccessForTestables Access { get; set; }
        public Comp_Answers Answer { get; set; }
        public int MarkAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime DateTimeAnswer { get; set; }

    }

    #endregion

    #region Кеттел

    public class KT_Main
    {
        [Key]
        public int ID { get; set; }
        public int Years { get; set; }
        public string Gender { get; set; }
        public DateTime DateTest { get; set; }
        public AccessForTestables Acces { get; set; }
    }

    public class KT_Questions
    {
        [Key]
        public int ID { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }
    }

    public class KT_Answers
    {
        [Key]
        public int ID { get; set; }
        public string AnswerNumber { get; set; }
        public string AnswerText { get; set; }
        public KT_Questions Question { get; set; }
    }

    public class KT_Categories
    {
        [Key]
        public int ID { get; set; }
        public string CategoryName { get; set; }
    }

    public class KT_Levels
    {
        [Key]
        public int ID { get; set; }
        public KT_Categories Category { get; set; }
        public int ScoreFrom { get; set; }
        public int ScoreTo { get; set; }
        public int Level { get; set; }
        public string LiterForLevel { get; set; }
        public string LiterForLevelWordInReport { get; set; }
    }

    public class KT_Key
    {
        [Key]
        public int ID { get; set; }
        public KT_Categories Category { get; set; }
        public KT_Answers Answer { get; set; }
        public int Score { get; set; }
    }

    public class TestableAnswers
    {
        [Key] 
        public int ID { get; set; }
        public KT_Main MainInfo { get; set; }
    }

    #endregion
}
