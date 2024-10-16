using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bank.Common;

public static class Serde
{
    private static readonly JsonSerializerOptions _defaultSerializerSettings =
        new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    public static string Serialize(this object obj, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Serialize(obj, options ?? _defaultSerializerSettings);
    }

    public static byte[] Serialize(this string rawObject)
    {
        return Encoding.UTF8.GetBytes(rawObject);
    }

    public static T? Deserialize<T>(this byte[] bytes, JsonSerializerOptions? options = null)
    {
        return Deserialize<T>(Encoding.UTF8.GetString(bytes), options ?? _defaultSerializerSettings);
    }

    public static T? Deserialize<T>(this string rawObject, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Deserialize<T>(rawObject, options ?? _defaultSerializerSettings);
    }


    public static StringContent ToJsonContent(this object obj, JsonSerializerOptions? options = null)
    {
        // Create a JsonContent object with the serialized JSON string
        return new StringContent(obj.Serialize(options), Encoding.UTF8,
            MediaTypeNames.Application.Json);
    }
}