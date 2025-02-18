namespace WebServiceClient;

internal static class Extentions
{
    private const BindingFlags ConstructorDefault = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance;


    #region CastModel

    public static T? CastModel<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this object? value) where T : class
    {
        return value is null ? null : (T)(Activator.CreateInstance(typeof(T), ConstructorDefault, null, [value], null) ?? throw new ArgumentException($"CastModel failed for {typeof(T).Name}!"));
    }

    public static T? CastModel<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this object? value, object service) where T : class
    {
        return value is null ? null : (T)(Activator.CreateInstance(typeof(T), ConstructorDefault, null, [service, value], null) ?? throw new ArgumentException($"CastModel failed for {typeof(T).Name}!"));
    }

    public static List<T>? CastModel<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IEnumerable? value) where T : class
    {
        return value?.Cast<object>().Select(i => (T)(Activator.CreateInstance(typeof(T), ConstructorDefault, null, [i], null) ?? throw new ArgumentException($"CastModel failed for {typeof(T).Name}!"))).ToList<T>();
    }

    public static List<T>? CastModel<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IEnumerable? value, object service) where T : class
    {
        return value?.Cast<object>().Select(i => (T)(Activator.CreateInstance(typeof(T), ConstructorDefault, null, [service, i], null) ?? throw new ArgumentException($"CastModel with service failed for {typeof(T).Name}!"))).ToList<T>();
    }

    #endregion

    #region JsonValue

    public static int? GetJsonValueInt(this Dictionary<string, object?>? dict, string key)
    {
        if ((dict?.TryGetValue(key, out object? elm) ?? false) && elm != null)
        {
            var res = ((JsonElement)elm).GetInt32();
            return res;
        }
        return null;
    }

    public static string? GetJsonValueString(this Dictionary<string, object?>? dict, string key)
    {
        if ((dict?.TryGetValue(key, out object? elm) ?? false) && elm != null)
        {
            var res = ((JsonElement)elm).GetString();
            return res;
        }
        return null;
    }

    public static DateTime? GetJsonValueDateTime(this Dictionary<string, object?>? dict, string key)
    {
        if ((dict?.TryGetValue(key, out object? elm) ?? false) && elm != null)
        {
            try
            {
                var res = ((JsonElement)elm).GetDateTime();
                return res;
            }
            catch
            {
                // fix missing double point in time zone
                var str = ((JsonElement)elm).GetString();
                str = str?.Insert(str.Length - 2, ":");
                var res = DateTime.Parse(str!);
                return res;
            }
        }
        return null;
    }

    public static T? GetJsonValue<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicConstructors)] T,
        TM>(this Dictionary<string, object?>? dict, string key, JsonSerializerContext context) where T : class where TM : class
    {
        if ((dict?.TryGetValue(key, out object? elm) ?? false) && elm != null)
        {
            JsonTypeInfo<TM> jsonTypeInfo = (JsonTypeInfo<TM>)context.GetTypeInfo(typeof(TM))!;
            var model = JsonSerializer.Deserialize<TM>((JsonElement)elm!, jsonTypeInfo);
            var item = model.CastModel<T>();
            return item;
        }
        return default;
    }

    public static T? GetJsonValue<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicConstructors)] T,
        TM>(this Dictionary<string, object?>? dict, string key, object service, JsonSerializerContext context) where T : class where TM : class
    {
        if ((dict?.TryGetValue(key, out object? elm) ?? false) && elm != null)
        {
            JsonTypeInfo<TM> jsonTypeInfo = (JsonTypeInfo<TM>)context.GetTypeInfo(typeof(TM))!;
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

    //public static T? Facade<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this object? value) where T : class
    //{
    //    if (value is null) { return null; }

    //    return (T?)Activator.CreateInstance(typeof(T), value);
    //}

    //public static T2? Facade<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2>(this T1? value) where T1 : class where T2 : class
    //{
    //    if (value is null) { return null; }

    //    return (T2?)Activator.CreateInstance(typeof(T2), value);
    //}

    //public static IEnumerable<T2>? Facade<T1, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T2>(this IEnumerable<T1>? values) where T1 : class where T2 : class
    //{
    //    if (values is null) { return null; }

    //    return values.Select(item => ((T2?)Activator.CreateInstance(typeof(T2), item))!);
    //}

    #endregion

    #region PatternMatch

    //internal static IEnumerable<FileModel> WhereMatch(this IEnumerable<FileModel> source, string? pattern)
    //{
    //    return source.Where(f => pattern == null || Regex.IsMatch(f.Uri!.ToString().Trim('/'), pattern, RegexOptions.IgnoreCase));
    //}

    internal static string? FilterToRegex(this string? filter)
    {
        if (filter == null || filter == "*" || filter == "*.*")
        {
            return null;
        }

        var s = new StringBuilder(filter);
        s.Replace(".", @"\.");
        s.Replace("+", @"\+");
        s.Replace("$", @"\$");
        s.Replace("(", @"\(");
        s.Replace(")", @"\)");
        s.Replace("[", @"\[");
        s.Replace("]", @"\]");
        s.Replace("?", ".?");
        s.Replace("*", ".*");
        return s.ToString();
    }

    #endregion

    public static string? Value<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TEnum>(this TEnum enumVal) where TEnum : Enum
    {
        return typeof(TEnum).GetMember(enumVal?.ToString()!).FirstOrDefault()?.GetCustomAttributes(typeof(EnumMemberAttribute), false).Cast<EnumMemberAttribute>().FirstOrDefault()?.Value;
    }
}
