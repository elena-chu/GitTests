using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ws.Extensions.Serialization
{
    /// <summary>
    /// When serializing enums, adds the enum value as a comment
    /// </summary>
    public class CommentedEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var underlying = Nullable.GetUnderlyingType(objectType);

            return underlying == null ? objectType.IsEnum : underlying.IsEnum;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new InvalidOperationException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteValue(value);
            writer.WriteComment(value.ToString());
        }

        public override bool CanRead => false;
    }
}
