using System.Data;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Services;

/// <summary>
/// Exactly one AccessForTestable row per testable. Changing the period updates
/// that row in place so the AccessID and every linked test/result are preserved.
/// </summary>
public sealed class PeriodAssignmentService
{
    private readonly AnketaContext _db;

    public PeriodAssignmentService(AnketaContext db) => _db = db;

    public async Task<AccessForTestable> AssignAsync(
        int testableId, int periodId, CancellationToken cancellationToken = default)
    {
        if (testableId <= 0 || periodId <= 0)
            throw new ArgumentException("Укажите тестируемого и период.");

        // When called by an importing/batch controller, join its surrounding
        // transaction so that the period, assignment and other changes commit together.
        var ownsTransaction = _db.Database.CurrentTransaction == null;
        await using var transaction = ownsTransaction
            ? await _db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken)
            : null;

        // All assignment paths lock the same parent row before querying/creating
        // the link. The unique DB index is the final defence against duplicates.
        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT 1 FROM \"Testables\" WHERE \"ID\" = {testableId} FOR UPDATE",
            cancellationToken);

        var testable = await _db.Testables.FirstOrDefaultAsync(
            x => x.ID == testableId, cancellationToken)
            ?? throw new ArgumentException("Тестируемый не найден.", nameof(testableId));
        var period = await _db.TestPeriods.FirstOrDefaultAsync(
            x => x.ID == periodId, cancellationToken)
            ?? throw new ArgumentException("Период не найден.", nameof(periodId));

        var existing = await _db.AccessForTestables
            .Include(x => x.TestPeriod)
            .Where(x => x.Testable.ID == testableId)
            .OrderBy(x => x.ID)
            .ToListAsync(cancellationToken);

        // Never silently discard legacy rows: they may own primary results and JSON.
        if (existing.Count > 1)
            throw new InvalidOperationException(
                "Найдены прежние дубли назначений. Данные необходимо безопасно объединить до переназначения.");

        AccessForTestable access;
        if (existing.Count == 0)
        {
            access = new AccessForTestable
            {
                Testable = testable,
                TestPeriod = period,
                isActive = true
            };
            _db.AccessForTestables.Add(access);
        }
        else
        {
            access = existing[0];
            if (access.TestPeriod.ID != period.ID)
            {
                access.TestPeriod = period;
                access.isActive = true;
            }
            // An idempotent selection of the same period does not re-open a closed link.
        }

        await _db.SaveChangesAsync(cancellationToken);
        if (transaction != null)
            await transaction.CommitAsync(cancellationToken);
        return access;
    }
}
