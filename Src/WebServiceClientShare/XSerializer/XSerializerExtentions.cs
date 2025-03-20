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
    public static T? XDeserialize<T>(this string? value, XName rootName) where T : IXSerializable, new()
    {
        if (value is null)
        {
            return default;
        }

        var res = new T();
        var doc = XDocument.Parse(value);
        var elm = doc.Element(rootName)!;
        if (elm is null) return default;
        res.ReadX(elm);
        return res;
    }

    /// <summary>
    /// Deserializes an XML string into an object of type T using the specified root element name and namespace.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize.</typeparam>
    /// <param name="value">The XML string to deserialize.</param>
    /// <param name="rootElementName">The root element name of the XML.</param>
    /// <param name="namespaceName">The namespace of the root element.</param>
    /// <returns>The deserialized object of type T, or default if the value is null.</returns>
    public static T? XDeserialize<T>(this string? value, string rootElementName,  string namespaceName) where T : IXSerializable, new()
    {
        if (value is null)
        {
            return default;
        }

        var res = new T();
        var doc = XDocument.Parse(value);
        var elm = doc.Element(XName.Get(rootElementName, namespaceName))!;
        if (elm is null) return default;
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


    private static XElement? GetElement(this XElement? elm, XName name)
    {
        if (elm is null) return null;

        string namespaceName = string.IsNullOrEmpty(name.NamespaceName) ? elm.Name.NamespaceName : name.NamespaceName;
        var xName = XName.Get(name.LocalName, namespaceName);
        return elm.Element(xName);
    }

    private static IEnumerable<XElement> GetElements(this XElement? elm, XName name)
    {
        if (elm is null) return [];

        string namespaceName = string.IsNullOrEmpty(name.NamespaceName) ? elm.Name.NamespaceName : name.NamespaceName;
        var xName = XName.Get(name.LocalName, namespaceName);
        return elm.Elements(xName);
    }


    /// <summary>
    /// Reads an element value as a string from an XElement.
    /// </summary>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the element.</param>
    /// <returns>The element value as a string, or null if the element does not exist.</returns>
    internal static string? ReadElementString(this XElement? elm, XName name)
    {
        return elm.GetElement(name)?.Value;
        //string namespaceName = string.IsNullOrEmpty(name.NamespaceName) ? elm?.Name.NamespaceName ?? String.Empty : name.NamespaceName;
        //var xName = XName.Get(name.LocalName, namespaceName);
        ////name.NamespaceName = name.NamespaceName ?? elm?.Name.NamespaceName;
        //return elm?.Element(xName)?.Value;
    }

    /// <summary>
    /// Reads an element value as an integer from an XElement.
    /// </summary>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the element.</param>
    /// <returns>The element value as an integer, or null if the element does not exist or cannot be parsed.</returns>
    internal static int? ReadElementInt(this XElement? elm, XName name)
    {
//        string? value = elm?.Element(name)?.Value;
        string? value = elm.GetElement(name)?.Value;
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
    internal static bool? ReadElementBool(this XElement? elm, XName name)
    {
        //        string? value = elm?.Element(name)?.Value;
        string? value = elm.GetElement(name)?.Value;
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
    internal static DateTime? ReadElementDateTime(this XElement? elm, XName name)
    {
        //string? value = elm?.Element(name)?.Value;
        string? value = elm.GetElement(name)?.Value;
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
    internal static T? ReadElementEnum<T>(this XElement? elm, XName name) where T : Enum
    {
        //string? value = elm?.Element(name)?.Value;
        string? value = elm.GetElement(name)?.Value;
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
    internal static List<T>? ReadElementEnums<T>(this XElement? elm, XName name) where T : Enum
    {
//        return elm?.Elements(name).Select(i => (T)(object)(int.TryParse(i.Value, out int val) ? val : 0))?.ToList();
        return elm.GetElements(name).Select(i => (T)(object)(int.TryParse(i.Value, out int val) ? val : 0))?.ToList();
    }

    /// <summary>
    /// Reads an element value as an object of type T from an XElement.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the element.</param>
    /// <returns>The element value as an object of type T, or default if the element does not exist.</returns>
    internal static T? ReadElementItem<T>(this XElement? elm, XName name) where T : IXSerializable, new()
    {
//        var child = elm?.Element(name);
        var child = elm.GetElement(name);
        if (child is null) return default;
        
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
    internal static List<string>? ReadElementStrings(this XElement? elm, XName name)
    {
        //return elm?.Elements(name).Select(e => e.Value).ToList();
        return elm.GetElements(name).Select(e => e.Value).ToList();
    }

    /// <summary>
    /// Reads multiple element values as a list of objects of type T from an XElement.
    /// </summary>
    /// <typeparam name="T">The type of the objects.</typeparam>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="name">The name of the elements.</param>
    /// <returns>A list of objects of type T, or null if the elements do not exist.</returns>
    internal static List<T>? ReadElementList<T>(this XElement? elm, XName name) where T : IXSerializable, new()
    {
        // return elm?.Elements(name).Select(e => { T i = new(); i.ReadX(e); return i; }).ToList();
        return elm.GetElements(name).Select(e => { T i = new(); i.ReadX(e); return i; }).ToList();
    }

    /// <summary>
    /// Reads multiple element values as a list of objects of type T from an XElement, within a specified array element.
    /// </summary>
    /// <typeparam name="T">The type of the objects.</typeparam>
    /// <param name="elm">The XElement to read from.</param>
    /// <param name="array">The name of the array element.</param>
    /// <param name="name">The name of the elements within the array.</param>
    /// <returns>A list of objects of type T, or null if the elements do not exist.</returns>
    internal static List<T>? ReadElementList<T>(this XElement? elm, XName array, XName name) where T : IXSerializable, new()
    {
        //return elm?.Element(array)?.Elements(name).Select(e => { T i = new(); i.ReadX(e); return i; }).ToList();
        return elm.GetElement(array).GetElements(name).Select(e => { T i = new(); i.ReadX(e); return i; }).ToList();
    }
}
