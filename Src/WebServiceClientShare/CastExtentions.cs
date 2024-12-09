
namespace WebServiceClient;

internal static class CastExtentions
{
    private const BindingFlags ConstructorDefault = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance;

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
}
