namespace WebServiceClient;

public static class AsyncEnumerableExtensions
{
    public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items, CancellationToken cancellationToken = default)
    {
        var results = new List<T>();
        await foreach (var item in items.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            results.Add(item);
        }
        return results;
    }

    //public static async IAsyncEnumerable<T2> CastList<T1, T2>(this IAsyncEnumerable<T1> items, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    //{
    //    await foreach (var item in items.WithCancellation(cancellationToken).ConfigureAwait(false))
    //    {
    //        yield return (T2)item!;
    //    }
    //}
}

