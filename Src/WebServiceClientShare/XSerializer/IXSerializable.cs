namespace WebServiceClient.XSerializer;

// 

/// <summary>
/// Defines methods for XML serialization and deserialization.
/// </summary>
/// <remarks>The default XML Serializer is not trimmable and AOT compatible.</remarks>
public interface IXSerializable
{
    /// <summary>
    /// Reads the object from the specified XML element.
    /// </summary>
    /// <param name="elm">The XML element to read from.</param>
    void ReadX(XElement elm);

    // /// <summary>
    // /// Writes the object to the specified XML element.
    // /// </summary>
    // /// <param name="elm">The XML element to write to.</param>
    // void WriteX(XElement elm);
}

