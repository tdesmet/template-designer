using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace template_designer
{
    public class JsonToDictionaryConverter
    {
        public static Dictionary<string, object> DeserializeJsonToDictionary(string json)
        {
            var jObject = JObject.Parse(json);

            return ParseJObject(jObject);
        }

        private static Dictionary<string, object> ParseJObject(JObject jObject)
        {
            return jObject.Properties().ToDictionary(property => property.Name, property => ParseValue(property.Value));
        }

        private static object ParseValue(JToken value)
        {
            switch (value.Type)
            {
                case JTokenType.Array:
                    return ParseJArray(value.ToObject<JArray>());
                case JTokenType.Object:
                    return ParseJObject(value.ToObject<JObject>());
                case JTokenType.Boolean:
                    return value.ToObject<bool>();
                case JTokenType.Integer:
                    return value.ToObject<int>();
                case JTokenType.Float:
                    return value.ToObject<float>();
                default:
                    return value.ToString();
            }
        }

        private static List<object> ParseJArray(JArray value)
        {
            return value.Children().Select(ParseValue).ToList();
        }
    }
}