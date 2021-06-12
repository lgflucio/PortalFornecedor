using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Extensions
{
    public class XmlTypesExtension
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
}
