using Microsoft.EntityFrameworkCore;
using SmartERP.Persistence.RelationalDB.Common.Interfaces;
//change namespace here 
namespace SmartERP.Persistence.RelationalDB.Common.Ids;

public class SequenceService(SmartERPDbContext db) : ISequenceService
{
    public async Task<string> NextAsync(string prefix, string sequenceName, CancellationToken ct)
    {
        // IMPORTANT: identifiers can't be parameterized in PostgreSQL.
        // So we must embed the sequence name into SQL (after validation / trusted input).
        var sql = $@"SELECT nextval('""{sequenceName}""')";

        var next = await db.Database
            .SqlQueryRaw<long>(sql)
            .SingleAsync(ct);

        return $"{prefix}-{next:D6}";
    }
}