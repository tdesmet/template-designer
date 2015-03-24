using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace template_designer
{
    public class JsonToDictionaryConverter
    {
        public static Dictionary<string, object> DeserializeJsonToDictionary(string json)
        {

            var jObject = JObject.Parse(json);

            var dict = ParseObject(jObject);


            return dict;
        }

        private static Dictionary<string, object> ParseObject(JObject jObject)
        {
            var dict = new Dictionary<string, object>();
            foreach (var property in jObject.Properties())
            {
                if (property.Value.Type == JTokenType.Array)
                {
                    dict.Add(property.Name, ParseArray(property.Value.ToObject<JArray>()));
                }
                else if (property.Value.Type == JTokenType.Object)
                {
                    dict.Add(property.Name, ParseObject(property.Value.ToObject<JObject>()));
                }
                else
                {
                    dict.Add(property.Name, property.Value.ToString());
                }


            }
            return dict;
        }

        private static List<object> ParseArray(JArray value)
        {
            var list = new List<object>();
            foreach (var child in value.Children())
            {
                if (child.Type == JTokenType.Array)
                {
                    list.Add(ParseArray(child.ToObject<JArray>()));
                }
                else if (child.Type == JTokenType.Object)
                {
                    list.Add(ParseObject(child.ToObject<JObject>()));
                }
                else
                {
                    list.Add(child.ToString());
                }
            }
            return list;
        }
    }
}