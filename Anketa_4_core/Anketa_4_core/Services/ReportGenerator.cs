using Anketa_4_core.Data.AnketaModels;
using ClosedXML.Excel;

namespace Anketa_4_core.Services
{
    /// <summary>
    /// XLSX rendering of already calculated TestResult.Results data.
    /// Sheet positions and column ordering are inherited from
    /// Anketa 3 Helpers/MakeExcelReport.cs, without old database queries.
    /// </summary>
    public static class ReportGenerator
    {
        public sealed class Subject
        {
            public int AccessID { get; init; }
            public string Code { get; init; } = "";
            public string Filial { get; init; } = "";
            public string Name { get; init; } = "—";
            public string Position { get; init; } = "—";
            public string Period { get; init; } = "";
            public JSON_ResultModel Data { get; init; } = new();
        }

        private static readonly string[] MarkColumns =
            { "H", "I", "J", "K", "M", "N", "O", "Q", "R", "S", "T", "U", "W", "X", "Y" };

        // Historical 360° order from MakeExcelReport.RespondentMarks().
        private static double[] Order360(JSON_Test360_ToReport v) =>
            new[] { v.q02, v.q03, v.q10, v.q15, v.q04, v.q06, v.q08,
                    v.q01, v.q09, v.q11, v.q13, v.q14, v.q05, v.q07, v.q12 };

        private static readonly string[] Categories =
            { "A", "B", "C", "E", "F", "G", "H", "I",
              "L", "M", "N", "O", "Q1", "Q2", "Q3", "Q4" };

        private static readonly string[] CategoryScoreColumns =
            { "F", "H", "J", "L", "N", "P", "R", "T",
              "V", "X", "Z", "AB", "AD", "AF", "AH", "AJ" };
        private static readonly string[] CategoryLevelColumns =
            { "G", "I", "K", "M", "O", "Q", "S", "U",
              "W", "Y", "AA", "AC", "AE", "AG", "AI", "AK" };

        /// <summary>
        /// Returns XLSX bytes. The report.xlsx file must be placed into
        /// {ContentRoot}/Reports/report.xlsx (as configured in csproj).
        /// Copies the second-year tab if this is an old one-year template.
        /// </summary>
        public static byte[] Generate(
            IReadOnlyList<Subject> subjects, string templatePath)
        {
            if (subjects == null || subjects.Count == 0)
                throw new ArgumentException("Не выбраны тестируемые.");
            if (subjects.Count > 100)
                throw new ArgumentException("В шаблоне предусмотрено не более 100 строк участников.");
            if (!File.Exists(templatePath))
                throw new FileNotFoundException(
                    "Не найден файл Reports/report.xlsx. Добавьте Excel-шаблон в проект.",
                    templatePath);

            using var workbook = new XLWorkbook(templatePath);
            EnsureCompetenceSheets(workbook);

            // A single workbook is used for a single assessment period; callers
            // must not mix participants belonging to different TestPeriods.
            var allPeriods = subjects.Select(x => x.Period).Distinct().ToArray();
            if (allPeriods.Length != 1)
                throw new InvalidOperationException(
                    "В один Excel-отчёт можно включать только один период оценки.");

            var sheet360 = workbook.Worksheet("360° ");
            var motivation = workbook.Worksheet("Мотивац. опросник");
            var kettel = workbook.Worksheet("Псих обследование");
            var comp1 = workbook.Worksheet("Компетенции (1 год)");
            var comp2 = workbook.Worksheet("Компетенции (2 год)");

            SetPeriod(sheet360, allPeriods[0]);
            SetPeriod(motivation, allPeriods[0]);
            SetPeriod(kettel, allPeriods[0]);
            SetPeriod(comp1, allPeriods[0]);
            SetPeriod(comp2, allPeriods[0]);

            for (int i = 0; i < subjects.Count; i++)
            {
                Write360(sheet360, 11 + i * 3, i + 1, subjects[i]);
                WriteMotivation(motivation, 13 + i, i + 1, subjects[i]);
                WriteKettel(kettel, 13 + i, i + 1, subjects[i]);
                WriteCompetence(comp1, 14 + i, i + 1, subjects[i], 1);
                WriteCompetence(comp2, 14 + i, i + 1, subjects[i], 2);
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        private static void EnsureCompetenceSheets(XLWorkbook workbook)
        {
            // Compatible with both uploaded legacy and updated template.
            if (!workbook.Worksheets.TryGetWorksheet("Компетенции (1 год)", out var year1))
            {
                if (!workbook.Worksheets.TryGetWorksheet("Компетенции", out year1))
                    throw new InvalidOperationException(
                        "В шаблоне отсутствует лист компетенций.");
                year1.Name = "Компетенции (1 год)";
            }
            if (!workbook.Worksheets.TryGetWorksheet("Компетенции (2 год)", out _))
                year1.CopyTo("Компетенции (2 год)");
        }

        private static void SetPeriod(IXLWorksheet sheet, string value)
        {
            // Historical Excel layouts use row 8 for the assessment period.
            sheet.Cell("B8").Value = value;
        }

        private static void Identity(IXLWorksheet sheet, int row, int ordinal, Subject item)
        {
            sheet.Cell(row, 1).Value = ordinal;
            sheet.Cell(row, 2).Value = item.Code; // keep code as text
            sheet.Cell(row, 3).Value = item.Name;
            sheet.Cell(row, 4).Value = item.Position;
            sheet.Cell(row, 5).Value = item.Filial;
        }

        private static void Write360(
            IXLWorksheet sheet, int formulaRow, int ordinal, Subject item)
        {
            Identity(sheet, formulaRow, ordinal, item);

            var data = item.Data;
            var tests = data.Tests360 ?? new List<JSON_Test360>();
            var self = tests.FirstOrDefault(x => x.IsSelfMark);
            var count = tests.Count(x => !x.IsSelfMark);
            var selfMarks = self != null && data.Test360ForReportTestable != null
                ? Order360(data.Test360ForReportTestable) : new double[15];
            var respondentMarks = count > 0 && data.Test360ForReportRespondents != null
                ? Order360(data.Test360ForReportRespondents) : new double[15];

            int selfRow = formulaRow + 1;
            int respondentsRow = formulaRow + 2;

            sheet.Cell("F" + formulaRow).Value = "Интегральная оценка";
            sheet.Cell("F" + selfRow).Value = "Самооценка";
            sheet.Cell("F" + respondentsRow).Value = "Оценка экспертов";

            for (int j = 0; j < MarkColumns.Length; j++)
            {
                string column = MarkColumns[j];
                double selfValue = selfMarks[j];
                double respondentValue = respondentMarks[j];
                double integrated = (count + (self == null ? 0 : 1)) > 0
                    ? (respondentValue * count + (self == null ? 0 : selfValue)) /
                      (count + (self == null ? 0 : 1))
                    : 0;

                // Pre-calculated averages from JSON; no queries to Test360.
                sheet.Cell(column + selfRow).Value = selfValue;
                sheet.Cell(column + respondentsRow).Value = respondentValue;
                sheet.Cell(column + formulaRow).Value = integrated;
                sheet.Cell(column + formulaRow).Style.NumberFormat.Format = "0.0";
                sheet.Cell(column + respondentsRow).Style.NumberFormat.Format = "0.0";
            }
            sheet.Cell("AB" + respondentsRow).Value = count;
            // Leave the existing template's aggregate formulas in L/P/V/Z/AA.
        }

        private static void WriteMotivation(
            IXLWorksheet sheet, int row, int ordinal, Subject item)
        {
            Identity(sheet, row, ordinal, item);
            var test = item.Data.MotivationTest;
            if (test == null || test.ID <= 0) return;

            // Precisely the four groups from Anketa 3 MakeExcelReport.
            sheet.Cell("F" + row).Value = test.Mark_01 + test.Mark_02;
            sheet.Cell("G" + row).Value = test.Mark_03 + test.Mark_04;
            sheet.Cell("I" + row).Value = test.Mark_05;
            sheet.Cell("J" + row).Value = test.Mark_06;
        }

        private static void WriteKettel(
            IXLWorksheet sheet, int row, int ordinal, Subject item)
        {
            Identity(sheet, row, ordinal, item);
            var marks = item.Data.KettelTest?.Results;
            if (marks == null || marks.Count == 0) return;

            foreach (var mark in marks)
            {
                int ix = Array.FindIndex(Categories,
                    x => x.Equals(mark.CategoryName, StringComparison.OrdinalIgnoreCase));
                if (ix < 0) continue;

                sheet.Cell(CategoryScoreColumns[ix] + row).Value = mark.CategoryBall;
                // Old report uses H(высокий) / C(средний) / Н(низкий) labels.
                string level = mark.CategoryBall <= 3 ? "Н" :
                               mark.CategoryBall <= 7 ? "С" : "В";
                sheet.Cell(CategoryLevelColumns[ix] + row).Value = level;
            }
        }

        private static void WriteCompetence(
            IXLWorksheet sheet, int row, int ordinal, Subject item, int year)
        {
            Identity(sheet, row, ordinal, item);
            var result = item.Data.CompetentionResult?
                .Where(x => x.Year == year).ToArray();
            if (result == null) return;

            foreach (var block in result)
            {
                string column = block.BlockWordInReport?.Trim() ?? "";
                if (!IsValidColumn(column)) continue;

                // Anketa 3 wrote LevelNumber, NOT the raw Ball. The new
                // JSON currently contains only Ball and MaxBall. Never put
                // raw scores into a cell labelled "Фактический уровень".
                // When LevelNumber is populated by the scoring workflow,
                // the generator uses it without further database queries.
                if (block.LevelNumber.HasValue)
                    sheet.Cell(column + row).Value = block.LevelNumber.Value;
            }
        }

        private static bool IsValidColumn(string column) =>
            column.Length is >= 1 and <= 3 &&
            column.All(ch => ch is >= 'A' and <= 'Z');
    }
}
