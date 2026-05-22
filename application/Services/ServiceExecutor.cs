using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectTaskManagement.Exceptions;

namespace ProjectTaskManagement.Services;

public static class ServiceExecutor
{
    public static async Task<T> ExecuteAsync<T>(Func<Task<T>> action, ILogger? logger = null)
    {
        try
        {
            return await action();
        }
        catch (AppException)
        {
            throw;
        }
        catch (DbUpdateException ex)
        {
            logger?.LogError(ex, "Database update failed");
            throw new AppException("A database error occurred.", HttpErrors.InternalServerError);
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Unexpected service error");
            throw new AppException("An unexpected error occurred.", HttpErrors.InternalServerError);
        }
    }

    public static async Task ExecuteAsync(Func<Task> action, ILogger? logger = null)
    {
        await ExecuteAsync(async () =>
        {
            await action();
            return true;
        }, logger);
    }

    public static T Execute<T>(Func<T> action, ILogger? logger = null)
    {
        try
        {
            return action();
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Unexpected service error");
            throw new AppException("An unexpected error occurred.", HttpErrors.InternalServerError);
        }
    }
}
