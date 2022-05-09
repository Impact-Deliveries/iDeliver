using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.Common
{
    public class CustomDateConverter : DateTimeConverterBase
    {
        private const string Format = "dd-MM-yyyy";

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString(Format));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return DateTime.UtcNow;

            var s = reader.Value.ToString();
            
            DateTime result;

            if (DateTime.TryParseExact(s, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }

            return DateTime.ParseExact(s,Format,CultureInfo.InvariantCulture);
        }
    }
}
