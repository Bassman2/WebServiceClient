namespace WebServiceClient;

/// <summary>
/// Provides a base implementation for clients that interact with JSON-based web services.
/// Manages the lifecycle of a <see cref="JsonService"/> and provides common functionality
/// such as retrieving the service version.
/// </summary>
public abstract class JsonBaseClient : IDisposable
{
    /// <summary>
    /// The service instance managed by the client.
    /// </summary>
    private JsonService? baseService;

    /// <summary>
    /// Defines and assigns the service instance managed by the client.
    /// </summary>
    /// <typeparam name="T">The type of the service to define.</typeparam>
    /// <param name="service">The service instance to assign.</param>
    /// <returns>The assigned service instance.</returns>
    protected T DefineService<T>(T service)
    {
        baseService = service as JsonService;
        return service;
    }

    /// <summary>
    /// Disposes the service instance and suppresses finalization.
    /// </summary>
    public void Dispose()
    {
        if (this.baseService != null)
        {
            this.baseService.Dispose();
            this.baseService = null;
        }
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Asynchronously gets the version of the connected web service.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task{Version}"/> representing the asynchronous operation.</returns>
    public async Task<Version> GetVersionAsync(CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(baseService);

        return await baseService.GetVersionAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously gets the version string of the connected web service.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task{String}"/> representing the asynchronous operation.</returns>
    public async Task<string> GetVersionStringAsync(CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(baseService);

        return await baseService.GetVersionStringAsync(cancellationToken);
    }
}
