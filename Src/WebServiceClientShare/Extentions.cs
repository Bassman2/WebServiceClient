namespace WebServiceClient;

internal static class Extentions
{
    private const BindingFlags ConstructorDefault = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance;


    #region CastModel

    public static T? CastModel<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this object? value) where T : class
    {
        return value is not null ? (T?)Activator.CreateInstance(typeof(T), ConstructorDefault, null, [value], null) : null;
    }

    public static T? CastModel<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this object? value, object service) where T : class
    {
        return value is not null ? (T?)Activator.CreateInstance(typeof(T), ConstructorDefault, null, [service, value], null) : null;
    }

    public static IEnumerable<T> CastModel<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IEnumerable? value) where T : class
    {
        if (value is not null)
        {
            foreach (var item in value)
            {
                var inst = Activator.CreateInstance(typeof(T), ConstructorDefault, null, [item], null);
                if (inst != null)
                {
                    yield return (T)inst;
                }
            }
        }
    }

    public static IEnumerable<T> CastModel<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IEnumerable? value, object service) where T : class
    {
        if (value is not null)
        {
            foreach (var item in value)
            {
                var inst = Activator.CreateInstance(typeof(T), ConstructorDefault, null, [service, item], null);
                if (inst != null)
                {
                    yield return (T)inst;
                }
            }
        }
    }

    #endregion

    #region CastFields

    public static int? GetFieldInt(this IssueModel issueModel, string key)
    {
        if (issueModel.Fields!.TryGetValue(key, out JsonElement? elm) && elm != null)
        {
            var res = elm?.GetInt32();
            return res;
        }
        return null;
    }

    public static string? GetFieldString(this IssueModel issueModel, string key)
    {
        if (issueModel.Fields!.TryGetValue(key, out JsonElement? elm) && elm != null)
        {
            var res = elm?.GetString();
            return res;
        }
        return null;
    }

    public static DateTime? GetFieldDateTime(this IssueModel issueModel, string key)
    {
        if (issueModel.Fields!.TryGetValue(key, out JsonElement? elm) && elm != null)
        {
            try
            {
                var res = elm?.GetDateTime();
                return res;
            }
            catch
            {
                // fix missing double point in time zone
                var str = elm?.GetString();
                str = str?.Insert(str.Length - 2, ":");
                var res = DateTime.Parse(str!);
                return res;
            }
        }
        return null;
    }

    public static T? GetField<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicConstructors)] T,
        TM>(this IssueModel issueModel, string key) where T : class where TM : class
    {
        JsonTypeInfo<TM> jsonTypeInfo = (JsonTypeInfo<TM>)SourceGenerationContext.Default.GetTypeInfo(typeof(TM))!;

        if (issueModel.Fields!.TryGetValue(key, out JsonElement? elm) && elm != null)
        {
            var model = JsonSerializer.Deserialize<TM>((JsonElement)elm!, jsonTypeInfo);
            var item = model.CastModel<T>();
            return item;
        }
        return default;
    }

    public static T? GetField<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicConstructors)] T,
        TM>(this IssueModel issueModel, string key, JiraService service) where T : class where TM : class
    {
        JsonTypeInfo<TM> jsonTypeInfo = (JsonTypeInfo<TM>)SourceGenerationContext.Default.GetTypeInfo(typeof(TM))!;

        if (issueModel.Fields!.TryGetValue(key, out JsonElement? elm) && elm != null)
        {
            var model = JsonSerializer.Deserialize<TM>((JsonElement)elm!, jsonTypeInfo);
            var item = model.CastModel<T>(service);
            return item;
        }
        return default;
    }

    #endregion

    #region IAsyncEnumerable

    public static async Task<List<T>?> ToListAsync<T>(this IAsyncEnumerable<T>? items, CancellationToken cancellationToken = default)
    {
        if (items == null)
        {
            return null;
        }
        var results = new List<T>();
        await foreach (var item in items.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            results.Add(item);
        }
        return results;
    }

    //public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items, CancellationToken cancellationToken = default)
    //{
    //    var results = new List<T>();
    //    await foreach (var item in items.WithCancellation(cancellationToken).ConfigureAwait(false))
    //    {
    //        results.Add(item);
    //    }
    //    return results;
    //}

    public static IEnumerable<T> ToEnumerable<T>(this IAsyncEnumerable<T> asyncEnumerable)
    {
        var list = new BlockingCollection<T>();

        async Task AsyncIterate()
        {
            await foreach (var item in asyncEnumerable)
            {
                list.Add(item);
            }

            list.CompleteAdding();
        }

        _ = AsyncIterate();

        return list.GetConsumingEnumerable();
    }

    public static List<T> ToList<T>(this IAsyncEnumerable<T> asyncEnumerable)
        => asyncEnumerable.ToEnumerable().ToList();


    //public static async IAsyncEnumerable<T2> CastList<T1, T2>(this IAsyncEnumerable<T1> items, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    //{
    //    await foreach (var item in items.WithCancellation(cancellationToken).ConfigureAwait(false))
    //    {
    //        yield return (T2)item!;
    //    }
    //}

    #endregion

    #region Facade

    public static T? Facade<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this object? value) where T : class
    {
        if (value is null) { return null; }

        return (T?)Activator.CreateInstance(typeof(T), value);
    }

    //public static T2? Facade<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2>(this T1? value) where T1 : class where T2 : class
    //{
    //    if (value is null) { return null; }

    //    return (T2?)Activator.CreateInstance(typeof(T2), value);
    //}

    public static IEnumerable<T2>? Facade<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2>(this IEnumerable<T1>? values) where T1 : class where T2 : class
    {
        if (values is null) { return null; }

        return values.Select(item => ((T2?)Activator.CreateInstance(typeof(T2), item))!);
    }

    #endregion
}
