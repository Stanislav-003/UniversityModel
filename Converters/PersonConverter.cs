using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniversityModel.Models;

namespace UniversityModel.Converters;

public class PersonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(Person).IsAssignableFrom(objectType);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        JObject jo = JObject.Load(reader);

        string? type = jo["Type"]?.Value<string>() ?? jo["type"]?.Value<string>();

        Person person = type switch
        {
            "Student" => new Student(),
            "Teacher" => new Teacher(),
            _ => throw new NotSupportedException($"Unknown person type: {type}")
        };

        using (var subReader = jo.CreateReader())
        {
            serializer.Populate(subReader, person);
        }

        return person;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}