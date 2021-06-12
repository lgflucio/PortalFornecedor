using System;

namespace Repository.Entities.RFE_ENTITIES
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class XmlType : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FileType : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ClobType : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class VarcharType : Attribute
    {
    }
}
