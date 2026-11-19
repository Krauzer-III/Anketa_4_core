using Anketa_4_core.Data;
using Anketa_4_core.Data.LK_Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Anketa_4_core.Services
{
    /// <summary>
    /// Loads and updates individual sections of the JSON stored in TestableUser.LK_Data.
    /// Replaces one selected section only; unknown root properties and other sections survive.
    /// </summary>
    public class LKCabinetService
    {
        private readonly AnketaContext _db;
        private static readonly JsonSerializerOptions JsonOptions =
            new() { PropertyNameCaseInsensitive = true };

        private static readonly IReadOnlyDictionary<string, Type> SectionTypes =
            new Dictionary<string, Type>(StringComparer.Ordinal)
            {
                ["ProfTasks"] = typeof(List<LK_ProfTask>),
                ["Internships"] = typeof(List<LK_Internship>),
                ["LK_ViceBoss"] = typeof(List<LK_ViceBoss>),
                ["ReportBooks"] = typeof(List<LK_ReportBook>),
                ["TrainingProgramms"] = typeof(List<LK_TrainingProgramm>),
                ["DevelopmentalActionMaproperty"] = typeof(LK_DevelopmentalActionMap),
                ["AdvancedTrainingCourses"] = typeof(List<LK_AdvancedTrainingCourse>),
                ["LanguageTrainings"] = typeof(List<LK_LanguageTraining>),
                ["TeachingActivities"] = typeof(List<LK_TeachingActivity>),
                ["Mentorships"] = typeof(List<LK_Mentorship>),
                ["DistanceLearning"] = typeof(List<LK_DistanceLearning>),
                ["ProfessionalRetrainings"] = typeof(List<LK_ProfessionalRetraining>),
                ["ProjectActivities"] = typeof(List<LK_ProjectActivity>),
                ["ConferencesAndCompetitions"] = typeof(List<LK_ConferenceCompetition>),
                ["PsychologicalConsultations"] = typeof(List<LK_PsychologicalConsultation>),
                ["PersonnelMotivation"] = typeof(LK_PersonnelMotivation)
            };

        private static readonly HashSet<string> ObjectSections = new(StringComparer.Ordinal)
        {
            "DevelopmentalActionMaproperty",
            "PersonnelMotivation"
        };

        public LKCabinetService(AnketaContext db) => _db = db;

        public async Task<JsonObject> LoadForUserAsync(string username)
        {
            var stored = await _db.TestableUsers.AsNoTracking()
                .Where(x => x.UserName == username)
                .Select(x => x.LK_Data).FirstOrDefaultAsync();
            if (stored == null)
            {
                bool exists = await _db.TestableUsers.AnyAsync(x => x.UserName == username);
                if (!exists) throw new InvalidOperationException("Личный кабинет не найден.");
            }
            var root = ParseRoot(stored);
            ApplyPlanDefaults(root);
            return root;
        }

        public async Task<JsonObject> LoadForTestableAsync(int testableId)
        {
            var record = await _db.TestableUsers.AsNoTracking()
                .Where(x => x.testable.ID == testableId)
                .Select(x => new { x.ID, x.LK_Data }).FirstOrDefaultAsync();
            if (record == null)
                throw new InvalidOperationException("Тестируемому не назначен личный кабинет.");
            var root = ParseRoot(record.LK_Data);
            ApplyPlanDefaults(root);
            return root;
        }

        public async Task<JsonObject> SaveForUserAsync(string username, string section, JsonElement value)
        {
            var record = await _db.TestableUsers.AsNoTracking()
                .Where(x => x.UserName == username).Select(x => x.ID).FirstOrDefaultAsync();
            if (record == 0) throw new InvalidOperationException("Личный кабинет не найден.");
            return await SaveAsync(record, section, value);
        }

        public async Task<JsonObject> SaveForTestableAsync(int testableId, string section, JsonElement value)
        {
            var record = await _db.TestableUsers.AsNoTracking()
                .Where(x => x.testable.ID == testableId)
                .Select(x => x.ID).FirstOrDefaultAsync();
            if (record == 0)
                throw new InvalidOperationException("Тестируемому не назначен личный кабинет.");
            return await SaveAsync(record, section, value);
        }

        private async Task<JsonObject> SaveAsync(int recordId, string section, JsonElement value)
        {
            if (!SectionTypes.TryGetValue(section, out var targetType))
                throw new ArgumentException("Раздел личного кабинета не поддерживается.");
            JsonValueKind expectedKind = ObjectSections.Contains(section)
                ? JsonValueKind.Object : JsonValueKind.Array;
            if (value.ValueKind != expectedKind)
                throw new ArgumentException("Неверная структура данных раздела.");

            // Only known fields are validated. Update is atomic and only replaces
            // the selected section, not the entire LK_Data JSON.
            object typed = JsonSerializer.Deserialize(value.GetRawText(), targetType, JsonOptions)
                ?? throw new ArgumentException("Некорректные данные раздела.");
            var normalized = JsonSerializer.SerializeToNode(typed, targetType, JsonOptions);
            if (normalized == null) throw new ArgumentException("Данные раздела пусты.");

            await using var transaction =
                await _db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            await _db.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT 1 FROM \"TestableUsers\" WHERE \"ID\" = {recordId} FOR UPDATE");

            var record = await _db.TestableUsers.FirstOrDefaultAsync(x => x.ID == recordId)
                ?? throw new InvalidOperationException("Личный кабинет был удалён.");
            var root = ParseRoot(record.LK_Data);
            root[section] = normalized;
            ApplyPlanDefaults(root);
            record.LK_Data = root.ToJsonString();
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
            return root;
        }

        private static void ApplyPlanDefaults(JsonObject root)
        {
            // The official IПП instruction provides a fixed list of eight
            // distance-learning topics. Keep all as unselected until the
            // testable chooses at least two for the plan.
            if (root["DistanceLearning"] is not JsonArray learning || learning.Count == 0)
            {
                string[] topics =
                {
                    "Конфликт в команде: почему он будет и как из него выходить",
                    "Мотивация сотрудников: материальная и нематериальная",
                    "Постановка целей по SMART",
                    "Ситуационное руководство",
                    "Управление нематериальной мотивацией сотрудников",
                    "Успешное делегирование",
                    "Эмоциональный интеллект",
                    "Обратная связь (интерактивный курс)"
                };

                root["DistanceLearning"] = new JsonArray(topics.Select(topic =>
                    (JsonNode)new JsonObject
                    {
                        ["IsPlanned"] = JsonValue.Create(false),
                        ["ActivityType"] = JsonValue.Create("Дистанционный курс"),
                        ["Title"] = JsonValue.Create(topic),
                        ["Platform"] = JsonValue.Create("База знаний СНФПО ПАО «Газпром»")
                    }).ToArray());
            }

            // The instruction requires one professional book and two books
            // for personal/business/management competencies. The fixed book
            // from item 5.2.1 is prefilled; the testable enters reading dates
            // and the two reflection fields.
            if (root["ReportBooks"] is not JsonArray books || books.Count == 0)
            {
                root["ReportBooks"] = new JsonArray(
                    new JsonObject
                    {
                        ["BookCategory"] = JsonValue.Create("Профессиональные компетенции"),
                        ["BookName"] = JsonValue.Create(""),
                        ["Author"] = JsonValue.Create("")
                    },
                    new JsonObject
                    {
                        ["BookCategory"] = JsonValue.Create("Личностно-деловые и управленческие компетенции"),
                        ["BookName"] = JsonValue.Create("Руководство по развитию управленческих компетенций. Управление исполнением."),
                        ["Author"] = JsonValue.Create("В. Красноруций")
                    },
                    new JsonObject
                    {
                        ["BookCategory"] = JsonValue.Create("Личностно-деловые и управленческие компетенции"),
                        ["BookName"] = JsonValue.Create(""),
                        ["Author"] = JsonValue.Create("")
                    });
            }
        }

        public static JsonObject ParseRoot(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new JsonObject();
            try
            {
                return JsonNode.Parse(json) as JsonObject
                    ?? throw new InvalidOperationException("Корень LK_Data должен быть объектом JSON.");
            }
            catch (JsonException)
            {
                throw new InvalidOperationException("Данные личного кабинета содержат некорректный JSON.");
            }
        }
    }
}
