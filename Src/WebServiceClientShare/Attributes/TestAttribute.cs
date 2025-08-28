namespace WebServiceClient.Attributes;

/// <summary>
/// Specifies the access level for the <see cref="TestAttribute"/>.
/// </summary>
public enum TestAttributeAccess
{
    /// <summary>
    /// Public access level.
    /// </summary>
    Public,
    /// <summary>
    /// Internal access level.
    /// </summary>
    Internal,
    /// <summary>
    /// Protected access level.
    /// </summary>
    Protected,
    /// <summary>
    /// Private access level.
    /// </summary>
    Private
}

/// <summary>
/// Indicates that a class is a model used by the WebServiceClient.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
//public class TestAttribute(int value, TestAttributeAccess access, string name, bool flag) : Attribute
public class TestAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestAttribute"/> class with the specified value, access level, name, and flag.
    /// </summary>
    /// <param name="value">The integer value associated with the attribute.</param>
    /// <param name="access">The access level for the attribute.</param>
    /// <param name="name">The name associated with the attribute.</param>
    /// <param name="flag">A boolean flag for the attribute.</param>
    public TestAttribute(int value, TestAttributeAccess access, string name, bool flag)
    {
        this.Value = value;
        this.Access = access;
        this.Name = name;
        this.Flag = flag;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestAttribute"/> class with the specified string and integer values.
    /// </summary>
    /// <param name="strx">The string value associated with the attribute.</param>
    /// <param name="intx">The integer value associated with the attribute.</param>
    public TestAttribute(string strx, int intx)
    {
        this.StrX = strx;
        this.IntX = intx;
    }

    /// <summary>
    /// Gets the integer value associated with the attribute.
    /// </summary>
    public int Value { get; }
  /// <summary>
    /// Gets the access level for the attribute.
    /// </summary>
    public TestAttributeAccess Access { get; }
    /// <summary>
    /// Gets the name associated with the attribute.
    /// </summary>
    public string? Name { get; }
    /// <summary>
    /// Gets a boolean flag for the attribute.
    /// </summary>
    public bool Flag { get; }


    /// <summary>
    /// Gets or sets the secondary integer value associated with the attribute.
    /// </summary>
    public int Value2 { get; set; }
    /// <summary>
    /// Gets or sets the secondary access level for the attribute.
    /// </summary>
    public TestAttributeAccess Access2 { get; set; }
    /// <summary>
    /// Gets or sets the secondary name associated with the attribute.
    /// </summary>
    public string? Name2 { get; set; }
    /// <summary>
    /// Gets or sets the secondary boolean flag for the attribute.
    /// </summary>
    public bool Flag2 { get; set; }

    /// <summary>
    /// Gets the string value associated with the attribute.
    /// </summary>
    public string? StrX { get; }

    /// <summary>
    /// Gets the integer value associated with the attribute.
    /// </summary>
    public int IntX { get; }
}

