using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace _DataPersistance
{
    public abstract class StatConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Stat).IsAssignableFrom(objectType);
        }

        protected abstract StatSerializer Serializer { get; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            
            JToken jToken = JToken.ReadFrom(reader);
            var obj = Create(objectType, jToken);

            if (obj != null)
            {
                serializer.Populate(jToken.CreateReader(), obj);
            }
            return obj;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                JObject o = (JObject)t;
                o.WriteTo(writer);
            }
        }

        protected Stat Create(Type objectType, JToken jToken)
        {
            string statName = jToken.Value<string>("Name");
            return Serializer.CreateInstance(statName);
        }
    }    
}