using Microsoft.Extensions.Logging;

namespace ProjectTaskManagement.Services;

public abstract class ServiceBase
{
    protected ILogger Logger { get; }

    protected ServiceBase(ILogger logger)
    {
        Logger = logger;
    }

    protected Task<T> ExecuteAsync<T>(Func<Task<T>> action) =>
        ServiceExecutor.ExecuteAsync(action, Logger);

    protected Task ExecuteAsync(Func<Task> action) =>
        ServiceExecutor.ExecuteAsync(action, Logger);

    protected T Execute<T>(Func<T> action) =>
        ServiceExecutor.Execute(action, Logger);
}
