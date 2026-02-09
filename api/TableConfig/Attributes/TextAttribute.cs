/*
* ==============================================================================
*
* FileName: TextAttribute.cs
* Created: 2026/1/20 14:29:57
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.Reflection;
using System;

namespace TableConfig.Attributes
{
    public class TextAttribute : Attribute
    {
        public TextAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }

    /// <summary>
    /// 枚举扩展属性
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获得枚举提示文本
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToText(this Enum obj)
        {
            Type type = obj.GetType();
            FieldInfo field = type.GetField(obj.ToString());
            TextAttribute attribute = (TextAttribute)field.GetCustomAttribute(typeof(TextAttribute));
            return attribute.Value;
        }

    }
}
