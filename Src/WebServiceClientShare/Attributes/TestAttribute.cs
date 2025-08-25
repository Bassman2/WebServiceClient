using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace WebServiceClient.Attributes;

public enum TestAttributeAccess
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
//public class TestAttribute(int value, TestAttributeAccess access, string name, bool flag) : Attribute
public class TestAttribute : Attribute
{
    public TestAttribute(int value, TestAttributeAccess access, string name, bool flag)
    {
        this.Value = value;
        this.Access = access;
        this.Name = name;
        this.Flag = flag;
    }

    public TestAttribute(string strx, int intx)
    {
        this.StrX = strx;
        this.IntX = intx;
    }

    //public int Value { get; set; } = value;
    //public TestAttributeAccess Access { get; } = access;
    //public string Name { get; } = name;
    //public bool Flag { get; } = flag;

    public int Value { get; } 
    public TestAttributeAccess Access { get; }
    public string? Name { get; }
    public bool Flag { get; }


    public int Value2 { get; set; }
    public TestAttributeAccess Access2 { get; set; }
    public string? Name2 { get; set; }
    public bool Flag2 { get; set; }

    public string? StrX { get; }

    public int IntX { get; }
}

