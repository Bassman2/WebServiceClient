namespace WebServiceClient;

internal static class CastExtentions
{
    public static T? Cast<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this object? value) where T : class
    {
        return value is not null ? (T?)Activator.CreateInstance(typeof(T), value): null;
    }

    public static T? Cast<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this object? value, object service) where T : class
    {
        return value is not null ? (T?)Activator.CreateInstance(typeof(T), service, value) : null;
    }

    public static IEnumerable<T> Cast<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IEnumerable? value) where T : class
    {
        if (value is not null)
        {
            foreach (var item in value)
            {
                var inst = Activator.CreateInstance(typeof(T), item);
                if (inst != null)
                {
                    yield return (T)inst;
                }
            }
        }
    }

    public static IEnumerable<T> Cast<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this IEnumerable? value, object service) where T : class
    {
        if (value is not null)
        {
            foreach (var item in value)
            {
                var inst = Activator.CreateInstance(typeof(T), service, item);
                if (inst != null)
                {
                    yield return (T)inst;
                }
            }
        }
    }
}
