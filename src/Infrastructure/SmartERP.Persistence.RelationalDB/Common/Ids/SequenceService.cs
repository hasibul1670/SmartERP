using Microsoft.EntityFrameworkCore;
using SmartERP.Persistence.RelationalDB.Common.Interfaces;

//change namespace here 
namespace SmartERP.Persistence.RelationalDB.Common.Ids;

public class SequenceService(SmartERPDbContext db) : ISequenceService
{
    public async Task<string> NextAsync(string prefix, string sequenceName, CancellationToken ct)
    {
        var sql = $@"SELECT nextval('public.""{sequenceName}""') AS ""Value""";
        var next = await db.Database
            .SqlQueryRaw<long>(sql)
            .SingleAsync(ct);

        return $"{prefix}-{next:D6}";
    }
}