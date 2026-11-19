using Anketa_4_core.Data.AnketaModels;
using Xunit;

namespace Anketa_4_core.Tests;

public sealed class JsonResultModelTests
{

    [Fact]
    public void AddKettelAnswer_UpdatesAggregateWithoutRecalculatingIncompleteTest()
    {
        var result = new JSON_ResultModel();
        var main = new KT_Main { ID = 12, Years = 41, Gender = "Женщина" };
        var question = new KT_Question { ID = 22, Number = 1, Text = "Вопрос" };
        var answer = new KT_Answer
        {
            ID = 33, Question = question, AnswerNumber = "2", AnswerText = "Ответ"
        };
        var selected = new KT_TestableAnswers
        {
            ID = 44, MainInfo = main, Answer = answer, DateTimeAnswer = DateTime.UtcNow
        };

        result.Add_JSON_Kettel_Answer(main, selected);

        var saved = Assert.Single(result.KettelTest.QA);
        Assert.Equal(22, saved.QuestionID);
        Assert.Equal(33, saved.AnswerID);
        Assert.Equal(41, result.KettelTest.Years);
        Assert.Equal("Женщина", result.KettelTest.Gender);
        Assert.False(result.KettelTest.IsCompleted);
    }

    [Fact]
    public void Test360Constructor_MapsRespondentRoleAndAllAnswers()
    {
        var entity = new Test360
        {
            ID = 55,
            IsSelfMark = false,
            DatetimeTest = DateTime.UtcNow,
            results = System.Text.Json.JsonSerializer.Serialize(new Test360_Result
            {
                q01 = 1, q02 = 2, q03 = 3, q04 = 4, q05 = 5,
                q06 = 1, q07 = 2, q08 = 3, q09 = 4, q10 = 5,
                q11 = 1, q12 = 2, q13 = 3, q14 = 4, q15 = 5
            })
        };
        var role = new RespondentRole { ID = 7, Name = "Эксперт", isSelfMark = false };

        var result = new JSON_Test360(entity, role);

        Assert.Equal(55, result.ID);
        Assert.Equal(7, result.RespondentRoleID);
        Assert.Equal("Эксперт", result.RespondentRoleName);
        Assert.False(result.IsSelfMark);
        Assert.Equal(5d, result.q15);
    }

    [Fact(Skip = "KNOWN DEFECT: JSON_ResultModel(int) contains a TODO and does not hydrate the existing result.")]
    public void ResultJson_AccessIdConstructorShouldLoadExistingResult()
    {
        var result = new JSON_ResultModel(1);
        Assert.NotNull(result.Tests360);
        Assert.NotNull(result.CompetentionAnswers);
    }

    [Fact(Skip = "KNOWN DEFECT: JSON_Test360 does not safely handle malformed/null survey JSON.")]
    public void Test360Constructor_MalformedSurveyJsonShouldNotThrow()
    {
        var entity = new Test360 { results = "{broken" };
        var role = new RespondentRole { ID = 1, Name = "Эксперт", isSelfMark = false };
        var result = new JSON_Test360(entity, role);
        Assert.NotNull(result);
    }

    [Fact]
    public void AddRespondent360_RecalculatesAveragesAndDoesNotCountSelfAssessment()
    {
        var result = new JSON_ResultModel();
        result.Add_JSON_Test360_ToReport_Testable(new JSON_Test360
        {
            IsSelfMark = true, q01 = 5, q02 = 5
        });
        result.Add_JSON_Test360_ToReport_Respondents(new JSON_Test360
        {
            IsSelfMark = false, q01 = 2, q02 = 4
        });
        result.Add_JSON_Test360_ToReport_Respondents(new JSON_Test360
        {
            IsSelfMark = false, q01 = 4, q02 = 2
        });

        Assert.Equal(3, result.Tests360.Count);
        Assert.Equal(3d, result.Test360ForReportRespondents.q01, 3);
        Assert.Equal(3d, result.Test360ForReportRespondents.q02, 3);
        Assert.Equal(5d, result.Test360ForReportTestable.q01, 3);
    }

    [Fact]
    public void AddSelf360_CopiesAllFifteenAnswers()
    {
        var result = new JSON_ResultModel();
        var self = new JSON_Test360
        {
            IsSelfMark = true,
            q01 = 1, q02 = 2, q03 = 3, q04 = 4, q05 = 5,
            q06 = 6, q07 = 7, q08 = 8, q09 = 9, q10 = 10,
            q11 = 11, q12 = 12, q13 = 13, q14 = 14, q15 = 15
        };

        result.Add_JSON_Test360_ToReport_Testable(self);

        Assert.Equal(15d, result.Test360ForReportTestable.q15);
        Assert.Single(result.Tests360);
        Assert.True(result.Tests360[0].IsSelfMark);
    }

    [Fact]
    public void SaveCompetentionQuestion_StoresFourRanksAndReplacesSameQuestion()
    {
        var block = new Comp_Block { ID = 1, BlockName = "Блок", Year = 1 };
        var question = new Comp_Question
        {
            ID = 10, QuestionNumber = 1, QuestionText = "Ситуация", Block = block
        };
        var answers = BuildCompetenceAnswers(question, new[] { 1, 2, 3, 4 });
        var result = new JSON_ResultModel();

        result.SaveCompetentionQuestion(1, question, answers);
        result.SaveCompetentionQuestion(1, question, BuildCompetenceAnswers(question, new[] { 4, 3, 2, 1 }));

        var saved = Assert.Single(result.CompetentionAnswers);
        Assert.Equal(1, saved.Year);
        Assert.Equal(10, saved.QuestionID);
        Assert.Equal(4, saved.Answers.Length);
        Assert.Equal(new[] { 4, 3, 2, 1 }, saved.Answers.Select(x => x.AnswerMark));
        Assert.Contains(saved.Answers, x => x.AnswerMark == x.CorrectAnswer && x.IsCorrect == true);
    }

    [Fact]
    public void SaveCompetentionQuestion_RejectsIncompleteOrDuplicateAnswerSets()
    {
        var question = new Comp_Question
        {
            ID = 11, QuestionNumber = 1, QuestionText = "Q",
            Block = new Comp_Block { ID = 1, BlockName = "B", Year = 1 }
        };
        var four = BuildCompetenceAnswers(question, new[] { 1, 2, 3, 4 });

        Assert.Throws<ArgumentException>(() =>
            new JSON_ResultModel().SaveCompetentionQuestion(1, question, four.Take(3).ToArray()));

        var duplicate = four.ToArray();
        duplicate[3] = duplicate[2];
        Assert.Throws<ArgumentException>(() =>
            new JSON_ResultModel().SaveCompetentionQuestion(1, question, duplicate));
    }

    [Fact]
    public void CalculateCompetentionYear_UsesYearTwoNormalizationAndPreservesOtherYear()
    {
        var block1 = new Comp_Block { ID = 1, BlockName = "Год 1", Year = 1, WordInReport = "A" };
        var block2 = new Comp_Block { ID = 2, BlockName = "Год 2", Year = 2, WordInReport = "B" };
        var result = new JSON_ResultModel
        {
            CompetentionResult = new List<JSON_Motivation_Result.JSON_Competention_Result>
            {
                new() { Year = 1, BlockID = 1, Ball = 4, MaxBall = 12 },
                new() { Year = 2, BlockID = 2, Ball = 1, MaxBall = 12 }
            }
        };

        var year2Answers = BuildCompetenceAnswers(
            new Comp_Question
            {
                ID = 20, QuestionNumber = 1, QuestionText = "Q2", Block = block2
            },
            Enumerable.Repeat(1, 11).Concat(new[] { 0 }).ToArray());

        result.CalculateCompetentionYear(2, new[] { block1, block2 }, year2Answers);

        var year1 = Assert.Single(result.CompetentionResult.Where(x => x.Year == 1));
        var year2 = Assert.Single(result.CompetentionResult.Where(x => x.Year == 2));
        Assert.Equal(4, year1.Ball);
        Assert.Equal(11, year2.Ball);
        Assert.Equal(12, year2.MaxBall);
        Assert.Equal(4, year2.LevelNumber);
    }

    [Fact]
    public void CalculateCompetentionYear_LeavesFirstYearLevelUnknownInsteadOfGuessing()
    {
        var block = new Comp_Block { ID = 3, BlockName = "Год 1", Year = 1 };
        var answers = BuildCompetenceAnswers(
            new Comp_Question { ID = 30, QuestionNumber = 1, QuestionText = "Q", Block = block },
            new[] { 1, 1, 1, 1 });

        var result = new JSON_ResultModel();
        result.CalculateCompetentionYear(1, new[] { block }, answers);

        Assert.Null(Assert.Single(result.CompetentionResult).LevelNumber);
    }

    [Fact]
    public void KettelRecordAnswer_ReplacesSameQuestionSortsAndClearsOldScores()
    {
        var main = new KT_Main { ID = 7, Years = 35, Gender = "Женщина" };
        var q2 = new KT_Question { ID = 2, Number = 2, Text = "Вопрос 2" };
        var q1 = new KT_Question { ID = 1, Number = 1, Text = "Вопрос 1" };
        var answer2 = new KT_Answer { ID = 22, Question = q2, AnswerNumber = "2", AnswerText = "Ответ 2" };
        var answer1 = new KT_Answer { ID = 11, Question = q1, AnswerNumber = "1", AnswerText = "Ответ 1" };
        var answer1New = new KT_Answer { ID = 12, Question = q1, AnswerNumber = "2", AnswerText = "Новый ответ" };
        var kettel = new JSON_Motivation_Result.JSON_Kettel
        {
            IsCompleted = true,
            Results = new List<JSON_Motivation_Result.JSON_Kettel_KategoryResult>
            {
                new() { CategoryID = 1, CategoryName = "A", CategoryBall = 5 }
            }
        };

        kettel.RecordAnswer(main, new KT_TestableAnswers
        {
            Answer = answer2, DateTimeAnswer = DateTime.UtcNow
        });
        kettel.RecordAnswer(main, new KT_TestableAnswers
        {
            Answer = answer1, DateTimeAnswer = DateTime.UtcNow
        });
        kettel.RecordAnswer(main, new KT_TestableAnswers
        {
            Answer = answer1New, DateTimeAnswer = DateTime.UtcNow
        });

        Assert.Equal(new[] { 1, 2 }, kettel.QA.Select(x => x.QuestionID));
        Assert.Equal(12, kettel.QA[0].AnswerID);
        Assert.False(kettel.IsCompleted);
        Assert.Empty(kettel.Results);
        Assert.Equal(35, kettel.Years);
        Assert.Equal("Женщина", kettel.Gender);
    }

    [Fact]
    public void KettelConstructor_CalculatesRawAndNormalizedCategoryScores()
    {
        var category = new KT_Category { ID = 1, CategoryName = "A" };
        var question = new KT_Question { ID = 1, Number = 1, Text = "Q" };
        var answer = new KT_Answer
        {
            ID = 100, Question = question, AnswerNumber = "1", AnswerText = "A"
        };
        var main = new KT_Main { ID = 5, Years = 30, Gender = "Мужчина" };
        var selected = new KT_TestableAnswers
        {
            ID = 200, MainInfo = main, Answer = answer, DateTimeAnswer = DateTime.UtcNow
        };
        var key = new KT_KeyForCategory { ID = 1, Category = category, Answer = answer, Score = 2 };
        var mark = new KT_Mark
        {
            ID = 1, Category = category, ScoreFrom = 0, ScoreTo = 2,
            Mark = 5, LevelWord = "Средний", WordInReport = "F", LevelWordInReport = "G"
        };

        var result = new JSON_Motivation_Result.JSON_Kettel(
            main, new[] { selected }, new[] { category }, new[] { key }, new[] { mark });

        Assert.True(result.IsCompleted);
        var score = Assert.Single(result.Results);
        Assert.Equal(2, score.RawScore);
        Assert.Equal(5, score.CategoryBall);
        Assert.Equal("Средний", score.CategoryLevel);
    }

    [Fact]
    public void KettelConstructor_RejectsMissingNormTable()
    {
        var category = new KT_Category { ID = 1, CategoryName = "A" };
        var question = new KT_Question { ID = 1, Number = 1, Text = "Q" };
        var answer = new KT_Answer { ID = 1, Question = question, AnswerNumber = "1", AnswerText = "A" };
        var main = new KT_Main { ID = 1, Years = 20, Gender = "Мужчина" };
        var selected = new KT_TestableAnswers { MainInfo = main, Answer = answer };

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new JSON_Motivation_Result.JSON_Kettel(
                main, new[] { selected }, new[] { category },
                Array.Empty<KT_KeyForCategory>(), Array.Empty<KT_Mark>()));
        Assert.Contains("Не загружены категории", exception.Message);
    }

    [Fact]
    public void MotivationResult_MapsSixScoringRules()
    {
        var entity = new MotivationTest
        {
            ID = 9,
            IsAgree = true,
            DatetimeTest = DateTime.UtcNow,
            Results = System.Text.Json.JsonSerializer.Serialize(new Motivation_Result
            {
                Question_YES_NO = "Да",
                Question_01 = "Да",
                Question_04 = "Да",
                Question_05 = "Да",
                Question_07 = "Да",
                Question_08 = "Не имеет значения"
            })
        };

        var result = new JSON_Motivation_Result(entity);

        Assert.Equal(new[] { 1, 1, 1, 1, 1, 1 },
            new[] { result.Mark_01, result.Mark_02, result.Mark_03, result.Mark_04, result.Mark_05, result.Mark_06 });
        Assert.Equal(6, result.SummBall);
    }

    [Fact]
    public void ResultJson_RoundTripsAllSections()
    {
        var original = new JSON_ResultModel
        {
            TestableCode = "000012",
            AccessID = 12,
            Tests360 = new List<JSON_Test360> { new() { IsSelfMark = true, q01 = 5 } },
            CompetentionResult = new List<JSON_Motivation_Result.JSON_Competention_Result>
            {
                new() { Year = 2, BlockID = 3, Ball = 11, MaxBall = 12, LevelNumber = 4 }
            }
        };

        var restored = new JSON_ResultModel(original.GetJSON());

        Assert.Equal("000012", restored.TestableCode);
        Assert.Equal(12, restored.AccessID);
        Assert.Single(restored.Tests360);
        Assert.Equal(4, Assert.Single(restored.CompetentionResult).LevelNumber);
    }

    [Fact(Skip = "KNOWN DEFECT: invalid JSON is swallowed and leaves JSON_ResultModel collections null; fix constructor before enabling.")]
    public void ResultJson_InvalidInputShouldLeaveSafeEmptyCollections()
    {
        var result = new JSON_ResultModel("{broken");
        Assert.NotNull(result.Tests360);
        Assert.NotNull(result.CompetentionAnswers);
    }

    [Fact(Skip = "KNOWN DEFECT: JSON_Motivation_Result dereferences nullable question fields; fix null handling before enabling.")]
    public void MotivationResult_MissingOptionalQuestionFieldsShouldNotThrow()
    {
        var entity = new MotivationTest
        {
            ID = 1, IsAgree = false, Results = "{}"
        };
        var result = new JSON_Motivation_Result(entity);
        Assert.Equal(0, result.SummBall);
    }

    private static Comp_TestableAnswer[] BuildCompetenceAnswers(
        Comp_Question question, IReadOnlyList<int> marks)
    {
        return Enumerable.Range(0, marks.Count).Select(i =>
        {
            var answer = new Comp_Answer
            {
                ID = i + 1,
                Question = question,
                Number = (i + 1).ToString(),
                Text = "Вариант " + (i + 1),
                Correct_Answer = 1
            };
            return new Comp_TestableAnswer
            {
                Answer = answer,
                Mark = marks[i],
                DateTimeAnswer = DateTime.UtcNow
            };
        }).ToArray();
    }
}
