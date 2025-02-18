namespace WebServiceClient.XSerializer;

/// <summary>
/// Provides extension methods for XML serialization and deserialization.
/// </summary>
/// <remarks>The default XML Serializer is not trimmable and AOT compatible.</remarks>
public static class XSerializableExt
{
    private readonly static DateTime begin = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Deserializes an XML string into an object of type T.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize.</typeparam>
    /// <param name="value">The XML string to deserialize.</param>
    /// <param name="rootName">The root element name of the XML.</param>
    /// <returns>The deserialized object of type T, or default if the value is null.</returns>
    public static T? XDeserialize<T>(this string? value, string rootName) where T : IXSerializable, new()
    {
        if (value is null)
        {
            return default;
        }

        var res = new T();
        var doc = XDocument.Parse(value);
        var elm = doc.Element(rootName)!;
        res.ReadX(elm);
        return res;
    }

    /// <summary>
    /// Reads an attribute value as a string from an XElement.
    /// </summary>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the attribute.</param>
    /// <returns>The attribute value as a string, or null if the attribute does not exist.</returns>
    internal static string? ReadAttributeString(this XElement? elm, string name)
    {
        return elm?.Attribute(name)?.Value;
    }

    /// <summary>
    /// Reads an attribute value as an integer from an XElement.
    /// </summary>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the attribute.</param>
    /// <returns>The attribute value as an integer, or null if the attribute does not exist or cannot be parsed.</returns>
    internal static int? ReadAttributeInt(this XElement? elm, string name)
    {
        string? value = elm?.Attribute(name)?.Value;
        if (int.TryParse(value, out var val))
        {
            return val;
        }
        return null;
    }

    /// <summary>
    /// Reads an attribute value as a boolean from an XElement.
    /// </summary>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the attribute.</param>
    /// <returns>The attribute value as a boolean, or null if the attribute does not exist or cannot be parsed.</returns>
    internal static bool? ReadAttributeBool(this XElement? elm, string name)
    {
        string? value = elm?.Attribute(name)?.Value;
        if (int.TryParse(value, out var val))
        {
            return val != 0;
        }
        return null;
    }

    /// <summary>
    /// Reads an attribute value as an enum from an XElement.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the attribute.</param>
    /// <returns>The attribute value as an enum, or default if the attribute does not exist or cannot be parsed.</returns>
    internal static T? ReadAttributeEnum<T>(this XElement? elm, string name) where T : Enum
    {
        string? value = elm?.Attribute(name)?.Value;
        if (int.TryParse(value, out var val))
        {
            return (T)(object)val;
        }
        return default;
    }

    /// <summary>
    /// Reads an element value as a string from an XElement.
    /// </summary>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the element.</param>
    /// <returns>The element value as a string, or null if the element does not exist.</returns>
    internal static string? ReadElementString(this XElement? elm, string name)
    {
        return elm?.Element(name)?.Value;
    }

    /// <summary>
    /// Reads an element value as an integer from an XElement.
    /// </summary>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the element.</param>
    /// <returns>The element value as an integer, or null if the element does not exist or cannot be parsed.</returns>
    internal static int? ReadElementInt(this XElement? elm, string name)
    {
        string? value = elm?.Element(name)?.Value;
        if (int.TryParse(value, out var val))
        {
            return val;
        }
        return null;
    }

    /// <summary>
    /// Reads an element value as a boolean from an XElement.
    /// </summary>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the element.</param>
    /// <returns>The element value as a boolean, or null if the element does not exist or cannot be parsed.</returns>
    internal static bool? ReadElementBool(this XElement? elm, string name)
    {
        string? value = elm?.Element(name)?.Value;
        if (int.TryParse(value, out var val))
        {
            return val != 0;
        }
        return null;
    }

    /// <summary>
    /// Reads an element value as a DateTime from an XElement.
    /// </summary>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the element.</param>
    /// <returns>The element value as a DateTime, or null if the element does not exist or cannot be parsed.</returns>
    internal static DateTime? ReadElementDateTime(this XElement? elm, string name)
    {
        string? value = elm?.Element(name)?.Value;
        if (long.TryParse(value, out var val))
        {
            return begin.AddSeconds(val); //.ToLocalTime();
        }
        return null;
    }

    /// <summary>
    /// Reads an element value as an enum from an XElement.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the element.</param>
    /// <returns>The element value as an enum, or default if the element does not exist or cannot be parsed.</returns>
    internal static T? ReadElementEnum<T>(this XElement? elm, string name) where T : Enum
    {
        string? value = elm?.Element(name)?.Value;
        if (int.TryParse(value, out var val))
        {
            return (T)(object)val;
        }
        return default;
    }

    /// <summary>
    /// Reads multiple element values as a list of enums from an XElement.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the elements.</param>
    /// <returns>A list of enum values, or null if the elements do not exist or cannot be parsed.</returns>
    internal static List<T>? ReadElementEnums<T>(this XElement? elm, string name) where T : Enum
    {
        return elm?.Elements(name).Select(i => (T)(object)(int.TryParse(i.Value, out int val) ? val : 0))?.ToList();
    }

    /// <summary>
    /// Reads an element value as an object of type T from an XElement.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the element.</param>
    /// <returns>The element value as an object of type T, or default if the element does not exist.</returns>
    internal static T? ReadElementItem<T>(this XElement? elm, string name) where T : IXSerializable, new()
    {
        var child = elm?.Element(name);
        if (child is null)
        {
            return default;
        }
        var item = new T();
        item.ReadX(child);
        return item;

    }

    /// <summary>
    /// Reads multiple element values as a list of strings from an XElement.
    /// </summary>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the elements.</param>
    /// <returns>A list of string values, or null if the elements do not exist.</returns>
    internal static List<string>? ReadElementStrings(this XElement? elm, string name)
    {
        return elm?.Elements(name).Select(e => e.Value).ToList();
    }

    /// <summary>
    /// Reads multiple element values as a list of objects of type T from an XElement.
    /// </summary>
    /// <typeparam name="T">The type of the objects.</typeparam>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the elements.</param>
    /// <returns>A list of objects of type T, or null if the elements do not exist.</returns>
    internal static List<T>? ReadElementList<T>(this XElement? elm, string name) where T : IXSerializable, new()
    {
        return elm?.Elements(name).Select(e => { T i = new(); i.ReadX(e); return i; }).ToList();
    }

    /// <summary>
    /// Reads multiple element values as a list of objects of type T from an XElement, within a specified array element.
    /// </summary>
    /// <typeparam name="T">The type of the objects.</typeparam>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="array">The name of the array element.</param>
    /// <param name="name">The name of the elements within the array.</param>
    /// <returns>A list of objects of type T, or null if the elements do not exist.</returns>
    internal static List<T>? ReadElementList<T>(this XElement? elm, string array, string name) where T : IXSerializable, new()
    {
        return elm?.Element(array)?.Elements(name).Select(e => { T i = new(); i.ReadX(e); return i; }).ToList();
    }
}
