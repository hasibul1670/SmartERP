namespace SmartERP.Persistence.RelationalDB.Common.Interfaces;

public interface ISequenceService
{
    Task<string> NextAsync(string prefix, string sequenceName, CancellationToken ct);
}