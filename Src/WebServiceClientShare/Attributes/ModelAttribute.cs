using System;
using System.Collections.Generic;
using System.Text;

namespace WebServiceClient.Attributes;

public enum ModelAccess
{
    Public,
    Internal,
    Protected,
    Private
}

/// <summary>
/// Indicates that a class is a model used by the WebServiceClient.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ModelAttribute(int value, ModelAccess access, string name, bool flag) : Attribute
{
    public int Value { get; } = value;
    public ModelAccess Access { get; } = access;
    public string Name { get; } = name;
    public bool Flag { get; } = flag;
    public int Value2 { get; set; }

    public ModelAccess Access2 { get; set; }

    public string? Name2 { get; set; }
    public bool Flag2 { get; set; }

}
