using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GlobalLib.Enum
{
    public static class EnumExtensions
    {
        public static string GetXmlEnumValue<T>(this T enumValue) where T : System.Enum
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            XmlEnumAttribute[] attributes = (XmlEnumAttribute[])fi.GetCustomAttributes(typeof(XmlEnumAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Name;
            }

            return enumValue.ToString();
        }
        public static string? GetXmlEnumValueNullable<T>(this T? enumValue) where T : struct, System.Enum
        {
            if (!enumValue.HasValue)
                return null;

            FieldInfo fi = enumValue.Value.GetType().GetField(enumValue.Value.ToString());

            XmlEnumAttribute[] attributes = (XmlEnumAttribute[])fi.GetCustomAttributes(typeof(XmlEnumAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Name;
            }

            return enumValue.Value.ToString();
        }
    }
}
