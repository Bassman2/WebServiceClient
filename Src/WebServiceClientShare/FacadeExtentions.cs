namespace GithubWebApi.Service;

internal static class Extentions
{
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
}
