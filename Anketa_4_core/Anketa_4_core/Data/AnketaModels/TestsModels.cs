using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Anketa_4_core.Data.AnketaModels
{
    public class Test360
    {
        [Key]
        public int ID { get; set; }
        public DateTime DatetimeTest { get; set; }
        public AccessForTestables Access { get; set; }
        public RespondentRole Role { get; set; }
        public int Q01 { get; set; }
        public int Q02 { get; set; }
        public int Q03 { get; set; }
        public int Q04 { get; set; }
        public int Q05 { get; set; }
        public int Q06 { get; set; }
        public int Q07 { get; set; }
        public int Q08 { get; set; }
        public int Q09 { get; set; }
        public int Q10 { get; set; }
        public int Q11 { get; set; }
        public int Q12 { get; set; }
        public int Q13 { get; set; }
        public int Q14 { get; set; }
        public int Q15 { get; set; }
    }

    public class MotivationTest
    {
        [Key]
        public int ID { get; set; }
        public bool IsAgree { get; set; }
        public DateTime DatetimeTest { get; set; }
        public AccessForTestables Access { get; set; }
        public string RecomendPosition { get; set; }
        public string Recomendator { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string Q5 { get; set; }
        public string Q6 { get; set; }
        public string Q7 { get; set; }
        public string Q8 { get; set; }
        public string Q9 { get; set; }
        /// <summary>
        /// Вопрос ДА/НЕТ
        /// </summary>
        public string QYN { get; set; }
        public int M1 { get; set; }
        public int M2 { get; set; }
        public int M3 { get; set; }
        public int M4 { get; set; }
        public int M5 { get; set; }
        public int M6 { get; set; }
        public bool isChecked { get; set; }
    }


    public class RespondentRole
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [DefaultValue(false)]
        public bool isYouself { get; set; }
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
        public bool isCorrect { get; set; }
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

    public class KT_TestableAnswers
    {
        [Key] 
        public int ID { get; set; }
        public KT_Main MainInfo { get; set; }
        public KT_Answers Answer { get; set; }

    }

    #endregion
}
