using System.Text.Json;
using System.Linq;
using System.Drawing;
using static Anketa_4_core.Data.AnketaModels.JSON_Motivation_Result;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace Anketa_4_core.Data.AnketaModels
{

    public class JSON_ResultModel
    {
        public string TestableCode { get; set; }
        public int AccessID { get; set; }
        public List<JSON_Test360> Tests360 { get; set; }
        public JSON_Test360_ToReport Test360ForReportTestable { get; set; }
        public JSON_Test360_ToReport Test360ForReportRespondents { get; set; }
        public JSON_Motivation_Result MotivationTest { get; set; }
        public JSON_Kettel KettelTest { get; set; }
        public List<JSON_Competention_Answers> CompetentionAnswers { get; set; }
        public List<JSON_Competention_Result> CompetentionResult { get; set; }


        /// <summary>
        /// Поиск результата по айдишнику
        /// </summary>
        /// <param name="accesID">ИД доступа к тестам</param>
        public JSON_ResultModel(int accesID)
        {
            using (var context = new AnketaContext())
            {
                var tr = context.TestResults.Include(aft => aft.Access).FirstOrDefault(aft => aft.Access.ID == accesID);
                if (tr == null)
                {
                    this.AccessID = accesID;
                    this.TestableCode = context.AccessForTestables.Include(t => t.Testable).First(aft => aft.ID == accesID).Testable.Code;
                    _JSON_ResultModel();
                }
                else
                {
                    //TODO доделать
                }
            }
        }

        public JSON_ResultModel(string JSONstring)
        {
            try
            {
                var @this = JsonSerializer.Deserialize<JSON_ResultModel>(JSONstring);
                if (@this != null)
                {
                    this.TestableCode = @this.TestableCode;
                    this.AccessID = @this.AccessID;
                    this.Tests360 = @this.Tests360;
                    this.Test360ForReportTestable = @this.Test360ForReportTestable;
                    this.Test360ForReportRespondents = @this.Test360ForReportRespondents;
                    this.MotivationTest = @this.MotivationTest;
                    this.KettelTest = @this.KettelTest;
                    this.CompetentionAnswers = @this.CompetentionAnswers;
                    this.CompetentionResult = @this.CompetentionResult;
                }
                else throw new Exception("Невозможно распознать строку результатов");

            }
            catch (Exception ex)
            {
                //TODO Журналировать
            }

        }

        /// <summary>
        /// Заполнялка пустых значений
        /// </summary>
        private void _JSON_ResultModel()
        {
            this.Tests360 = new List<JSON_Test360>();
            this.Test360ForReportTestable = new JSON_Test360_ToReport();
            this.Test360ForReportRespondents = new JSON_Test360_ToReport();
            this.MotivationTest = new JSON_Motivation_Result();
            this.KettelTest = new JSON_Kettel();
            this.CompetentionAnswers = new List<JSON_Competention_Answers>();
            this.CompetentionResult = new List<JSON_Competention_Result>();
        }

        /// <summary>
        /// Заполнялка пустых значений
        /// </summary>
        private void _JSON_ResultModel(TestResult @this, int accessID, string code)
        {
            //TODO доделать
            this.Tests360 = new List<JSON_Test360>();
            this.Test360ForReportTestable = new JSON_Test360_ToReport();
            this.Test360ForReportRespondents = new JSON_Test360_ToReport();
            this.MotivationTest = new JSON_Motivation_Result();
            this.KettelTest = new JSON_Kettel();
            this.CompetentionAnswers = new List<JSON_Competention_Answers>();
            this.CompetentionResult = new List<JSON_Competention_Result>();
        }



        public void Add_JSON_Test360_ToReport_Respondents(JSON_Test360 t360)
        {
            Tests360.Add(t360);
            var items = Tests360.Where(e => !e.IsSelfMark);
            Test360ForReportRespondents = new JSON_Test360_ToReport
            {
                q01 = items.Sum(e => e.q01) / items.Count(),
                q02 = items.Sum(e => e.q02) / items.Count(),
                q03 = items.Sum(e => e.q03) / items.Count(),
                q04 = items.Sum(e => e.q04) / items.Count(),
                q05 = items.Sum(e => e.q05) / items.Count(),
                q06 = items.Sum(e => e.q06) / items.Count(),
                q07 = items.Sum(e => e.q07) / items.Count(),
                q08 = items.Sum(e => e.q08) / items.Count(),
                q09 = items.Sum(e => e.q09) / items.Count(),
                q10 = items.Sum(e => e.q10) / items.Count(),
                q11 = items.Sum(e => e.q11) / items.Count(),
                q12 = items.Sum(e => e.q12) / items.Count(),
                q13 = items.Sum(e => e.q13) / items.Count(),
                q14 = items.Sum(e => e.q14) / items.Count(),
                q15 = items.Sum(e => e.q15) / items.Count()
            };
        }

        public void Add_JSON_Kettel(KT_Main main, KT_TestableAnswers[] answers)
        {
            KettelTest = new JSON_Kettel(main, answers);
        }

        public string GetJSON() => JsonSerializer.Serialize(this);

        /// <summary>
        /// Добавить результат тестирования компетенций
        /// </summary>
        /// <param name="ta">Ответы, которые давал тестируемый</param>
        public void Add_JSON_Competention(Comp_TestableAnswer[] ta)
        {
            CompetentionAnswers = new List<JSON_Competention_Answers>();
            CompetentionResult = new List<JSON_Competention_Result>();

            foreach (var a in ta.Select(x => x.Answer.Question).OrderBy(q => q.QuestionNumber))
            {
                CompetentionAnswers.Add(new JSON_Competention_Answers
                {
                    QuestionNumber = a.QuestionNumber,
                    QuestionText = a.QuestionText,
                    Answers = ta.Where(x => x.Answer.Question.QuestionNumber == a.QuestionNumber)
                    .OrderBy(x => x.Answer.Number)
                    .Select(x => new JSON_Competention_AnswerResult
                    {
                        AnswerNumber = x.Answer.Number,
                        AnswerText = x.Answer.Text,
                        AnswerMark = x.Mark
                    }).ToArray()
                });
            }
            using (var context = new AnketaContext())
            {
                var blocks = context.Comp_Blocks.Where(b => b.Year == ta[0].Answer.Question.Block.Year);
                var list_correctAnswers_all = context.Comp_Answers.Include(x => x.Question.Block);
                foreach (var block in blocks)
                {
                    Task.Factory.StartNew(() =>
                    {
                        int summ = 0;

                        lock (ta)
                        {
                            lock (list_correctAnswers_all)
                            {
                                var list_correctAnswers = list_correctAnswers_all.Where(x => x.Question.Block.ID == block.ID);
                                foreach (var ans in ta.Where(x => x.Answer.Question.Block.ID == block.ID))
                                    if (ans.Mark == list_correctAnswers.First(x => x.ID == ans.Answer.ID).Correct_Answer)
                                        summ++;
                            }
                        }
                        lock (CompetentionResult)
                        {
                            CompetentionResult.Add(new JSON_Competention_Result
                            {
                                Block = block.BlockName,
                                BlockWordInReport = block.WordInReport,
                                Ball = summ
                            });
                        }
                    });
                    Task.WaitAll();
                }
            }
        }

    }

    public class JSON_Test360
    {
        public int ID { get; set; }
        public int RespondentRoleID { get; set; }
        public string RespondentRoleName { get; set; }
        public bool IsSelfMark { get; set; }
        public DateTime DatetimeTest { get; set; }
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


        public JSON_Test360(Test360 t360, RespondentRole rr)
        {
            ID = t360.ID;
            RespondentRoleID = rr.ID;
            RespondentRoleName = rr.Name;
            IsSelfMark = t360.IsSelfMark;
            DatetimeTest = t360.DatetimeTest;
            Test360_Result? _Result = JsonSerializer.Deserialize<Test360_Result>(t360.results);
            q01 = _Result.q01;
            q02 = _Result.q02;
            q03 = _Result.q03;
            q04 = _Result.q04;
            q05 = _Result.q05;
            q06 = _Result.q06;
            q07 = _Result.q07;
            q08 = _Result.q08;
            q09 = _Result.q09;
            q10 = _Result.q10;
            q11 = _Result.q11;
            q12 = _Result.q12;
            q13 = _Result.q13;
            q14 = _Result.q14;
            q15 = _Result.q15;
        }

    }

    public class JSON_Test360_ToReport
    {

        public double q01 { get; set; }
        public double q02 { get; set; }
        public double q03 { get; set; }
        public double q04 { get; set; }
        public double q05 { get; set; }
        public double q06 { get; set; }
        public double q07 { get; set; }
        public double q08 { get; set; }
        public double q09 { get; set; }
        public double q10 { get; set; }
        public double q11 { get; set; }
        public double q12 { get; set; }
        public double q13 { get; set; }
        public double q14 { get; set; }
        public double q15 { get; set; }

    }

    public class JSON_Motivation_Result
    {
        public int ID { get; set; }
        public bool IsAgree { get; set; }
        public DateTime DatetimeTest { get; set; }
        public string RecomendPosition { get; set; }
        public string Recomendator { get; set; }
        public string Question_01 { get; set; }
        public string Question_02 { get; set; }
        public string Question_03 { get; set; }
        public string Question_04 { get; set; }
        public string Question_05 { get; set; }
        public string Question_06 { get; set; }
        public string Question_07 { get; set; }
        public string Question_08 { get; set; }
        public string Question_09 { get; set; }
        public string Question_YES_NO { get; set; }
        public int Mark_01 { get; set; }
        public int Mark_02 { get; set; }
        public int Mark_03 { get; set; }
        public int Mark_04 { get; set; }
        public int Mark_05 { get; set; }
        public int Mark_06 { get; set; }
        public int SummBall { get; set; }

        public JSON_Motivation_Result() { }

        public JSON_Motivation_Result(MotivationTest m)
        {
            ID = m.ID;
            IsAgree = m.IsAgree;
            DatetimeTest = m.DatetimeTest;
            Motivation_Result? mr = JsonSerializer.Deserialize<Motivation_Result>(m.Results);
            RecomendPosition = mr.RecomendPosition;
            Recomendator = mr.Recomendator;
            Question_01 = mr.Question_01;
            Question_02 = mr.Question_02;
            Question_03 = mr.Question_03;
            Question_04 = mr.Question_04;
            Question_05 = mr.Question_05;
            Question_06 = mr.Question_06;
            Question_07 = mr.Question_07;
            Question_08 = mr.Question_08;
            Question_09 = mr.Question_09;
            Question_YES_NO = mr.Question_YES_NO;

            Mark_01 = 0;
            Mark_02 = 0;
            Mark_03 = 0;
            Mark_04 = 0;
            Mark_05 = 0;
            Mark_06 = 0;

            if (Question_YES_NO.Contains("Да")) Mark_01 = 1;
            if (Question_05.Contains("Да")) Mark_02 = 1;
            if (Question_01.Contains("Да")) Mark_03 = 1;
            if (Question_04.Contains("Да")) Mark_04 = 1;
            if (Question_07.Contains("Да")) Mark_05 = 1;
            if (Question_08.Contains("Не имеет значения")) Mark_06 = 1;

            SummBall = Mark_01 + Mark_02 + Mark_03 + Mark_04 + Mark_05 + Mark_06;

        }

        public class JSON_Kettel
        {
            public int ID { get; set; }
            public int Years { get; set; }
            public string Gender { get; set; }
            public List<JSON_Kettel_Question_Answer> QA { get; set; }
            public List<JSON_Kettel_KategoryResult> Results { get; set; }

            public JSON_Kettel()
            {
                this.ID = -1;
                this.Years = -1;
                this.Gender = "";
                this.QA = new List<JSON_Kettel_Question_Answer>();
                this.Results = new List<JSON_Kettel_KategoryResult>();
            }
            public JSON_Kettel(KT_Main main, KT_TestableAnswers[] answers)
            {
                ID = main.ID;
                Years = main.Years;
                Gender = main.Gender;
                QA = answers.Select(a => new JSON_Kettel_Question_Answer
                {
                    QuestionNumber = a.Answer.Question.Number.ToString(),
                    QuestionText = a.Answer.Question.Text,
                    AnswerNumber = a.Answer.AnswerNumber,
                    AnswerText = a.Answer.AnswerText
                }).OrderBy(x => x.QuestionNumber).ToList();
                Results = new List<JSON_Kettel_KategoryResult>();
                using (var db = new AnketaContext())
                {
                    var answerIds = answers.Select(a => a.ID).ToArray();
                    foreach (var category in db.KT_Categories)
                    {
                        var ballForCategory = db.KT_Keys
                           .Include(c => c.Category)
                           .Include(a => a.Answer)
                           .Where(x => x.Category.ID == category.ID && answerIds.Contains(x.Answer.ID))
                           .Sum(x => x.Score);
                        var mark = db.KT_Marks.FirstOrDefault(x => x.Category.ID == category.ID &&
                            x.ScoreFrom <= ballForCategory && x.ScoreTo >= ballForCategory);

                        Results.Add(new JSON_Kettel_KategoryResult
                        {
                            CategoryID = category.ID,
                            CategoryBall = mark.Mark,
                            CategoryLevel = mark.LevelWord,
                            CategoryName = category.CategoryName,
                            CategoryBallExcelColumn = mark.WordInReport,
                            CategoryLevelExcelColumn = mark.LevelWordInReport
                        });
                    }
                }
            }
        }

        public class JSON_Kettel_Question_Answer
        {
            public string QuestionNumber { get; set; }
            public string QuestionText { get; set; }
            public string AnswerNumber { get; set; }
            public string AnswerText { get; set; }
        }


        public class JSON_Kettel_KategoryResult
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; }
            public int CategoryBall { get; set; }
            public string CategoryBallExcelColumn { get; set; }
            public string CategoryLevel { get; set; }
            public string CategoryLevelExcelColumn { get; set; }
        }


        public class JSON_Competention_Answers
        {
            public int QuestionNumber { get; set; }
            public string QuestionText { get; set; }
            public JSON_Competention_AnswerResult[] Answers { get; set; }
        }

        public class JSON_Competention_AnswerResult
        {
            public string AnswerNumber { get; set; }
            public string AnswerText { get; set; }
            public int AnswerMark { get; set; }
        }


        public class JSON_Competention_Result
        {
            public string Block { get; set; }
            public string BlockWordInReport { get; set; }
            public int Ball { get; set; }
        }


    }
}
