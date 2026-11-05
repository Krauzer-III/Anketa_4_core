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
        public JSON_ResultModel()
        {
            _JSON_ResultModel();
        }

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
            Tests360 ??= new List<JSON_Test360>();
            Tests360.Add(t360);
            var items = Tests360.Where(e => !e.IsSelfMark).ToList();
            Test360ForReportRespondents = new JSON_Test360_ToReport
            {
                q01 = items.Average(e => (double)e.q01),
                q02 = items.Average(e => (double)e.q02),
                q03 = items.Average(e => (double)e.q03),
                q04 = items.Average(e => (double)e.q04),
                q05 = items.Average(e => (double)e.q05),
                q06 = items.Average(e => (double)e.q06),
                q07 = items.Average(e => (double)e.q07),
                q08 = items.Average(e => (double)e.q08),
                q09 = items.Average(e => (double)e.q09),
                q10 = items.Average(e => (double)e.q10),
                q11 = items.Average(e => (double)e.q11),
                q12 = items.Average(e => (double)e.q12),
                q13 = items.Average(e => (double)e.q13),
                q14 = items.Average(e => (double)e.q14),
                q15 = items.Average(e => (double)e.q15)
            };
        }

        public void Add_JSON_Test360_ToReport_Testable(JSON_Test360 t360)
        {
            Tests360 ??= new List<JSON_Test360>();
            Tests360.Add(t360);
            Test360ForReportTestable = new JSON_Test360_ToReport
            {
                q01 = t360.q01, q02 = t360.q02, q03 = t360.q03,
                q04 = t360.q04, q05 = t360.q05, q06 = t360.q06,
                q07 = t360.q07, q08 = t360.q08, q09 = t360.q09,
                q10 = t360.q10, q11 = t360.q11, q12 = t360.q12,
                q13 = t360.q13, q14 = t360.q14, q15 = t360.q15
            };
        }

        public void Add_JSON_Kettel(KT_Main main, KT_TestableAnswers[] answers)
        {
            KettelTest = new JSON_Kettel(main, answers);
        }

        /// <summary>
        /// Сохраняет очередной ответ Кеттела в общий JSON без расчёта итогов.
        /// В конце теста Add_JSON_Kettel заменяет промежуточные данные полным результатом.
        /// </summary>
        public void Add_JSON_Kettel_Answer(KT_Main main, KT_TestableAnswers answer)
        {
            KettelTest ??= new JSON_Kettel();
            KettelTest.RecordAnswer(main, answer);
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


        public JSON_Test360() { }

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

            if (m.IsAgree && string.Equals(Question_YES_NO?.Trim(), "Да", StringComparison.Ordinal)) Mark_01 = 1;
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
            public bool IsCompleted { get; set; }

            public JSON_Kettel()
            {
                ID = -1;
                Years = -1;
                Gender = "";
                QA = new List<JSON_Kettel_Question_Answer>();
                Results = new List<JSON_Kettel_KategoryResult>();
            }

            /// <summary>
            /// Совместимость с прежним способом создания отчётных результатов.
            /// Для сохранения последнего ответа внутри транзакции используйте
            /// перегрузку, принимающую категории/ключи/диапазоны из того же DbContext.
            /// </summary>
            public JSON_Kettel(KT_Main main, KT_TestableAnswers[] answers) : this()
            {
                using var db = new AnketaContext();
                Calculate(main, answers,
                    db.KT_Categories.ToList(),
                    db.KT_Keys.Include(x => x.Category).Include(x => x.Answer).ToList(),
                    db.KT_Marks.Include(x => x.Category).ToList());
            }

            public JSON_Kettel(
                KT_Main main,
                KT_TestableAnswers[] answers,
                IReadOnlyCollection<KT_Category> categories,
                IReadOnlyCollection<KT_KeyForCategory> keys,
                IReadOnlyCollection<KT_Mark> marks) : this()
            {
                Calculate(main, answers, categories, keys, marks);
            }

            public void RecordAnswer(KT_Main main, KT_TestableAnswers answer)
            {
                ID = main.ID;
                Years = main.Years;
                Gender = main.Gender;
                QA ??= new List<JSON_Kettel_Question_Answer>();
                Results ??= new List<JSON_Kettel_KategoryResult>();

                var question = answer.Answer.Question;
                QA.RemoveAll(x => x.QuestionID == question.ID ||
                    (x.QuestionID == 0 && x.QuestionNumber == question.Number.ToString()));

                QA.Add(new JSON_Kettel_Question_Answer
                {
                    QuestionID = question.ID,
                    AnswerID = answer.Answer.ID,
                    QuestionNumber = question.Number.ToString(),
                    QuestionText = question.Text,
                    AnswerNumber = answer.Answer.AnswerNumber,
                    AnswerText = answer.Answer.AnswerText,
                    DateTimeAnswer = answer.DateTimeAnswer
                });
                QA = QA.OrderBy(x => int.TryParse(x.QuestionNumber, out var number)
                        ? number : int.MaxValue).ToList();

                // Сырые ответы уже записаны, но стандартизованные баллы
                // появляются только после последнего вопроса.
                IsCompleted = false;
                Results.Clear();
            }

            private void Calculate(
                KT_Main main,
                KT_TestableAnswers[] answers,
                IReadOnlyCollection<KT_Category> categories,
                IReadOnlyCollection<KT_KeyForCategory> keys,
                IReadOnlyCollection<KT_Mark> marks)
            {
                foreach (var answer in answers.OrderBy(a => a.Answer.Question.Number))
                    RecordAnswer(main, answer);

                var selectedAnswerIDs = answers.Select(x => x.Answer.ID).ToHashSet();
                Results.Clear();

                foreach (var category in categories.OrderBy(x => x.ID))
                {
                    // Ключи привязаны к KT_Answer.ID, а не к KT_TestableAnswers.ID!
                    int rawScore = keys.Where(x => x.Category.ID == category.ID &&
                                                  selectedAnswerIDs.Contains(x.Answer.ID))
                                       .Sum(x => x.Score);
                    var mark = marks.FirstOrDefault(x => x.Category.ID == category.ID &&
                         x.ScoreFrom <= rawScore && rawScore <= x.ScoreTo);
                    if (mark == null)
                        throw new InvalidOperationException(
                            $"Нет таблицы перевода баллов для категории '{category.CategoryName}', результат {rawScore}.");

                    Results.Add(new JSON_Kettel_KategoryResult
                    {
                        CategoryID = category.ID,
                        CategoryName = category.CategoryName,
                        RawScore = rawScore,
                        CategoryBall = mark.Mark,
                        CategoryLevel = mark.LevelWord,
                        CategoryBallExcelColumn = mark.WordInReport,
                        CategoryLevelExcelColumn = mark.LevelWordInReport
                    });
                }

                IsCompleted = true;
            }
        }

        public class JSON_Kettel_Question_Answer
        {
            public int QuestionID { get; set; }
            public int AnswerID { get; set; }
            public string QuestionNumber { get; set; }
            public string QuestionText { get; set; }
            public string AnswerNumber { get; set; }
            public string AnswerText { get; set; }
            public DateTime DateTimeAnswer { get; set; }
        }

        public class JSON_Kettel_KategoryResult
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; }
            public int RawScore { get; set; }
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
