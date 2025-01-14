using EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCore.API.Data.Interceptors;

// While using interceptors
// We don't need to override SaveChanges()
// to insert custom logic
public class SaveChangesInterceptor : ISaveChangesInterceptor
{
    public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var changeTracker = eventData.Context!.ChangeTracker;
        var deleteGenreEntries = changeTracker.Entries<Genre>().Where(entry => entry.State == EntityState.Deleted);

        foreach (var entry in deleteGenreEntries)
        {
            entry.Property("IsDeleted").CurrentValue = true;
            entry.State = EntityState.Modified;
        }

        return result;
    }

    public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(SavingChanges(eventData, result));
    }
}
