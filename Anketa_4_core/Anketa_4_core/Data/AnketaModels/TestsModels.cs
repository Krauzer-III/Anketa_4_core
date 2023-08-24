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
        public AccessForTestable AccessTest { get; set; }
        public RespondentRole Role { get; set; }
        public bool IsSelfMark { get; set; }
        public string results { get; set; }
    }

    public class MotivationTest
    {
        [Key]
        public int ID { get; set; }
        public bool IsAgree { get; set; }
        public DateTime DatetimeTest { get; set; }
        public AccessForTestable AccessTest { get; set; }
        public string Results { get; set; }
    }


    public class RespondentRole
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [DefaultValue(false)]
        public bool isSelfMark { get; set; }
    }

    #region Компетенции

    public class Comp_Block
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string BlockName { get; set; }
        public string WordInReport { get; set; }
        public int Year { get; set; }

    }

    public class Comp_Question
    {
        [Key]
        public int ID { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public Comp_Block Block { get; set; }
    }

    public class Comp_Answer
    {
        [Key]
        public int ID { get; set; }
        public Comp_Question Question { get; set; }
        public string Text { get; set; }
        public string Number { get; set; }
        public int Correct_Answer { get; set; }
    }

    public class Comp_TestableAnswer
    {
        [Key]
        public int ID { get; set; }
        public AccessForTestable Access { get; set; }
        public Comp_Answer Answer { get; set; }
        public int Mark { get; set; }
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
        public AccessForTestable Access { get; set; }
    }

    public class KT_Question
    {
        [Key]
        public int ID { get; set; }
        public int Number { get; set; }
        public string Text { get; set; }
    }

    public class KT_Answer
    {
        [Key]
        public int ID { get; set; }
        public string AnswerNumber { get; set; }
        public string AnswerText { get; set; }
        public KT_Question Question { get; set; }
    }

    public class KT_Category
    {
        [Key]
        public int ID { get; set; }
        public string CategoryName { get; set; }
    }

    public class KT_Mark
    {
        [Key]
        public int ID { get; set; }
        public KT_Category Category { get; set; }
        public int ScoreFrom { get; set; }
        public int ScoreTo { get; set; }
        public int Mark { get; set; }
        public string WordInReport { get; set; }
        public string LevelWord { get; set; }
        public string LevelWordInReport { get; set; }
    }

    public class KT_KeyForCategory
    {
        [Key]
        public int ID { get; set; }
        public KT_Category Category { get; set; }
        public KT_Answer Answer { get; set; }
        public int Score { get; set; }
    }

    public class KT_TestableAnswers
    {
        [Key] 
        public int ID { get; set; }
        public KT_Main MainInfo { get; set; }
        public KT_Answer Answer { get; set; }
        public DateTime DateTimeAnswer { get; set; }


    }

    #endregion


    public class TestResult
    {
        [Key]
        public int ID { get; set; }
        public AccessForTestable Access { get; set; }
        public string Results { get; set; }
    }



    #region JSON classes

    public class Test360_Result
    {
        public int q01 { get; set; }
        public int q02 { get; set; }
        public int q03 { get; set; }
        public int q04 { get; set; }
        public int q05 { get; set; }
        public int q06 { get; set; }
        public int q07 { get; set; }
        public int q08 { get; set; }
        public int q09 { get; set; }
        public int q10 { get; set; }
        public int q11 { get; set; }
        public int q12 { get; set; }
        public int q13 { get; set; }
        public int q14 { get; set; }
        public int q15 { get; set; }
    }

    public class Motivation_Result
    {
      public string RecomendPosition {get; set;}
      public string Recomendator {get; set;}
      public string Question_01 {get; set;}
      public string Question_02 {get; set;}
      public string Question_03 {get; set;}
      public string Question_04 {get; set;}
      public string Question_05 {get; set;}
      public string Question_06 {get; set;}
      public string Question_07 {get; set;}
      public string Question_08 {get; set;}
      public string Question_09 {get; set;}
      public string Question_YES_NO {get; set;}
    }

    #endregion
}
