/*
* ==============================================================================
*
* FileName: LongTypeConverter.cs
* Created: 2026/1/21 16:23:12
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using Newtonsoft.Json;
using System;

namespace TableConfig.Converters
{
    public class LongTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(long) || objectType == typeof(long?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null || string.IsNullOrWhiteSpace(reader.Value.ToString()))
            {
                return null!;
            }

            return Convert.ToInt64(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            if ((long)value > 10000000000000000)
            {
                writer.WriteValue(value.ToString());
                return;
            }

            writer.WriteValue(value);
        }
    }
}
