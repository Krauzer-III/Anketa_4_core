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
                ["DevelopmentalActionMaproperty"] = typeof(LK_DevelopmentalActionMap)
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
            return ParseRoot(stored);
        }

        public async Task<JsonObject> LoadForTestableAsync(int testableId)
        {
            var record = await _db.TestableUsers.AsNoTracking()
                .Where(x => x.testable.ID == testableId)
                .Select(x => new { x.ID, x.LK_Data }).FirstOrDefaultAsync();
            if (record == null)
                throw new InvalidOperationException("Тестируемому не назначен личный кабинет.");
            return ParseRoot(record.LK_Data);
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
            if (value.ValueKind != (section == "DevelopmentalActionMaproperty"
                    ? JsonValueKind.Object : JsonValueKind.Array))
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
            record.LK_Data = root.ToJsonString();
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
            return root;
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
